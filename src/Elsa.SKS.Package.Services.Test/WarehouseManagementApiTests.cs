using System.Globalization;
using Elsa.SKS.Controllers;
using Elsa.SKS.Package.Services.DTOs;
using Elsa.SKS.Package.Services.DTOs.Enums;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elsa.SKS.Package.Services.Test
{
    public class WarehouseManagementApiTests
    {
        private WarehouseManagementApiController _controller;
        private Warehouse _warehouse = TestConstants.ExistingWarehouses;

        public WarehouseManagementApiTests()
        {
            _controller = new WarehouseManagementApiController(_warehouse);
        }
        
        [Fact]
        public void GivenWarehousesExist_WhenExportWarehouses_ThenReturn200()
        {
            var actionResult = _controller.ExportWarehouses();
            Assert.IsType<OkObjectResult>(actionResult);
        }
        
        [Fact]
        public void GivenHierarchyNotLoaded_WhenExportWarehouses_ThenReturn404()
        {
            var controller = new WarehouseManagementApiController();
            var actionResult = controller.ExportWarehouses();
            Assert.IsType<NotFoundResult>(actionResult);
        }
        
        [Fact]
        public void GivenHierarchyIsNull_WhenExportWarehouses_ThenReturn400()
        {
            var controller = new WarehouseManagementApiController(null);
            var actionResult = controller.ExportWarehouses();
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }
        
        [Fact]
        public void GivenWarehousesExist_WhenWarehouseIsRequested_ThenReturn200()
        {
            const string warehouseCode = TestConstants.ExistentWareHouseCode;
            var actionResult = _controller.GetWarehouse(warehouseCode);
            Assert.IsType<OkObjectResult>(actionResult);
        }
        
        [Fact]
        public void GivenWarehousesDoesNotExist_WhenWarehouseIsRequested_ThenReturn404()
        {
            const string warehouseCode = TestConstants.NonExistentWarehouseCode;
            var actionResult = _controller.GetWarehouse(warehouseCode);
            Assert.IsType<NotFoundResult>(actionResult);
        }
        
        [Fact]
        public void GivenWarehousesIsFaulty_WhenWarehouseIsRequested_ThenReturn400()
        {
            const string warehouseCode = TestConstants.FaultyWarehouseCode;
            var actionResult = _controller.GetWarehouse(warehouseCode);
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }
        
        [Fact]
        public void GivenWarehousesIsValid_WhenWarehouseIsImported_ThenReturn200()
        {
            var warehouse = new Warehouse();
            var actionResult = _controller.ImportWarehouses(warehouse);
            Assert.IsType<OkResult>(actionResult);
        }
        
        [Fact]
        public void GivenWarehousesIsNotValid_WhenWarehouseIsImported_ThenReturn400()
        {
            var warehouse = new Warehouse { HopType = HopType.Truck };
            var actionResult = _controller.ImportWarehouses(warehouse);
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }
    }
}