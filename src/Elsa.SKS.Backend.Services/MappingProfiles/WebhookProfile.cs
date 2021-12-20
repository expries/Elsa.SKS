using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Elsa.SKS.Backend.BusinessLogic.Entities;
using Elsa.SKS.Backend.Services.DTOs;

namespace Elsa.SKS.MappingProfiles
{
    /// <summary>
    /// 
    /// </summary>
    public class WebhookProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        [ExcludeFromCodeCoverage]
        public WebhookProfile()
        {
            CreateMap<WebhookResponse, Subscription>().ReverseMap();
            CreateMap<WebhookResponses, WebhookResponse>().ReverseMap();
        }
    }
}