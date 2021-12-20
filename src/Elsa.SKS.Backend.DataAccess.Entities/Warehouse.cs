using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Backend.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class Warehouse : Hop
    {
        /// <summary>
        /// Gets or Sets Level
        /// </summary>
        public int? Level { get; set; }

        /// <summary>
        /// Next hops after this warehouse (warehouses or trucks).
        /// </summary>
        /// <value>Next hops after this warehouse (warehouses or trucks).</value>
        public virtual List<WarehouseNextHop> NextHops { get; set; }
    }
}