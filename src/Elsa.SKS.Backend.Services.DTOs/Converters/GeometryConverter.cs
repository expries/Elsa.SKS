using System.IO;
using AutoMapper;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Elsa.SKS.Backend.Services.DTOs.Converters
{
    public class GeometryConverter : IValueConverter<string, Geometry>, IValueConverter<Geometry, string>
    {
        public Geometry Convert(string geoJson, ResolutionContext context)
        {
            var jObject = JObject.Parse(geoJson);
            string geometryToken = jObject["geometry"].ToString();

            var geoSerializer = GeoJsonSerializer.Create();
            using var stringReader = new StringReader(geometryToken);
            using var jsonReader = new JsonTextReader(stringReader);
            var geometry = geoSerializer.Deserialize<Geometry>(jsonReader);

            // reverse polygon coordinates if not counter-clock-wise
            if (geometry is Polygon polygon && !polygon.Shell.IsCCW)
            {
                geometry = polygon.Reverse();
            }

            // coordinates of polygons inside multipolygon if not counter-clock-wise
            if (geometry is MultiPolygon multiPolygon)
            {
                for (int i = 0; i < multiPolygon.Geometries.Length; i++)
                {
                    if (multiPolygon.Geometries[i] is Polygon polygonGeometry && !polygonGeometry.Shell.IsCCW)
                    {
                        multiPolygon.Geometries[i] = polygonGeometry.Reverse();
                    }
                }
            }

            return geometry;
        }

        public string Convert(Geometry geometry, ResolutionContext context)
        {
            var geoSerializer = GeoJsonSerializer.Create();
            using var stringWriter = new StringWriter();
            using var jsonWriter = new JsonTextWriter(stringWriter);
            geoSerializer.Serialize(jsonWriter, geometry);
            string json = stringWriter.ToString();
            return json;
        }
    }
}