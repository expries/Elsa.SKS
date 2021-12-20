using AutoMapper;
using Elsa.SKS.Controllers;
using Elsa.SKS.Backend.BusinessLogic.Exceptions;
using Elsa.SKS.Backend.BusinessLogic.Interfaces;
using Elsa.SKS.Backend.Services.DTOs;
using FakeItEasy;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Elsa.SKS.Backend.Services.Tests
{
    public class WarehouseManagementApiTests
    {
        private readonly WarehouseManagementApiController _controller;

        private readonly IWarehouseLogic _warehouseLogic;

        private readonly IMapper _mapper;
        
        private readonly ILogger<WarehouseManagementApiController> _logger;

        public WarehouseManagementApiTests()
        {
            _warehouseLogic = A.Fake<IWarehouseLogic>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<WarehouseManagementApiController>>();
            _controller = new WarehouseManagementApiController(_warehouseLogic, _mapper, _logger);
        }
        
        [Fact]
        public void GivenWarehousesExist_WhenExportWarehouses_ThenReturn200()
        {
            var warehouse = Builder<BusinessLogic.Entities.Warehouse>
                .CreateNew()
                .Build();
            
            A.CallTo(() => _warehouseLogic.ExportWarehouses())
                .Returns(warehouse);
            
            var actionResult = _controller.ExportWarehouses();
            
            actionResult.Should().BeOfType<OkObjectResult>();
        }
        
        [Fact]
        public void GivenAWarehouseHierarchyNotLoadedExceptionIsThrown_WhenExportingWarehouses_ThenReturn404()
        {
            A.CallTo(() => _warehouseLogic.ExportWarehouses())
                .Throws<WarehouseHierarchyNotLoadedException>();

            var actionResult = _controller.ExportWarehouses();
            
            actionResult.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public void GivenABusinessExceptionIsThrown_WhenExportingWarehouses_ThenReturn400()
        {
            A.CallTo(() => _warehouseLogic.ExportWarehouses())
                .Throws<BusinessException>();

            var actionResult = _controller.ExportWarehouses();
            
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }
        
        [Fact]
        public void GivenWarehousesExist_WhenAWarehouseIsRequested_ThenReturn200()
        {
            const string warehouseCode = "warehouse_code";

            var warehouse = Builder<BusinessLogic.Entities.Warehouse>
                .CreateNew()
                .Build();

            A.CallTo(() => _warehouseLogic.GetWarehouse(A<string>._))
                .Returns(warehouse);
            
            var actionResult = _controller.GetWarehouse(warehouseCode);
            
            actionResult.Should().BeOfType<OkObjectResult>();
        }
        
        [Fact]
        public void GivenAWarehouseNotFoundExceptionIsThrown_WhenAWarehouseIsRequested_ThenReturn404()
        {
            const string warehouseCode = "warehouse_code";

            A.CallTo(() => _warehouseLogic.GetWarehouse(A<string>._))
                .Throws<WarehouseNotFoundException>();

            var actionResult = _controller.GetWarehouse(warehouseCode);
            
            actionResult.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public void GivenAInvalidWarehouseExceptionIsThrown_WhenAWarehouseIsRequested_ThenReturn400()
        {
            const string warehouseCode = "warehouse_code";

            A.CallTo(() => _warehouseLogic.GetWarehouse(A<string>._))
                .Throws<BusinessException>();
           
            var actionResult = _controller.GetWarehouse(warehouseCode);
            
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }
        
        [Fact]
        public void GivenWarehousesIsValid_WhenAWarehouseIsImported_ThenReturn200()
        {
            var warehouse = Builder<Warehouse>
                .CreateNew()
                .Build();
            
            A.CallTo(() => _warehouseLogic.ImportWarehouses(A<BusinessLogic.Entities.Warehouse>._))
                .DoesNothing();

            var actionResult = _controller.ImportWarehouses(warehouse);
            
            actionResult.Should().BeOfType<OkResult>();
        }
        
        [Fact]
        public void GivenABusinessExceptionIsThrown_WhenAWarehouseIsImported_ThenReturn400()
        {
            var warehouse = Builder<Warehouse>
                .CreateNew()
                .Build();
            
            A.CallTo(() => _warehouseLogic.ImportWarehouses(A<BusinessLogic.Entities.Warehouse>._))
                .Throws<BusinessException>();

            var actionResult = _controller.ImportWarehouses(warehouse);
            
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}