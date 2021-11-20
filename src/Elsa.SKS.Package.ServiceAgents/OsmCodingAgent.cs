using System;
using System.Net;
using System.Net.Http;
using Elsa.SKS.Package.ServiceAgents.Entities;
using Elsa.SKS.Package.ServiceAgents.Interfaces;
using Nominatim.API.Geocoders;
using Nominatim.API.Models;

namespace Elsa.SKS.Package.ServiceAgents
{
    public class OsmCodingAgent : IGeocodingAgent
    {
        public Geolocation GeocodeAddress(Address address)
        {
            var encoder = new ForwardGeocoder();

            var request = encoder.Geocode(new ForwardGeocodeRequest
            {
                StreetAddress = address.Street,
                City = address.City,
                ShowGeoJSON = true,
                LimitResults = 1
            });
            request.Wait();

            if (request.Result.Length > 0)
            {
                var geolocationData = new Geolocation
                {
                    Latitude = request.Result[0].Latitude,
                    Longitude = request.Result[0].Longitude
                };
                return geolocationData;
            }
            else
            {
                // logging
                // exception
                
                //TODO: change to exception
                return null;
            }

        }
    }
}