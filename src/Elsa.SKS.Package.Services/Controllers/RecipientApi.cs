/*
 * Parcel Logistics Service
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.20.1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Elsa.SKS.Attributes;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Elsa.SKS.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class RecipientApiController : ControllerBase
    {
        private readonly IParcelTrackingLogic _parcelTrackingLogic;
        
        private readonly IMapper _mapper;
        
        private readonly ILogger<RecipientApiController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parcelTrackingLogic"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public RecipientApiController(IParcelTrackingLogic parcelTrackingLogic, IMapper mapper, ILogger<RecipientApiController> logger)
        {
            _parcelTrackingLogic = parcelTrackingLogic;
            _mapper = mapper;
            _logger = logger;
        }
        
        /// <summary>
        /// Find the latest state of a parcel by its tracking ID. 
        /// </summary>
        /// <param name="trackingId">The tracking ID of the parcel. E.g. PYJRB4HZ6 </param>
        /// <response code="200">Parcel exists, here&#x27;s the tracking information.</response>
        /// <response code="400">The operation failed due to an error.</response>
        /// <response code="404">Parcel does not exist with this tracking ID.</response>
        [HttpGet]
        [Route("/parcel/{trackingId}")]
        [ValidateModelState]
        [SwaggerOperation("TrackParcel")]
        [SwaggerResponse(statusCode: 200, type: typeof(TrackingInformation), description: "Parcel exists, here&#x27;s the tracking information.")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "The operation failed due to an error.")]
        public IActionResult TrackParcel(
            [FromRoute][Required][RegularExpression("^[A-Z0-9]{9}$")] string trackingId)
        {
            try
            {
                var parcelEntity = _parcelTrackingLogic.TrackParcel(trackingId);
                var result = _mapper.Map<TrackingInformation>(parcelEntity);
                _logger.LogInformation("Track Parcel response: OK");
                return Ok(result);
            }
            catch (ParcelNotFoundException)
            {
                _logger.LogError("Parcel not found error");
                return NotFound();
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Track parcel error");
                var error = new Error { ErrorMessage = ex.Message };
                return BadRequest(error);
            }
        }
    }
}
