using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Parcel = Elsa.SKS.Package.DataAccess.Entities.Parcel;
using WebhookMessage = Elsa.SKS.Package.DataAccess.Entities.WebhookMessage;

namespace Elsa.SKS.Package.BusinessLogic.MappingProfiles
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class WebhookProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public WebhookProfile()
        {
            CreateMap<Subscription, DataAccess.Entities.Subscription>().ReverseMap();
            CreateMap<WebhookMessage, Parcel>().ReverseMap();
        }
    }
}