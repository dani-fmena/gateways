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
            var responseList = await _svcGateway.GetGatewaysRows();
            if (_svcGateway.HasProblem) return ReProblem(_svcGateway.Problem);

            return Ok(responseList);
        }
    }
}