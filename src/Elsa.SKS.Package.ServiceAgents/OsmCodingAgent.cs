using System;
using System.Linq;
using Elsa.SKS.Package.ServiceAgents.Entities;
using Elsa.SKS.Package.ServiceAgents.Exceptions;
using Elsa.SKS.Package.ServiceAgents.Interfaces;
using Microsoft.Extensions.Logging;
using Nominatim.API.Geocoders;
using Nominatim.API.Models;

namespace Elsa.SKS.Package.ServiceAgents
{
    public class OsmCodingAgent : IGeocodingAgent
    {
        private readonly ILogger<OsmCodingAgent> _logger;

        public OsmCodingAgent(ILogger<OsmCodingAgent> logger)
        {
            _logger = logger;
        }

        public Geolocation GeocodeAddress(Address address)
        {
            try
            {
                var encoder = new ForwardGeocoder();
                var request = encoder.Geocode(new ForwardGeocodeRequest
                {
                    StreetAddress = address.Street,
                    PostalCode = address.PostalCode,
                    City = address.City,
                    Country = address.Country,
                    ShowGeoJSON = true
                });
                request.Wait();

                if (request.Result.Length > 0)
                {
                    var results = request.Result.OrderByDescending(x => x.Importance).ToList();
                    var geolocationData = new Geolocation
                    {
                        Latitude = results[0].Latitude,
                        Longitude = results[0].Longitude
                    };
                    return geolocationData;
                }

                _logger.LogInformation("Given address was not found");
                throw new AddressNotFoundException("Given address was not found.");
            }
            catch (Exception ex) when (ex is not ServiceAgentException)
            {
                _logger.LogError(ex, "Request Error");
                throw new ServiceAgentException("Error in requesting geolocation data occured.", ex);
            }

        }
    }
}