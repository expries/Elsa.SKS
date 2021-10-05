using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Elsa.SKS.Package.BusinessLogic.Entities
{
    public class Warehouse : Hop
    {
        /// <summary>
        /// Gets or Sets Level
        /// </summary>
        [Required]
        public int? Level { get; set; }

        /// <summary>
        /// Next hops after this warehouse (warehouses or trucks).
        /// </summary>
        /// <value>Next hops after this warehouse (warehouses or trucks).</value>
        [Required]
        public List<WarehouseNextHops> NextHops { get; set; }
        
    }
}