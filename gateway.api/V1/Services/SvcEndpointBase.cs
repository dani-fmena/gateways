using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;

using AutoMapper;

using gateway.dal;
using gateway.domain;
using gateway.domain.Dto;

namespace gateway.api.V1.Services
{
    /// <summary>
    /// Interface for direct endpoint controller services.
    /// This services handles business login entities actions / operations 
    /// </summary>
    public interface ISvcEndpointBase
    {
        public Problem Problem { get; }
        public bool HasProblem { get; }
    }
    
    /// <summary>
    /// Service general base class. It has some base and common methods and properties.
    /// </summary>
    public class SvcEndpointBase
    {
        /// <summary>
        /// Object mapper (Automapper) lib instance
        /// </summary>
        protected readonly IMapper Mapper;
        
        /// <summary>
        /// Data access layer instance (db context dependency). 
        /// </summary>
        public readonly ADbContext Dal;

        protected SvcEndpointBase(ADbContext dal, IMapper mapper)
        {
            Dal = dal;
            Mapper = mapper;
        }
        
        /// <summary>
        /// This property contains error information related to some model / dto / object member. The dictionary
        /// tribute his info lib.Schema.Dto.Problem cref="Problem.Errors"/> property object.
        /// </summary>
        private readonly Dictionary<string, List<string>> _memberErrors = new Dictionary<string, List<string>>();

        /// <summary>
        /// The service has a <see cref="gateway.domain.Dto.Problem"/> object to hold all the error related information
        /// that may occurs during the service logic execution.
        /// </summary>
        private readonly Problem _problem = new Problem {Status = StatusCodes.Status400BadRequest};

        /// <summary>
        /// Allow to check if problems or error was register in the service instance.
        /// </summary>
        /// <returns>Return <c>true</c> if the service has a Problem properly defined</returns>
        public bool HasProblem => _problem.Type != null || _memberErrors.Any();

        /// <summary>
        /// Property for getting the problem service instance
        /// </summary>
        /// <remarks>
        /// The implementation will enforce lib.Schema.Dto.Problem cref="Problem.Type"/> property has been set previously.
        /// It isn't an internal server error will be returned by the getter.    
        /// </remarks>
        public Problem Problem
        {
            get
            {
                // We enforce the Problem.Type property was filled
                if (String.IsNullOrEmpty(_problem.Type))
                    return new Problem { Status = StatusCodes.Status500InternalServerError, Type = TksProblem.ErrNullProbType, Title = DtlProblem.DtlProblemTypeNotNull};
                
                _problem.Errors = _memberErrors;
                return _problem;
            }
        }
        
        /// <summary>
        /// Set the given error information in the <see cref="gateway.domain.Dto.Problem"/> service instance with the given params
        /// </summary>
        /// <param name="type" example="Err.Forbidden">Error type token</param>
        /// <param name="newStatus" example="403">New HTTP  status code</param>
        /// <param name="title" example="Something was really good">Error detail</param>
        protected void AddProblem(string type, int newStatus, string title)
        {
            _problem.Type = type;
            _problem.Status = newStatus;
            _problem.Title = title;
        }

        /// <summary>
        /// Set the given error information in the <see cref="gateway.domain.Dto.Problem"/> service instance with the given params
        /// </summary>
        /// <param name="type" example="Err.Forbidden">Error type token</param>
        /// <param name="newStatus" example="403">New HTTP  status code</param>
        protected void AddProblem(string type, int newStatus = 400)
        {
            _problem.Type = type;
            if (newStatus != 400) _problem.Status = newStatus;
        }

        /// <summary>
        /// Set the given error information lib.Schema.Dto.Problem cref="Problem.Errors"/> service instance with the given params. Accordingly
        /// with the params, adds member errors to this service <c>MemberError</c> dictionary.
        /// </summary>
        /// <param name="member" example="name">The member with the problem</param>
        /// <param name="msg" example="The field is incorret because ...">Error message</param>
        /// <param name="newStatus" example="403">New HTTP status code</param>
        /// <param name="type" example="Err.Forbidden">Error type token</param>
        /// <param name="title" example="Something was really good">Error detail</param>
        protected void AddProblem(string member, string msg, int newStatus = 400, string type = null, string title = null)
        {
            if (newStatus != 400) _problem.Status = newStatus;
            if (type != null) _problem.Type = type;
            if (title != null) _problem.Title = title;

            // adding the member errors
            if (!_memberErrors.ContainsKey(member))
            {
                _memberErrors.Add(member, new List<string>());
                _memberErrors[member].Add(msg);
            }
            else
            {
                _memberErrors[member].Add(msg);
            }
        }

