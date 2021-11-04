using System.Collections.Generic;
using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.DataAccess.Interfaces;
using FakeItEasy;
using FizzWare.NBuilder;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace Elsa.SKS.Package.BusinessLogic.Tests
{
    public class WarehouseLogicTests
    {
        private readonly IWarehouseLogic _logic;

        private readonly IHopRepository _hopRepository;

        private readonly IValidator<Warehouse> _warehouseValidator;

        private readonly IMapper _mapper;
        
        public WarehouseLogicTests()
        {
            _hopRepository = A.Fake<IHopRepository>();
            _warehouseValidator = A.Fake<IValidator<Warehouse>>();
            _mapper = A.Fake<IMapper>();
            _logic = new WarehouseLogic(_hopRepository, _warehouseValidator, _mapper);
        }
        
        [Fact]
        public void GivenHierarchyIsLoadedAndRootWarehouseIsNotNull_WhenExportingWarehouses_ThenReturnRootWarehouse()
        {
            var warehouseReturned = _logic.ExportWarehouses();
            
            warehouseReturned.Should().BeOfType<Warehouse>();
        }
        
        [Fact]
        public void GivenHierarchyIsNotLoaded_WhenExportingWarehouses_ThenThrowWarehouseHierarchyNotLoadedException()
        {
            Assert.Throws<WarehouseHierarchyNotLoadedException>(() => _logic.ExportWarehouses());
        }
        
        [Fact]
        public void GivenRootWareHouseIsNull_WhenExportingWarehouses_ThenThrowInvalidWarehouseException()
        {
            Assert.Throws<InvalidWarehouseException>(() => _logic.ExportWarehouses());
        }
        
        [Fact]
        public void GivenExistentWarehouseCode_WhenGettingWarehouse_ThenReturnWarehouse()
        {
            const string code = TestConstants.ExistentWareHouseCode;

            var warehouseReturned = _logic.GetWarehouse(code);
            
            warehouseReturned.Should().BeOfType<Warehouse>();
        }
        
        [Fact]
        public void GivenNonExistentWarehouseCode_WhenGettingWarehouse_ThenThrowWarehouseNotFoundException()
        {
            const string code = TestConstants.NonExistentWarehouseCode;

            Assert.Throws<WarehouseNotFoundException>(() => _logic.GetWarehouse(code));
        }
        
        [Fact]
        public void GivenFaultyWarehouseCode_WhenGettingWarehouse_ThenThrowInvalidWarehouseException()
        {
            const string code = TestConstants.FaultyWarehouseCode;

            Assert.Throws<InvalidWarehouseException>(() => _logic.GetWarehouse(code));
        }
        
        [Fact]
        public void GivenNonValidWarehouseValidation_WhenImportingWarehouses_ThenThrowInvalidWarehouseException()
        {
            var warehouse = Builder<Warehouse>.CreateNew()
                .With(x => x.Description = TestConstants.FaultyWarehouseDescription)
                .With(x => x.NextHops = null)
                .With(x => x.Code = TestConstants.ExistentWareHouseCode)
                .Build();

            Assert.Throws<InvalidWarehouseException>(() => _logic.ImportWarehouses(warehouse));
        }
        
        [Fact]
        public void GivenWarehouseCodeIsNullOrEmpty_WhenImportingWarehouses_ThenThrowInvalidWarehouseException()
        {
            var warehouse = Builder<Warehouse>.CreateNew()
                .With(x => x.Code = null)
                .With(x => x.NextHops = new List<WarehouseNextHops>())
                .Build();

            Assert.Throws<InvalidWarehouseException>(() => _logic.ImportWarehouses(warehouse));
        }
        
    }
}