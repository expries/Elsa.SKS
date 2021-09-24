using Elsa.SKS.Controllers;
using Elsa.SKS.Package.Services.DTOs;
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
        
    }
}