        /// <summary>
        /// Comes in handy as shortcut with entity's member validation check. Add a entity member problem.
        /// </summary>
        /// <remarks>
        /// Wrapper around <see cref="AddProblem(string,int,string)"/> overloaded methods
        /// </remarks>
        /// <param name="member">Entity's member with the issue</param>
        /// <param name="msg">Problem message</param>
        public void AddMemberProblem(string member, string msg)
        {
            AddProblem(member, msg, StatusCodes.Status400BadRequest, TksProblem.ErrInvEntity,
                DtlProblem.DtlInvalidEntityData);
        }

        /// <summary>
        /// Comes in handy as shortcut when the given data isn't valid. Add a entity member problem. 
        /// </summary>
        /// <remarks>
        /// Wrapper around <see cref="AddProblem(string,int,string)"/> overloaded methods
        /// </remarks>
        /// <param name="msg">Problem message</param>
        public void AddWrongDataProblem(string msg)
        {
            AddProblem(TksProblem.ErrBadArgs, StatusCodes.Status400BadRequest, msg);
        }

        /// <summary>
        /// Directly add a 'Resource Not Found' problem to the service
        /// </summary>
        /// <remarks>
        /// Wrapper around <see cref="AddProblem(string,int,string)"/> overloaded method
        /// </remarks>
        /// <param name="msg" example="Resource wasn't found">Problem message</param>
        protected void AddNotFoundProblem(string msg = DtlProblem.DtlNotFound)
        {
            AddProblem(TksProblem.ErrResNotFound, StatusCodes.Status404NotFound, msg);
        }

        /// <summary>
        /// Directly add a 'Database Access Layer' problem to the service
        /// </summary>
        /// <remarks>
        /// Wrapper around <see cref="AddProblem(string,int,string)"/> overloaded method
        /// </remarks>
        /// <param name="msg" example="A problem occour in the database">Problem message</param>
        protected void AddDalProblem(string msg)
        {
            AddProblem(TksProblem.ErrDal, StatusCodes.Status502BadGateway, msg);
        }

        /// <summary>
        /// Directly add unprocessable entity problem to the scope service
        /// </summary>
        /// <remarks>
        /// Wrapper around <see cref="AddProblem(string,int,string)"/> overloaded method
        /// </remarks>
        /// <param name="msg" example="A problem with the entity">Problem message</param>
        private void AddDuplicatedKeyProblem(string msg)
        {
            AddProblem(TksProblem.ErrDalDuplicateKey, StatusCodes.Status422UnprocessableEntity, msg);
        }

        /// <summary>
        /// Directly add a 'Invalid Argument' problem to the service 
        /// </summary>
        /// <remarks>
        /// Wrapper around <see cref="AddProblem(string,int,string)"/> overloaded method
        /// </remarks>
        /// <param name="msg" example="A problem occour in the database">Problem message</param>
        protected void AddInternalProblem(string msg)
        {
            AddProblem(TksProblem.ErrBadArgs, StatusCodes.Status500InternalServerError, msg);
        }

        /// <summary>
        /// Directly add a 'Not found or Deleted' problem to the service. This kind of problem is used when
        /// soft deleted entities are involved in the actions / operation 
        /// </summary>
        /// <remarks>
        /// Wrapper around <see cref="AddProblem(string,int,string)"/> overloaded method
        /// </remarks>
        /// <param name="msg" example="A problem occour in the database">Problem message</param>
        protected void AddNotFoundDeletedProblem(string msg)
        {
            AddProblem(TksProblem.ErrNotFoundOrDeleted, StatusCodes.Status404NotFound, msg);
        }
    }
}