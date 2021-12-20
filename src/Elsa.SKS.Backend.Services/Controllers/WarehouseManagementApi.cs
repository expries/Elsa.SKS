/*
 * Parcel Logistics Service
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.20.2
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Elsa.SKS.Backend.Services.Attributes;
using Elsa.SKS.Backend.BusinessLogic.Exceptions;
using Elsa.SKS.Backend.BusinessLogic.Interfaces;
using Elsa.SKS.Backend.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Elsa.SKS.Backend.Services.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class WarehouseManagementApiController : ControllerBase
    {
        private readonly IWarehouseLogic _warehouseLogic;

        private readonly IMapper _mapper;

        private readonly ILogger<WarehouseManagementApiController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="warehouseLogic"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public WarehouseManagementApiController(IWarehouseLogic warehouseLogic, IMapper mapper, ILogger<WarehouseManagementApiController> logger)
        {
            _warehouseLogic = warehouseLogic;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Exports the hierarchy of Warehouse and Truck objects. 
        /// </summary>
        /// <response code="200">Successful response</response>
        /// <response code="400">An error occurred loading.</response>
        /// <response code="404">No hierarchy loaded yet.</response>
        [HttpGet]
        [Route("/warehouse")]
        [ValidateModelState]
        [SwaggerOperation("ExportWarehouses")]
        [SwaggerResponse(statusCode: 200, type: typeof(Warehouse), description: "Successful response")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "An error occurred loading.")]
        public IActionResult ExportWarehouses()
        {
            try
            {
                var warehouseEntity = _warehouseLogic.ExportWarehouses();
                var result = _mapper.Map<Warehouse>(warehouseEntity);
                _logger.LogInformation("Export warehouses response: Ok");
                return Ok(result);
            }
            catch (WarehouseHierarchyNotLoadedException)
            {
                _logger.LogInformation("Loading warehouse hierarchy error");
                return NotFound();
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Export warehouses error");
                var error = new Error { ErrorMessage = ex.Message };
                return BadRequest(error);
            }
        }

        /// <summary>
        /// Get a certain warehouse or truck by code
        /// </summary>
        /// <param name="code"></param>
        /// <response code="200">Successful response</response>
        /// <response code="400">An error occurred loading.</response>
        /// <response code="404">No hop with the specified id could be found.</response>
        [HttpGet]
        [Route("/warehouse/{code}")]
        [ValidateModelState]
        [SwaggerOperation("GetWarehouse")]
        [SwaggerResponse(statusCode: 200, type: typeof(Hop), description: "Successful response")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "An error occurred loading.")]
        public IActionResult GetWarehouse([FromRoute][Required] string code)
        {
            try
            {
                var warehouseEntity = _warehouseLogic.GetWarehouse(code);
                var result = _mapper.Map<Warehouse>(warehouseEntity);
                _logger.LogInformation("Get warehouse response: Ok");
                return Ok(result);
            }
            catch (WarehouseNotFoundException)
            {
                _logger.LogInformation("Warehouse not found error");
                return NotFound();
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Get warehouse error");
                var error = new Error { ErrorMessage = ex.Message };
                return BadRequest(error);
            }
        }

        /// <summary>
        /// Imports a hierarchy of Warehouse and Truck objects. 
        /// </summary>
        /// <param name="body"></param>
        /// <response code="200">Successfully loaded.</response>
        /// <response code="400">The operation failed due to an error.</response>
        [HttpPost]
        [Route("/warehouse")]
        [ValidateModelState]
        [SwaggerOperation("ImportWarehouses")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "The operation failed due to an error.")]
        public IActionResult ImportWarehouses([FromBody] Warehouse body)
        {
            try
            {
                var entity = _mapper.Map<BusinessLogic.Entities.Warehouse>(body);
                _warehouseLogic.ImportWarehouses(entity);
                _logger.LogInformation("Import warehouse response: Ok");
                return Ok();
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Import warehouse error");
                var error = new Error { ErrorMessage = ex.Message };
                return BadRequest(error);
            }
        }
    }
}