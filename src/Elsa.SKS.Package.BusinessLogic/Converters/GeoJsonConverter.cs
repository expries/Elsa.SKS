using System.IO;
using AutoMapper;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;

namespace Elsa.SKS.Package.BusinessLogic.Converters
{
    public class GeoJsonConverter : IValueConverter<string, Geometry>, IValueConverter<Geometry, string>
    {
        public Geometry Convert(string geoJson, ResolutionContext context)
        {
            var serializer = GeoJsonSerializer.Create();
            using var stringReader = new StringReader(geoJson);
            using var jsonReader = new JsonTextReader(stringReader);
            var geometry = serializer.Deserialize<Geometry>(jsonReader);
            return geometry;
        }

        public string Convert(Geometry geometry, ResolutionContext context)
        {
            var serializer = GeoJsonSerializer.Create();
            using var stringWriter = new StringWriter();
            using var jsonWriter = new JsonTextWriter(stringWriter);
            serializer.Serialize(jsonWriter, geometry);
            string geoJson = stringWriter.ToString();
            return geoJson;
        }
    }
}