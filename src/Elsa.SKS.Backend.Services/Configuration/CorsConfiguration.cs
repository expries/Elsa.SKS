using System;

namespace Elsa.SKS.Backend.Services.Configuration
{
    /// <summary>
    /// CORS configuration format
    /// </summary>
    public class CorsConfiguration
    {
        /// <summary>
        /// Joined string of origins to allow CORS requests for
        /// </summary>
        public string? AllowedOrigins { get; set; }

        /// <summary>
        /// Array of origins to allow CORS requests for
        /// </summary>
        public string[] AllowedOriginsArray => AllowedOrigins?.Split(";") ?? Array.Empty<string>();
    }
}