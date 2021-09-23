using System;
using Elsa.SKS.Package.Services.DTOs;
using Newtonsoft.Json.Linq;

namespace Elsa.SKS.Converters
{
    public class HopConverter : JsonCreationConverter<Hop>
    {
        protected override Hop Create(Type objectType, JObject jObject)
        {
            throw new NotImplementedException();
        }
    }
}