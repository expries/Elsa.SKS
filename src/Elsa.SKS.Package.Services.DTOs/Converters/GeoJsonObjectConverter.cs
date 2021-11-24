using System;
using System.Linq;
using GeoJSON.Net;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json.Linq;

namespace Elsa.SKS.Package.Services.DTOs.Converters
{
    public class GeoJsonObjectConverter : JsonCreationConverter<GeoJSONObject>
    {
        protected override GeoJSONObject Create(Type objectType, JObject jObject)
        {
            var geometry = jObject["geometry"].ToObject<GeoJSONObject>();

            GeoJSONObject geoJsonObject = geometry?.Type switch
            {
                GeoJSONObjectType.Polygon => (Polygon) geometry,
                GeoJSONObjectType.MultiPolygon => (MultiPolygon) geometry,
                _ => throw new ArgumentOutOfRangeException($"Geometry type '{geometry?.Type}' is not supported.")
            };

            return geoJsonObject;
        }
    }
}