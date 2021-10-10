using System.Collections.Generic;
using AutoMapper;
using Elsa.SKS.Controllers;
using Elsa.SKS.MappingProfiles;
using Elsa.SKS.Package.BusinessLogic;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.Services.DTOs;
using FakeItEasy;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elsa.SKS.Package.Services.Tests
{
    public class WarehouseManagementApiTests
    {
        private WarehouseManagementApiController _controller;
        private Mapper _mapper;

        [Fact]
        public void GivenWarehousesExist_WhenExportWarehouses_ThenReturn200()
        {
            var warehouseLogic = A.Fake<IWarehouseLogic>();
            
            A.CallTo(() => warehouseLogic.ExportWarehouses())
                .Returns(new Elsa.SKS.Package.BusinessLogic.Entities.Warehouse());

            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<WarehouseProfile>()));
            var controller = new WarehouseManagementApiController(warehouseLogic, mapper);
            
            var actionResult = controller.ExportWarehouses();
            actionResult.Should().BeOfType<OkObjectResult>();
        }
        
        [Fact]
        public void GivenAWarehouseHierarchyNotLoadedExceptionIsThrown_WhenExportingWarehouses_ThenReturn404()
        {
            var warehouseLogic = A.Fake<IWarehouseLogic>();
            
            A.CallTo(() => warehouseLogic.ExportWarehouses())
                .Throws<WarehouseHierarchyNotLoadedException>();

            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<WarehouseProfile>()));
            var controller = new WarehouseManagementApiController(warehouseLogic, mapper);

            var actionResult = controller.ExportWarehouses();
            actionResult.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public void GivenABusinessExceptionIsThrown_WhenExportingWarehouses_ThenReturn400()
        {
            var warehouseLogic = A.Fake<IWarehouseLogic>();
            
            A.CallTo(() => warehouseLogic.ExportWarehouses())
                .Throws<BusinessException>();

            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<WarehouseProfile>()));
            var controller = new WarehouseManagementApiController(warehouseLogic, mapper);
            
            var actionResult = controller.ExportWarehouses();
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }
        
        [Fact]
        public void GivenWarehousesExist_WhenAWarehouseIsRequested_ThenReturn200()
        {
            var warehouseLogic = A.Fake<IWarehouseLogic>();

            A.CallTo(() => warehouseLogic.GetWarehouse(A<string>._))
                .Returns(new BusinessLogic.Entities.Warehouse());
            
            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<WarehouseProfile>()));
            var controller = new WarehouseManagementApiController(warehouseLogic, mapper);
            
            const string warehouseCode = TestConstants.ExistentWareHouseCode;
            var actionResult = controller.GetWarehouse(warehouseCode);
            actionResult.Should().BeOfType<OkObjectResult>();
        }
        
        [Fact]
        public void GivenAWarehouseNotFoundExceptionIsThrown_WhenAWarehouseIsRequested_ThenReturn404()
        {
            var warehouseLogic = A.Fake<IWarehouseLogic>();

            A.CallTo(() => warehouseLogic.GetWarehouse(A<string>._))
                .Throws<WarehouseNotFoundException>();
            
            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<WarehouseProfile>()));
            var controller = new WarehouseManagementApiController(warehouseLogic, mapper);
            const string warehouseCode = TestConstants.NonExistentWarehouseCode;
            
            var actionResult = controller.GetWarehouse(warehouseCode);
            actionResult.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public void GivenAInvalidWarehouseExceptionIsThrown_WhenAWarehouseIsRequested_ThenReturn400()
        {
            var warehouseLogic = A.Fake<IWarehouseLogic>();

            A.CallTo(() => warehouseLogic.GetWarehouse(A<string>._))
                .Throws<BusinessException>();
           
            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<WarehouseProfile>()));
            var controller = new WarehouseManagementApiController(warehouseLogic, mapper);
            const string warehouseCode = TestConstants.FaultyWarehouseCode;
            
            var actionResult = controller.GetWarehouse(warehouseCode);
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }
        
        [Fact]
        public void GivenWarehousesIsValid_WhenAWarehouseIsImported_ThenReturn200()
        {
            var warehouseLogic = A.Fake<IWarehouseLogic>();

            A.CallTo(() => warehouseLogic.ImportWarehouses(A<BusinessLogic.Entities.Warehouse>._))
                .DoesNothing();
            
            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<WarehouseProfile>()));
            var controller = new WarehouseManagementApiController(warehouseLogic, mapper);
            var warehouse = Builder<Warehouse>.CreateNew().Build();
            
            var actionResult = controller.ImportWarehouses(warehouse);
            actionResult.Should().BeOfType<OkResult>();
        }
        
        [Fact]
        public void GivenABusinessExceptionIsThrown_WhenAWarehouseIsImported_ThenReturn400()
        {
            var warehouseLogic = A.Fake<IWarehouseLogic>();

            A.CallTo(() => warehouseLogic.ImportWarehouses(A<BusinessLogic.Entities.Warehouse>._))
                .Throws<BusinessException>();

            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<WarehouseProfile>()));
            var controller = new WarehouseManagementApiController(warehouseLogic, mapper);
            var warehouse = Builder<Warehouse>.CreateNew().Build();
            
            var actionResult = controller.ImportWarehouses(warehouse);
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }
        
        [Fact]
        public void AutomapperConfigurationTester()
        {
            var configuration = new MapperConfiguration(cfg =>
                cfg.AddProfile(new WarehouseProfile()));

            configuration.AssertConfigurationIsValid();
        }
    }
}