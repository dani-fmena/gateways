using System.Net.Mime;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using gateway.domain;
using gateway.domain.Dto;
using gateway.api.V1.Services;
using gateway.domain.Entities;


namespace gateway.api.V1.Controllers.Management
{

    /// <summary>
    /// Peripheral Management endpoints for staff / administrator users
    /// </summary>
    /// <remarks>
    /// Administration scope level controller.
    /// Ac == Admin / Staff Controller
    /// </remarks>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = EndpointTags.TgPeripherals)]
    [Route("v{version:apiVersion}/mngmt/[controller]")]
    [ApiController]
    public class AcPeripheral : CBase
    {
        private readonly ISvcPeripherals _svcPeripherals;

        public AcPeripheral(ISvcPeripherals svcPeripherals)
        {
            _svcPeripherals = svcPeripherals;
        }
        
        /// <summary>
        /// List of <c>Peripherals</c>
        /// </summary>
        /// <remarks>
        /// Useful for getting a list of <c><see cref="Peripheral"/></c> rows according to a given <see cref="Gateway"/> identifier.
        /// </remarks>
        /// <returns>A list of <see cref="Peripheral"/> rows</returns>
        [HttpGet("rows")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Problem), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ICollection<DtoPeripheralRow>>> GetRows()
        {
            var responseList = await _svcPeripherals.GetRows();
            if (_svcPeripherals.HasProblem) return ReProblem(_svcPeripherals.Problem);

            return Ok(responseList);
        }
        
        /// <summary>
        /// List of <c>Peripherals</c>
        /// </summary>
        /// <remarks>
        /// Useful for getting a list of <c><see cref="Peripheral"/></c> rows according to a given <see cref="Gateway"/> identifier.
        /// </remarks>
        /// <returns>A list of <see cref="Peripheral"/> rows</returns>
        [HttpGet("rows/gateway/{gatewayId:int}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Problem), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ICollection<DtoPeripheralRow>>> GetRowsByGateway(int gatewayId)
        {
            var responseList = await _svcPeripherals.GetRowsByGateway(gatewayId);
            if (_svcPeripherals.HasProblem) return ReProblem(_svcPeripherals.Problem);

            return Ok(responseList);
        }
        
        /// <summary>Get specific <c>peripherals row</c></summary>
        /// <remarks>
        /// Get specific peripherals row given the identifier as request parameter
        /// </remarks>
        /// <param name="id" example="68">peripheral Id</param>
        /// <response code="200">Return peripheral row for the datatable</response>
        [HttpGet("rows/{id:int}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Problem), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DtoPeripheralRow>> GetRowById(int id)
        {
            var gateway = await _svcPeripherals.GetRowById(id);
            if (_svcPeripherals.HasProblem) return ReProblem(_svcPeripherals.Problem);

            return Ok(gateway);
        }
        
        /// <summary>Create a peripheral</summary>
        /// <remarks>
        /// Create a peripheral from the staff management section, according to the data present on the request
        /// 
        /// ❗ In post actions, the entity ID must be 0, or absent
        /// </remarks>
        /// <param name="postData">Peripheral input data</param>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Problem), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DtoPeripheralRow>> Post(DtoPeripheralIn postData)
        {
            // try to insert the new catalog
            var peripheral = await _svcPeripherals.Create(postData);
            if (_svcPeripherals.HasProblem || peripheral.Id <= 0) return ReProblem(_svcPeripherals.Problem);
            
            return CreatedAtAction(nameof(GetRowById), new {id = peripheral.Id, version = "1"}, peripheral);
        }
        
        /// <summary>Update a <c>Peripheral</c></summary>
        /// <remarks>Update an existing <c>Peripheral</c> according to the data given with the request</remarks>
        /// <param name="putData">Input peripheral data to update with</param>
        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Problem), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Problem), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DtoPeripheralRow>> Put(DtoPeripheralUpdateIn putData)
        {
            var updatedGateway = await _svcPeripherals.Update(putData);
            if (_svcPeripherals.HasProblem) return ReProblem(_svcPeripherals.Problem);
            
            return Ok(updatedGateway);
        }
        
        /// <summary>Delete peripherals</summary>
        /// <remarks>
        /// Delete a bunch of peripherals (or just one) corresponding to the given identifiers list.
        /// </remarks>
        /// <param name="ids">Peripherals identifiers to be deleted</param>
        [HttpDelete("batch")]
        [Consumes(MediaTypeNames.Application.Json)] 
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Problem), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteBulk(ICollection<int> ids)
        {
            await _svcPeripherals.BatchDelete(ids);                       // ar == was deleted
            if (_svcPeripherals.HasProblem) return ReProblem(_svcPeripherals.Problem);
            
            return NoContent();
        }
    }
}