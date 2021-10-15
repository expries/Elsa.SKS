using System;
using Elsa.SKS.Package.Services.DTOs.Enums;
using Newtonsoft.Json.Linq;

namespace Elsa.SKS.Package.Services.DTOs.Converters
{
    public class HopConverter : JsonCreationConverter<Hop>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="jObject"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        protected override Hop Create(Type objectType, JObject jObject)
        {
            if (jObject is null)
            {
                throw new ArgumentNullException(nameof(jObject));
            }

            if (!jObject.ContainsKey("hopType"))
            {
                throw new ArgumentException("jObject doesn't have a key hopType.");
            }
            
            var type = jObject["hopType"];

            HopType hopType;

            try
            {
                hopType = type.ToObject<HopType>();
            }
            catch (ArgumentException)
            {
                hopType = HopType.None;
            }

            Hop hop = hopType switch
            {
                HopType.Truck => new Truck(),
                HopType.Warehouse => new Warehouse(),
                HopType.TransferWarehouse => new TransferWarehouse(),
                _ => throw new Exception("Invalid operation. Unknown hopType.")
            };

            return hop;

        }
    }
}