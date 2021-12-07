using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.Services.DTOs;

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