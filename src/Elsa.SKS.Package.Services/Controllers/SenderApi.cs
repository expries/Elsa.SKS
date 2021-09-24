/*
 * Parcel Logistics Service
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.20.1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using Elsa.SKS.Attributes;
using Elsa.SKS.Package.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Elsa.SKS.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class SenderApiController : ControllerBase
    { 
        /// <summary>
        /// Submit a new parcel to the logistics service. 
        /// </summary>
        /// <param name="body"></param>
        /// <response code="201">Successfully submitted the new parcel</response>
        /// <response code="400">The operation failed due to an error.</response>
        [HttpPost]
        [Route("/parcel")]
        [ValidateModelState]
        [SwaggerOperation("SubmitParcel")]
        [SwaggerResponse(statusCode: 201, type: typeof(NewParcelInfo), description: "Successfully submitted the new parcel")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "The operation failed due to an error.")]
        public virtual IActionResult SubmitParcel([FromBody]Parcel body)
        {
            if (body.Weight <= 0)
            {
                var error = new Error();
                return BadRequest(error);
            }

            var result = new NewParcelInfo();
            var trackingId = TestConstants.TrackingIdOfSubmittedParcel;
            return Created("/" + trackingId, result);
        }
    }
}
