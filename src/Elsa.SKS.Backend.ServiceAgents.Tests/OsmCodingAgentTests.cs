using System;
using Elsa.SKS.Backend.ServiceAgents.Entities;
using Elsa.SKS.Backend.ServiceAgents.Exceptions;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using Xunit;

namespace Elsa.SKS.Backend.ServiceAgents.Tests
{
    public class OsmCodingAgentTests
    {

        private readonly ILogger<OsmCodingAgent> _logger;

        public OsmCodingAgentTests()
        {
            _logger = A.Fake<ILogger<OsmCodingAgent>>();
        }

        [Fact]
        public void GivenValidAddressInVienna_WhenEncoding_ThenReturnCorrectCoordinates()
        {
            var encoder = new OsmCodingAgent(_logger);

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

            var geolocationActualResult = encoder.GeocodeAddress(address);

            geolocationCorrectResult.Should().BeEquivalentTo(geolocationActualResult);
        }

        [Fact]
        public void GivenNotValidAddressInVienna_WhenEncoding_ThenReturnNotFoundException()
        {
            var encoder = new OsmCodingAgent(_logger);

            var address = new Address
            {
                Street = "Notvalid 1",
                City = "Wien"
            };

            Action geocodeAddress = () => encoder.GeocodeAddress(address);

            geocodeAddress.Should().Throw<AddressNotFoundException>();
        }


    }
}