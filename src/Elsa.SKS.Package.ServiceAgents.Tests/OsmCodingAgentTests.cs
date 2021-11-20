using System;
using Elsa.SKS.Package.ServiceAgents.Entities;
using FluentAssertions;
using Xunit;

namespace Elsa.SKS.Package.ServiceAgents.Tests
{
    public class Tests
    {
        [Fact]
        public void GivenValidAddressInVienna_WhenEncoding_ThenReturnCorrectCoordinates()
        {
            var address = new Address
            {
                Street = "Stephansplatz 1",
                City = "Wien"
            };
            
            var geolocationCorrectResult = new Geolocation
            {
                Latitude = 48.2081643,
                Longitude = 16.3734772
            };
            
            var encoder = new OsmCodingAgent();
            var geolocationActualResult = encoder.GeocodeAddress(address);
            
            geolocationCorrectResult.Should().BeEquivalentTo(geolocationActualResult);
        }
        
        [Fact]
        public void GivenNotValidAddressInVienna_WhenEncoding_ThenReturnNotFoundException()
        {
            var address = new Address
            {
                Street = "Notvalid 1",
                City = "Wien"
            };

            var encoder = new OsmCodingAgent();
            var geolocationActualResult = encoder.GeocodeAddress(address);
            
            geolocationActualResult.Should().BeNull(); //TODO: change to exception
        }
        
        
    }   
}