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
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Elsa.SKS.Attributes;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.Services.DTOs;
using Elsa.SKS.Package.Webhooks.Interfaces;
using Microsoft.Extensions.Logging;

namespace Elsa.SKS.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class ParcelWebhookApiController : ControllerBase
    { 
        private readonly IWebhookLogic _webhookLogic;
        
        private readonly IMapper _mapper;

        private readonly ILogger<ParcelWebhookApiController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webhookLogic"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public ParcelWebhookApiController(IWebhookLogic webhookLogic, IMapper mapper, ILogger<ParcelWebhookApiController> logger)
        {
            _webhookLogic = webhookLogic;
            _mapper = mapper;
            _logger = logger;
        }
        
        /// <summary>
        /// Get all registered subscriptions for the parcel webhook.
        /// </summary>
        /// <param name="trackingId"></param>
        /// <response code="200">List of webooks for the &#x60;trackingId&#x60;</response>
        /// <response code="404">No parcel found with that tracking ID.</response>
        [HttpGet]
        [Route("/parcel/{trackingId}/webhooks")]
        [ValidateModelState]
        [SwaggerOperation("ListParcelWebhooks")]
        [SwaggerResponse(statusCode: 200, type: typeof(WebhookResponses), description: "List of webooks for the &#x60;trackingId&#x60;")]
        public virtual IActionResult ListParcelWebhooks([FromRoute][Required][RegularExpression("^[A-Z0-9]{9}$")]string trackingId)
        {
            try
            {
                var allWebhooks = _webhookLogic.GetParcelWebhooks(trackingId);
                var response = _mapper.Map<WebhookResponses>(allWebhooks);
                return Ok(response);
            }
            catch (ParcelNotFoundException ex)
            {
                _logger.LogInformation("Parcel not found");
                return NotFound();
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Subscribe parcel error");
                var error = new Error { ErrorMessage = ex.Message };
                return BadRequest(error);
            }
        }

        /// <summary>
        /// Subscribe to a webhook notification for the specific parcel.
        /// </summary>
        /// <param name="trackingId"></param>
        /// <param name="url"></param>
        /// <response code="200">Successful response</response>
        /// <response code="404">No parcel found with that tracking ID.</response>
        [HttpPost]
        [Route("/parcel/{trackingId}/webhooks")]
        [ValidateModelState]
        [SwaggerOperation("SubscribeParcelWebhook")]
        [SwaggerResponse(statusCode: 200, type: typeof(WebhookResponse), description: "Successful response")]
        public virtual IActionResult SubscribeParcelWebhook([FromRoute][Required][RegularExpression("^[A-Z0-9]{9}$")]string trackingId, [FromQuery][Required()]string url)
        {
            try
            {
                var subscription = _webhookLogic.SubscribeParcelWebhook(trackingId, url);
                var webhookResponse = _mapper.Map<WebhookResponse>(subscription);
                _logger.LogInformation("Subscribe ParcelWebhook response: Created");
                return Created("/" + subscription.TrackingId, webhookResponse);
            }
            catch (ParcelNotFoundException ex)
            {
                _logger.LogInformation("Parcel not found");
                return NotFound();
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Subscribe parcel error");
                var error = new Error { ErrorMessage = ex.Message };
                return BadRequest(error);
            }
        }

        /// <summary>
        /// Remove an existing webhook subscription.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Success</response>
        /// <response code="404">Subscription does not exist.</response>
        [HttpDelete]
        [Route("/parcel/webhooks/{id}")]
        [ValidateModelState]
        [SwaggerOperation("UnsubscribeParcelWebhook")]
        public virtual IActionResult UnsubscribeParcelWebhook([FromRoute][Required]long? id)
        {
            try
            {
                _webhookLogic.UnsubscribeParcelWebhook(id);
                _logger.LogInformation("Unsubscribe ParcelWebhook response: Successfully deleted");
                return Ok();
            }
            catch (SubscriptionNotFoundException ex)
            {
                _logger.LogInformation("Subscription not found");
                return NotFound();
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Unsubscribe parcel error");
                var error = new Error { ErrorMessage = ex.Message };
                return BadRequest(error);
            }
        }
    }
}
