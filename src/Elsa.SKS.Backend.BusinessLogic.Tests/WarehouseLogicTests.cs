using System;
using System.Collections.Generic;
using AutoMapper;
using Elsa.SKS.Backend.BusinessLogic.Entities;
using Elsa.SKS.Backend.BusinessLogic.Exceptions;
using Elsa.SKS.Backend.BusinessLogic.Interfaces;
using Elsa.SKS.Backend.DataAccess.Interfaces;
using Elsa.SKS.Backend.DataAccess.Sql.Exceptions;
using FakeItEasy;
using FizzWare.NBuilder;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Xunit;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Elsa.SKS.Backend.BusinessLogic.Tests
{
    public class WarehouseLogicTests
    {
        private readonly IWarehouseLogic _logic;

        private readonly IHopRepository _hopRepository;

        private readonly IValidator<Warehouse> _warehouseValidator;

        private readonly IMapper _mapper;
        
        private readonly ILogger<WarehouseLogic> _logger;

        public WarehouseLogicTests()
        {
            _hopRepository = A.Fake<IHopRepository>();
            _warehouseValidator = A.Fake<IValidator<Warehouse>>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<WarehouseLogic>>();
            _logic = new WarehouseLogic(_hopRepository, _warehouseValidator, _mapper, _logger);
        }

        [Fact]
        public void GivenAWarehouseHierarchy_WhenExportingWarehouses_ThenReturnTheRootWarehouse()
        {
            var warehouse = Builder<Warehouse>
                .CreateNew()
                .Build();
            
            var warehouseEntity = Builder<DataAccess.Entities.Warehouse>
                .CreateNew()
                .Build();

            A.CallTo(() => _hopRepository.GetAllWarehouses()).Returns(warehouseEntity);
            A.CallTo(() => _mapper.Map<Warehouse>(A<DataAccess.Entities.Warehouse>._)).Returns(warehouse);
            
            var rootWarehouse = _logic.ExportWarehouses();
            
            rootWarehouse.Should().BeOfType<Warehouse>();
        }
        
        [Fact]
        public void GivenTheWarehouseHierarchyWasNotLoadedYet_WhenExportingWarehouses_ThenThrowAWarehouseHierarchyNotLoadedException()
        {
            A.CallTo(() => _hopRepository.GetAllWarehouses()).Returns(null);

            Action exportWarehouses = () => _logic.ExportWarehouses();
            
            exportWarehouses.Should().Throw<WarehouseHierarchyNotLoadedException>();
        }
        
        [Fact]
        public void GivenASingleOrDefaultExceptionIsThrown_WhenExportingWarehouses_ThenThrowAInvalidWarehouseException()
        {
            A.CallTo(() => _hopRepository.GetAllWarehouses()).Throws<SingleOrDefaultException>();

            Action exportWarehouses = () => _logic.ExportWarehouses();
            
            exportWarehouses.Should().Throw<InvalidWarehouseException>();
        }
        
        [Fact]
        public void GivenADataAccessErrorOccurs_WhenExportingWarehouses_ThenThrowABusinessException()
        {
            A.CallTo(() => _hopRepository.GetAllWarehouses()).Throws<DataAccessException>();

            Action exportWarehouses = () => _logic.ExportWarehouses();
            
            exportWarehouses.Should().Throw<BusinessException>();
        }

        [Fact]
        public void GivenAWarehouseExists_WhenGettingWarehouse_ThenReturnTheWarehouse()
        {
            const string code = "warehouse_code";

            var warehouse = Builder<Warehouse>
                .CreateNew()
                .Build();
            
            var warehouseEntity = Builder<DataAccess.Entities.Warehouse>
                .CreateNew()
                .Build();

            A.CallTo(() => _hopRepository.GetWarehouseByCode(A<string>._)).Returns(warehouseEntity);
            A.CallTo(() => _mapper.Map<Warehouse>(A<DataAccess.Entities.Warehouse>._)).Returns(warehouse);
            
            var result = _logic.GetWarehouse(code);
            
            result.Should().Be(warehouse);
        }
        
        [Fact]
        public void GivenAWarehouseDoesNotExist_WhenGettingWarehouse_ThenThrowAWarehouseNotFoundException()
        {
            const string code = "warehouse_code";
            
            A.CallTo(() => _hopRepository.GetWarehouseByCode(A<string>._)).Returns(null);

            Action getWarehouse = () => _logic.GetWarehouse(code);
            
            getWarehouse.Should().Throw<WarehouseNotFoundException>();
        }
        
        [Fact]
        public void GivenASingleOrDefaultException_WhenGettingWarehouse_ThenThrowAInvalidWarehouseException()
        {
            const string code = "warehouse_code";

            A.CallTo(() => _hopRepository.GetWarehouseByCode(A<string>._)).Throws<SingleOrDefaultException>();
            
            Action getWarehouse = () => _logic.GetWarehouse(code);
            
            getWarehouse.Should().Throw<InvalidWarehouseException>();
        }
        
        [Fact]
        public void GivenADataAccessErrorOccurs_WhenGettingWarehouse_ThenThrowABusinessException()
        {
            const string code = "warehouse_code";

            A.CallTo(() => _hopRepository.GetWarehouseByCode(A<string>._)).Throws<DataAccessException>();
            
            Action getWarehouse = () => _logic.GetWarehouse(code);
            
            getWarehouse.Should().Throw<BusinessException>();
        }

        [Fact]
        public void GivenAValidWarehouseHierarchy_WhenImportingWarehouses_ThenReturnSuccessfully()
        {
            var validationResults = new ValidationResult();

            var warehouse = Builder<Warehouse>
                .CreateNew()
                .Build();
            
            A.CallTo(() => _warehouseValidator.Validate(A<Warehouse>._)).Returns(validationResults);
            
            Action importWarehouses = () => _logic.ImportWarehouses(warehouse);

            importWarehouses.Should().NotThrow<Exception>();
        }
        
        [Fact]
        public void GivenWarehouseValidationFails_WhenImportingWarehouses_ThenThrowAInvalidWarehouseException()
        {
            var validationFailure = new ValidationResult(new List<ValidationFailure>()
            {
                new ValidationFailure("prop", "message")
            });

            var warehouse = Builder<Warehouse>
                .CreateNew()
                .Build();
            
            A.CallTo(() => _warehouseValidator.Validate(A<Warehouse>._)).Returns(validationFailure);
            
            Action importWarehouses = () => _logic.ImportWarehouses(warehouse);

            importWarehouses.Should().Throw<InvalidWarehouseException>();
        }
        
        [Fact]
        public void GivenADataAccessErrorOccurs_WhenImportingWarehouses_ThenThrowABusinessException()
        {
            var validationResults = new ValidationResult();

            var warehouse = Builder<Warehouse>
                .CreateNew()
                .Build();
            
            A.CallTo(() => _warehouseValidator.Validate(A<Warehouse>._)).Returns(validationResults);
            A.CallTo(() => _hopRepository.Create(A<DataAccess.Entities.Warehouse>._)).Throws<DataAccessException>();
            
            Action importWarehouses = () => _logic.ImportWarehouses(warehouse);

            importWarehouses.Should().Throw<BusinessException>();
        }
    }
}