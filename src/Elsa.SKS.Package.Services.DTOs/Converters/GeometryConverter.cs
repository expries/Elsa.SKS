using System;
using System.IO;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Elsa.SKS.Package.Services.DTOs.Converters
{
    public class GeometryConverter : JsonCreationConverter<Geometry>
    {
        protected override Geometry Create(Type objectType, JObject jObject)
        {
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
                    var multiPolygonGeometry = multiPolygon.Geometries[i];

                    if (multiPolygonGeometry is Polygon polygonGeometry && !polygonGeometry.Shell.IsCCW)
                    {
                        multiPolygon.Geometries[i] = polygonGeometry.Reverse();
                    }
                }
            }

            return geometry;
        }
    }
}