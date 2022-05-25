using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using gateway.domain.Dto;

namespace gateway.api.V1.Controllers
{
    /// <summary>
    /// This a default controller class with a collection of commons methods
    /// </summary>
    /// <remarks>
    /// This is a generalization class
    /// </remarks>
    public abstract class CBase : ControllerBase
    {
        /// <summary>
        /// It comes in handy when there is a problem to notify to the client. The HTTP response
        /// status code is accordingly with the parameter status property.
        /// </summary>
        /// <param name="problem">Error information</param>
        /// <returns>An <see cref="Problem"/> json struct </returns>
        protected JsonResult ReProblem(Problem problem)
        {
            Response.StatusCode = problem.Status;
            return new JsonResult(problem);
        }
        
        /// <summary>
        /// Construct a bad request response with HTTP 400 error code.
        /// </summary>
        /// <remarks> Override the <see cref="Problem"/> status accordingly</remarks>
        /// <param name="problem">Error information</param>
        /// <returns>An <see cref="Problem"/> json struct </returns>
        protected JsonResult ReBadRequest(Problem problem)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            
            return new JsonResult(new Problem 
            {
                Status = Response.StatusCode, 
                Type = problem.Type, 
                Title = problem.Title
            });
        }
    }
}