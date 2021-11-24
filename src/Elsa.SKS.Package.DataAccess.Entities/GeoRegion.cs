using System.ComponentModel.DataAnnotations;
using GeoJSON.Net;

namespace Elsa.SKS.Package.DataAccess.Entities
{
    public class GeoRegion : GeoJSONObject
    {
        [Key]
        public int? Id { get; set; }
        
        public override GeoJSONObjectType Type { get; }
    }
}