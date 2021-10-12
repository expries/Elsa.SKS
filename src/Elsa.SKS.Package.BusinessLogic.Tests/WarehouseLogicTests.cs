using System.Collections.Generic;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using FizzWare.NBuilder;
using FluentAssertions;
using Xunit;

namespace Elsa.SKS.Package.BusinessLogic.Tests
{
    public class WarehouseLogicTests
    {
        [Fact]
        public void GivenHierarchyIsLoadedAndRootWarehouseIsNotNull_WhenExportingWarehouses_ThenReturnRootWarehouse()
        {
            var warehouseLogic = new WarehouseLogic();
            
            var warehouseReturned = warehouseLogic.ExportWarehouses();
            
            warehouseReturned.Should().BeOfType<Warehouse>();
        }
        
        [Fact]
        public void GivenHierarchyIsNotLoaded_WhenExportingWarehouses_ThenThrowWarehouseHierarchyNotLoadedException()
        {
            var warehouseLogic = WarehouseLogic.CreateWithoutLoadedHierarchy();
            
            Assert.Throws<WarehouseHierarchyNotLoadedException>(() => warehouseLogic.ExportWarehouses());
        }
        
        [Fact]
        public void GivenRootWareHouseIsNull_WhenExportingWarehouses_ThenThrowInvalidWarehouseException()
        {
            var warehouseLogic = WarehouseLogic.CreateWithFaultyWarehouse();
            
            Assert.Throws<InvalidWarehouseException>(() => warehouseLogic.ExportWarehouses());
        }
        
        [Fact]
        public void GivenExistentWarehouseCode_WhenGettingWarehouse_ThenReturnWarehouse()
        {
            var warehouseLogic = new WarehouseLogic();
            const string code = TestConstants.ExistentWareHouseCode;

            var warehouseReturned = warehouseLogic.GetWarehouse(code);
            
            warehouseReturned.Should().BeOfType<Warehouse>();
        }
        
        [Fact]
        public void GivenNonExistentWarehouseCode_WhenGettingWarehouse_ThenThrowWarehouseNotFoundException()
        {
            var warehouseLogic = new WarehouseLogic();
            const string code = TestConstants.NonExistentWarehouseCode;

            Assert.Throws<WarehouseNotFoundException>(() => warehouseLogic.GetWarehouse(code));
        }
        
        [Fact]
        public void GivenFaultyWarehouseCode_WhenGettingWarehouse_ThenThrowInvalidWarehouseException()
        {
            var warehouseLogic = new WarehouseLogic();
            const string code = TestConstants.FaultyWarehouseCode;

            Assert.Throws<InvalidWarehouseException>(() => warehouseLogic.GetWarehouse(code));
        }
        
        [Fact]
        public void GivenTheValidationIsNotValid_WhenImportingWarehouses_ThenThrowInvalidWarehouseException()
        {
            var warehouseLogic = new WarehouseLogic();
            var warehouse = Builder<Warehouse>.CreateNew()
                .With(x => x.Description = TestConstants.FaultyWarehouseDescription)
                .With(x => x.NextHops = null)
                .With(x => x.Code = TestConstants.ExistentWareHouseCode)
                .Build();

            Assert.Throws<InvalidWarehouseException>(() => warehouseLogic.ImportWarehouses(warehouse));
        }
        
        [Fact]
        public void GivenWarehouseCodeIsNullOrEmpty_WhenImportingWarehouses_ThenThrowInvalidWarehouseException()
        {
            var warehouseLogic = new WarehouseLogic();
            var warehouse = Builder<Warehouse>.CreateNew()
                .With(x => x.Code = null)
                .With(x => x.NextHops = new List<WarehouseNextHops>())
                .Build();

            Assert.Throws<InvalidWarehouseException>(() => warehouseLogic.ImportWarehouses(warehouse));
        }
        
    }
}