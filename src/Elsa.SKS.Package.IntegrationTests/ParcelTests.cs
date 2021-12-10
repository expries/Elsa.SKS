using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Elsa.SKS.Package.IntegrationTests.Data;
using Elsa.SKS.Package.IntegrationTests.Extensions;
using Elsa.SKS.Package.Services.DTOs;
using Elsa.SKS.Package.Services.DTOs.Enums;
using FluentAssertions;
using Xunit;

namespace Elsa.SKS.Package.IntegrationTests
{
    [Trait("Category", "IntegrationTests")]
    public class ParcelTests
    {
        private readonly HttpClient _client;

        public ParcelTests()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000")
            };
        }
        
        [Fact]
        public async Task ParcelJourneyOfSubmittedParcel()
        {
            // Submit parcel
            var parcel = ParcelData.Parcel;
            var content = parcel.ToJsonContent();

            var submitResponse = await _client.PostAsync("/parcel", content);
            var parcelInfo = await submitResponse.Content.ToJsonAsync<NewParcelInfo>();
            
            submitResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            parcelInfo.TrackingId.Should().NotBeNullOrEmpty();

            // Get parcel after submission
            var getResponse = await _client.GetAsync($"/parcel/{parcelInfo.TrackingId}");
            var trackingInformation = await getResponse.Content.ToJsonAsync<TrackingInformation>();
            
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            trackingInformation.State.Should().Be(ParcelState.Pickup);
            trackingInformation.FutureHops.Count.Should().BeGreaterThan(0);
            trackingInformation.VisitedHops.Count.Should().Be(0);
            
            // Report first hop
            var hop = trackingInformation.FutureHops[0];

            var reportHopResponse = await _client.PostAsync($"/parcel/{parcelInfo.TrackingId}/reportHop/{hop.Code}", null);

            reportHopResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            
            // Get parcel after first hop
            var getAfterFirstHop = await _client.GetAsync($"/parcel/{parcelInfo.TrackingId}");
            var trackingInformationPastHop = await getAfterFirstHop.Content.ToJsonAsync<TrackingInformation>();

            getAfterFirstHop.StatusCode.Should().Be(HttpStatusCode.OK);
            trackingInformationPastHop.VisitedHops.Count.Should().BeGreaterThan(0);
            trackingInformationPastHop.State.Should().BeOneOf(ParcelState.InTruckDelivery, ParcelState.InTransport);
            
            // Report parcel delivery
            var reportDeliveryResponse = await _client.PostAsync($"/parcel/{parcelInfo.TrackingId}/reportDelivery", null);

            reportDeliveryResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            
            // Get parcel after delivery
            var getAfterDeliveryResponse = await _client.GetAsync($"/parcel/{parcelInfo.TrackingId}");
            var trackingInformationPastDelivery = await getAfterDeliveryResponse.Content.ToJsonAsync<TrackingInformation>();

            getAfterDeliveryResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            trackingInformationPastDelivery.State.Should().Be(ParcelState.Delivered);
            trackingInformationPastDelivery.FutureHops.Count.Should().Be(0);
            trackingInformationPastDelivery.VisitedHops.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task ParcelJourneyOfTransitionedParcel()
        {
            // Transition parcel
            string trackingId = GenerateTrackingId();
            var parcel = ParcelData.Parcel;
            
            var content = parcel.ToJsonContent();

            var transitionParcel = await _client.PostAsync($"/parcel/{trackingId}", content);
            var parcelInfo = await transitionParcel.Content.ToJsonAsync<TrackingInformation>();
            
            transitionParcel.StatusCode.Should().Be(HttpStatusCode.OK);
            parcelInfo.State.Should().Be(ParcelState.Pickup);
            parcelInfo.FutureHops.Count.Should().BeGreaterThan(0);
            parcelInfo.VisitedHops.Count.Should().Be(0);

            // Get parcel after submission
            var getResponse = await _client.GetAsync($"/parcel/{trackingId}");
            var trackingInformation = await getResponse.Content.ToJsonAsync<TrackingInformation>();
            
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            trackingInformation.State.Should().Be(ParcelState.Pickup);
            trackingInformation.FutureHops.Count.Should().BeGreaterThan(0);
            trackingInformation.VisitedHops.Count.Should().Be(0);
            
            // Report first hop
            var hop = trackingInformation.FutureHops[0];

            var reportHopResponse = await _client.PostAsync($"/parcel/{trackingId}/reportHop/{hop.Code}", null);

            reportHopResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            
            // Get parcel after first hop
            var getAfterFirstHop = await _client.GetAsync($"/parcel/{trackingId}");
            var trackingInformationPastHop = await getAfterFirstHop.Content.ToJsonAsync<TrackingInformation>();

            getAfterFirstHop.StatusCode.Should().Be(HttpStatusCode.OK);
            trackingInformationPastHop.VisitedHops.Count.Should().BeGreaterThan(0);
            trackingInformationPastHop.State.Should().BeOneOf(ParcelState.InTruckDelivery, ParcelState.InTransport);
            
            // Report parcel delivery
            var reportDeliveryResponse = await _client.PostAsync($"/parcel/{trackingId}/reportDelivery", null);

            reportDeliveryResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            
            // Get parcel after delivery
            var getAfterDeliveryResponse = await _client.GetAsync($"/parcel/{trackingId}");
            var trackingInformationPastDelivery = await getAfterDeliveryResponse.Content.ToJsonAsync<TrackingInformation>();

            getAfterDeliveryResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            trackingInformationPastDelivery.State.Should().Be(ParcelState.Delivered);
            trackingInformationPastDelivery.FutureHops.Count.Should().Be(0);
            trackingInformationPastDelivery.VisitedHops.Count.Should().BeGreaterThan(0);
        }
        
        private static string GenerateTrackingId()
        {
            const int stringLength = 9;
            char[] allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
                
            var randomId = new StringBuilder();
            var random = new Random();

            for (int i = 0; i < stringLength; i++)
            {
                int randomCharSelected = random.Next(0, (allowedChars.Length - 1));
                randomId.Append(allowedChars[randomCharSelected]);
            }

            return randomId.ToString();
        }
    }
}