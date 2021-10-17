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
using Swashbuckle.AspNetCore.Annotations;

namespace Elsa.SKS.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class LogisticsPartnerApiController : ControllerBase
    {
        private readonly IParcelRegistration _parcelRegistration;

        private readonly IMapper _mapper;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parcelRegistration"></param>
        /// <param name="mapper"></param>
        public LogisticsPartnerApiController(IParcelRegistration parcelRegistration, IMapper mapper)
        {
            _parcelRegistration = parcelRegistration;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Transfer an existing parcel into the system from the service of a logistics partner. 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="trackingId">The tracking ID of the parcel. E.g. PYJRB4HZ6 </param>
        /// <response code="200">Successfully transitioned the parcel</response>
        /// <response code="400">The operation failed due to an error.</response>
        [HttpPost]
        [Route("/parcel/{trackingId}")]
        [ValidateModelState]
        [SwaggerOperation("TransitionParcel")]
        [SwaggerResponse(statusCode: 200, type: typeof(NewParcelInfo), description: "Successfully transitioned the parcel")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "The operation failed due to an error.")]
        public virtual IActionResult TransitionParcel(
            [FromBody] Parcel body, 
            [FromRoute][Required][RegularExpression("^[A-Z0-9]{9}$")] string trackingId)
        {
            try
            {
                var entity = _mapper.Map<Elsa.SKS.Package.BusinessLogic.Entities.Parcel>(body);
                var parcelEntity = _parcelRegistration.TransitionParcel(entity, trackingId);
                var result = _mapper.Map<TrackingInformation>(parcelEntity);
                return Ok(result);
            }
            catch (BusinessException ex)
            {
                var error = new Error { ErrorMessage = ex.Message };
                return BadRequest(error);
            }
        }
    }
}
