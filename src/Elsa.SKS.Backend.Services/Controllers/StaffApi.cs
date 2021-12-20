/*
 * Parcel Logistics Service
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.20.2
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
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
    public class StaffApiController : ControllerBase
    {
        private readonly IParcelTrackingLogic _parcelTrackingLogic;

        private readonly IMapper _mapper;

        private readonly ILogger<StaffApiController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parcelTrackingLogic"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public StaffApiController(IParcelTrackingLogic parcelTrackingLogic, IMapper mapper, ILogger<StaffApiController> logger)
        {
            _parcelTrackingLogic = parcelTrackingLogic;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Report that a Parcel has been delivered at it&#x27;s final destination address. 
        /// </summary>
        /// <param name="trackingId">The tracking ID of the parcel. E.g. PYJRB4HZ6 </param>
        /// <response code="200">Successfully reported hop.</response>
        /// <response code="400">The operation failed due to an error.</response>
        /// <response code="404">Parcel does not exist with this tracking ID. </response>
        [HttpPost]
        [Route("/parcel/{trackingId}/reportDelivery/")]
        [ValidateModelState]
        [SwaggerOperation("ReportParcelDelivery")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "The operation failed due to an error.")]
        public IActionResult ReportParcelDelivery(
            [FromRoute][Required][RegularExpression("^[A-Z0-9]{9}$")] string trackingId)
        {
            try
            {
                _parcelTrackingLogic.ReportParcelDelivery(trackingId);
                _logger.LogInformation("Report parcel delivery response: Ok");
                return Ok();
            }
            catch (ParcelNotFoundException)
            {
                _logger.LogInformation("Parcel not found");
                return NotFound();
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Reporting parcel error");
                var error = new Error { ErrorMessage = ex.Message };
                return BadRequest(error);
            }
        }

        /// <summary>
        /// Report that a Parcel has arrived at a certain hop either Warehouse or Truck. 
        /// </summary>
        /// <param name="trackingId">The tracking ID of the parcel. E.g. PYJRB4HZ6 </param>
        /// <param name="code">The Code of the hop (Warehouse or Truck).</param>
        /// <response code="200">Successfully reported hop.</response>
        /// <response code="400">The operation failed due to an error.</response>
        /// <response code="404">Parcel does not exist with this tracking ID or hop with code not found. </response>
        [HttpPost]
        [Route("/parcel/{trackingId}/reportHop/{code}")]
        [ValidateModelState]
        [SwaggerOperation("ReportParcelHop")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "The operation failed due to an error.")]
        public IActionResult ReportParcelHop(
            [FromRoute][Required][RegularExpression("^[A-Z0-9]{9}$")] string trackingId,
            [FromRoute][Required][RegularExpression("^[A-Z]{4}\\d{1,4}$")] string code)
        {
            try
            {
                _parcelTrackingLogic.ReportParcelHop(trackingId, code);
                _logger.LogInformation("Report parcel hop response: Ok");
                return Ok();
            }
            catch (Exception ex) when (ex is ParcelNotFoundException or HopNotFoundException)
            {
                _logger.LogInformation("Parcel or hop not found");
                return NotFound();
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Reporting parcel hop error");
                var error = new Error { ErrorMessage = ex.Message };
                return BadRequest(error);
            }
        }
    }
}