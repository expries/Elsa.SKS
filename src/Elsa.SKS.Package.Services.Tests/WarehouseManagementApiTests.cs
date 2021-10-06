using Elsa.SKS.Controllers;
using Elsa.SKS.Package.BusinessLogic;
using Elsa.SKS.Package.Services.DTOs;
using Elsa.SKS.Package.Services.DTOs.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elsa.SKS.Package.Services.Tests
{
    public class WarehouseManagementApiTests
    {
        private WarehouseManagementApiController _controller;

        public WarehouseManagementApiTests()
        {
            var warehouseLogic = new WarehouseLogic();
            _controller = new WarehouseManagementApiController(warehouseLogic);
        }
        
        [Fact]
        public void GivenWarehousesExist_WhenExportWarehouses_ThenReturn200()
        {
            var actionResult = _controller.ExportWarehouses();
            actionResult.Should().BeOfType<OkObjectResult>();
        }
        
        [Fact]
        public void GivenHierarchyNotLoaded_WhenExportWarehouses_ThenReturn404()
        {
            var warehouseLogic = WarehouseLogic.CreateWithoutLoadedHierarchy();
            var controller = new WarehouseManagementApiController(warehouseLogic);
            var actionResult = controller.ExportWarehouses();
            actionResult.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public void GivenHierarchyIsNull_WhenExportWarehouses_ThenReturn400()
        {
            var warehouseLogic = WarehouseLogic.CreateWithFaultyWarehouse();
            var controller = new WarehouseManagementApiController(warehouseLogic);
            var actionResult = controller.ExportWarehouses();
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }
        
        [Fact]
        public void GivenWarehousesExist_WhenWarehouseIsRequested_ThenReturn200()
        {
            const string warehouseCode = TestConstants.ExistentWareHouseCode;
            var actionResult = _controller.GetWarehouse(warehouseCode);
            actionResult.Should().BeOfType<OkObjectResult>();
        }
        
        [Fact]
        public void GivenWarehousesDoesNotExist_WhenWarehouseIsRequested_ThenReturn404()
        {
            const string warehouseCode = TestConstants.NonExistentWarehouseCode;
            var actionResult = _controller.GetWarehouse(warehouseCode);
            actionResult.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public void GivenWarehousesIsFaulty_WhenWarehouseIsRequested_ThenReturn400()
        {
            const string warehouseCode = TestConstants.FaultyWarehouseCode;
            var actionResult = _controller.GetWarehouse(warehouseCode);
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }
        
        [Fact]
        public void GivenWarehousesIsValid_WhenWarehouseIsImported_ThenReturn200()
        {
            var warehouse = new Warehouse { Code = TestConstants.ExistingWarehouses.Code };
            var actionResult = _controller.ImportWarehouses(warehouse);
            actionResult.Should().BeOfType<OkResult>();
        }
        
        [Fact]
        public void GivenWarehousesIsNotValid_WhenWarehouseIsImported_ThenReturn400()
        {
            var warehouse = new Warehouse { Code = null };
            var actionResult = _controller.ImportWarehouses(warehouse);
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}