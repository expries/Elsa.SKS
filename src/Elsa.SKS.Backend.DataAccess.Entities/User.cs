using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Backend.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class User
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int? Id { get; set; }

        /// <summary>
        /// Name of person or company.
        /// </summary>
        /// <value>Name of person or company.</value>
        public string Name { get; set; }

        /// <summary>
        /// Street
        /// </summary>
        /// <value>Street</value>
        public string Street { get; set; }

        /// <summary>
        /// Postalcode
        /// </summary>
        /// <value>Postalcode</value>
        public string PostalCode { get; set; }

        /// <summary>
        /// City
        /// </summary>
        /// <value>City</value>
        public string City { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        /// <value>Country</value>
        public string Country { get; set; }
    }
}