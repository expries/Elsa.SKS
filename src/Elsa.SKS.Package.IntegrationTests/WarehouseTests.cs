using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Elsa.SKS.Package.IntegrationTests.Data;
using Elsa.SKS.Package.IntegrationTests.Extensions;
using Elsa.SKS.Package.Services.DTOs;
using FluentAssertions;
using Xunit;

namespace Elsa.SKS.Package.IntegrationTests
{
    [Trait("TestCategory", "IntegrationTests")]
    public class WarehouseTests
    {
        private readonly HttpClient _client;

        public WarehouseTests()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000")
            };
        }

        [Fact]
        public async Task ImportAndExportWarehouse()
        {
            // Import warehouse
            var warehouse = WarehouseData.RootWarehouse;
            var jsonContent = warehouse.ToJsonContent();
            
            var importResponse = await _client.PostAsync("/warehouse", jsonContent);

            importResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            
            // Get warehouse by code
            var getWarehouseResponse = await _client.GetAsync($"/warehouse/{warehouse.Code}");
            var warehouseFromGet = await getWarehouseResponse.Content.ToJsonAsync<Warehouse>();
            
            getWarehouseResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            warehouseFromGet.Level.Should().Be(warehouse.Level);
            warehouseFromGet.Code.Should().Be(warehouse.Code);
            warehouseFromGet.Description.Should().Be(warehouse.Description);
            warehouseFromGet.LocationName.Should().Be(warehouse.LocationName);
            warehouseFromGet.NextHops.Count.Should().Be(warehouse.NextHops.Count);
            
            // Export warehouse
            var exportResponse = await _client.GetAsync("/warehouse");
            var exportedWarehouse = await exportResponse.Content.ToJsonAsync<Warehouse>();
            
            exportResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            exportedWarehouse.Level.Should().Be(warehouse.Level);
            exportedWarehouse.Code.Should().Be(warehouse.Code);
            exportedWarehouse.Description.Should().Be(warehouse.Description);
            exportedWarehouse.LocationName.Should().Be(warehouse.LocationName);
            exportedWarehouse.NextHops.Count.Should().Be(warehouse.NextHops.Count);
        }
    }
}

