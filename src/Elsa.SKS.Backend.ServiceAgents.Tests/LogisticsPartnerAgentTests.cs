using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Elsa.SKS.Backend.BusinessLogic.Entities;
using Elsa.SKS.Backend.ServiceAgents.Entities;
using Elsa.SKS.Backend.ServiceAgents.Exceptions;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using Xunit;

namespace Elsa.SKS.Backend.ServiceAgents.Tests
{
    public class LogisticsPartnerAgentTests
    {
        private readonly HttpClient _client;

        private readonly ILogger<LogisticsPartnerAgent> _logger;

        public LogisticsPartnerAgentTests()
        {
            _client = new HttpClient(new HttpMessageHandlerStub());
            _logger = A.Fake<ILogger<LogisticsPartnerAgent>>();
        }

        [Fact]
        public void GivenADbAccessExceptionIsThrown_WhenTransferringParcel_ThenReturnCorrectCoordinates()
        {
            var logisticsPartnerAgent = new LogisticsPartnerAgent(_client, _logger);

            var transferWarehouse = new TransferWarehouse();
            var parcel = new Parcel();

            Action create = () => logisticsPartnerAgent.TransferParcel(transferWarehouse, parcel);

            create.Should().Throw<ServiceAgentException>();
        }


        // For mocking purpose
        private class HttpMessageHandlerStub : HttpMessageHandler
        {
            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("This is a reply")
                };

                return await Task.FromResult(responseMessage);
            }
        }


    }
}