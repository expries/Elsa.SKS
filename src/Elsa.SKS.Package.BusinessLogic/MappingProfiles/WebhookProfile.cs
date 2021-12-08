using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;

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
            CreateMap<Parcel, DataAccess.Entities.WebhookMessage>().ReverseMap();
        }
    }
}