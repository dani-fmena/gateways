using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using gateway.domain;
using gateway.domain.Dto;
using gateway.api.V1.Services;
using gateway.domain.Entities;


namespace gateway.api.V1.Controllers.Management
{

    /// <summary>
    /// Gateway Management endpoints for staff / administrator users
    /// </summary>
    /// <remarks>
    /// Administration scope level controller.
    /// Ac == Admin / Staff Controller
    /// </remarks>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = EndpointTags.TgGateways)]
    [Route("v{version:apiVersion}/mngmt/[controller]")]
    [ApiController]
    public class AcGateway : CBase
    {
        private readonly ISvcGateway _svcGateway;

        public AcGateway(ISvcGateway svcGateway)
        {
            _svcGateway = svcGateway;
        }
        
        /// <summary>Get specific <c>gateway row</c></summary>
        /// <remarks>
        /// Get specific gateway row given, the catalog identifier as request parameter
        /// </remarks>
        /// <param name="id" example="68">gateway Id</param>
        /// <response code="200">Return gateway row for the datatable</response>
        [HttpGet("row/{id:int}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Problem), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Problem), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Problem), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DtoGatewayRow>> GetRowById(int id)
        {
            var gateway = await _svcGateway.GetRowById(id);
            if (_svcGateway.HasProblem) return ReProblem(_svcGateway.Problem);

            return Ok(gateway);
        }
        
        /// <summary>
        /// List of <c>Gateways</c>
        /// </summary>
        /// <remarks>
        /// Useful for getting a list of <c><see cref="Gateway"/></c> rows **with some information** related to each rows
        /// </remarks>
        /// <returns>A list of <see cref="Gateway"/> rows</returns>
        [HttpGet("rows")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Problem), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ICollection<Gateway>>> GetRows()
        {
            var responseList = await _svcGateway.GetRows();
            if (_svcGateway.HasProblem) return ReProblem(_svcGateway.Problem);

            return Ok(responseList);
        }
        
        /// <summary> Create a gateway from the staff management zone </summary>
        /// <remarks>
        /// Create a gateway according to the data present on the request
        /// 
        /// ❗ In post actions, the entity ID must be 0, or absent
        /// </remarks>
        /// <param name="postData">Gateway input data</param>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Problem), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Problem), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Problem), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<DtoGatewayRow>> Post(DtoGatewayIn postData)
        {
            // try to insert the new catalog
            var gateway = await _svcGateway.CreateGateway(postData);
            if (_svcGateway.HasProblem || gateway.Id <= 0) return ReProblem(_svcGateway.Problem);
            
            return CreatedAtAction(nameof(GetRowById), new {id = gateway.Id, version = "1"}, gateway);
        }
    }
}