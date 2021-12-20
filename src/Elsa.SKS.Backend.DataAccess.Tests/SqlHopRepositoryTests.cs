using System;
using System.Linq;
using Elsa.SKS.Backend.DataAccess.Entities;
using Elsa.SKS.Backend.DataAccess.Interfaces;
using Elsa.SKS.Backend.DataAccess.Sql;
using Elsa.SKS.Backend.DataAccess.Sql.Exceptions;
using FakeItEasy;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Elsa.SKS.Backend.DataAccess.Tests
{
    public class SqlHopRepositoryTests : DataAccessTests
    {
        private readonly IAppDbContext _context;

        private readonly IHopRepository _hopRepository;

        private readonly ILogger<SqlHopRepository> _logger;

        public SqlHopRepositoryTests()
        {
            _context = GetMockedAppDbContext();
            _logger = A.Fake<ILogger<SqlHopRepository>>();
            _hopRepository = new SqlHopRepository(_context, _logger);
        }

        [Fact]
        public void GivenAHop_WhenCreatingHop_AddItToDatabase()
        {
            var hop = Builder<Hop>
                .CreateNew()
                .Build();

            var createdHop = _hopRepository.Create(hop);

            _context.Hops.Count().Should().Be(1);
            _context.Hops.Should().Contain(hop);
            createdHop.Should().Be(hop);
        }

        [Fact]
        public void GivenADbUpdateExceptionExceptionIsThrown_WhenCreatingHop_ThenADataAccessExceptionIsThrown()
        {
            var hop = Builder<Hop>
                .CreateNew()
                .Build();

            A.CallTo(() => _context.Hops.Add(A<Hop>._)).Throws<DbUpdateException>();
            Action create = () => _hopRepository.Create(hop);

            create.Should().Throw<DataAccessException>();
        }

        [Fact]
        public void GivenADbUpdateConcurrencyExceptionIsThrown_WhenCreatingHop_ThenADataAccessExceptionIsThrown()
        {
            var hop = Builder<Hop>
                .CreateNew()
                .Build();

            A.CallTo(() => _context.Hops.Add(A<Hop>._)).Throws<DbUpdateConcurrencyException>();
            Action create = () => _hopRepository.Create(hop);

            create.Should().Throw<DataAccessException>();
        }

        [Fact]
        public void GivenAHop_WhenUpdatingHop_ThenUpdateAndReturnHop()
        {
            var hop = Builder<Hop>
                .CreateNew()
                .Build();

            _context.Hops.Add(hop);

            hop.LocationName = "Example";

            var updatedHop = _hopRepository.Update(hop);

            _context.Hops.Count().Should().Be(1);
            _context.Hops.First().LocationName.Should().Be(hop.LocationName);
            updatedHop.Should().Be(hop);
        }

        [Fact]
        public void GivenADbUpdateExceptionExceptionIsThrown_WhenUpdatingHop_ThenADataAccessExceptionIsThrown()
        {
            var hop = Builder<Hop>
                .CreateNew()
                .Build();

            A.CallTo(() => _context.Hops.Update(A<Hop>._)).Throws<DbUpdateException>();
            Action create = () => _hopRepository.Update(hop);

            create.Should().Throw<DataAccessException>();
        }

        [Fact]
        public void GivenADbUpdateConcurrencyExceptionIsThrown_WhenUpdatingHop_ThenADataAccessExceptionIsThrown()
        {
            var hop = Builder<Hop>
                .CreateNew()
                .Build();

            A.CallTo(() => _context.Hops.Update(A<Hop>._)).Throws<DbUpdateConcurrencyException>();
            Action create = () => _hopRepository.Update(hop);

            create.Should().Throw<DataAccessException>();
        }

        [Fact]
        public void GivenAHopExists_WhenDeletingHop_ThenReturnTrue()
        {
            var hop = Builder<Hop>
                .CreateNew()
                .Build();

            _context.Hops.Add(hop);

            bool hopWasDeleted = _hopRepository.Delete(hop);

            _context.Hops.Count().Should().Be(0);
            hopWasDeleted.Should().BeTrue();
        }

        [Fact]
        public void GivenAHopDoesNotExist_WhenDeletingHop_ThenReturnFalse()
        {
            var hop = Builder<Hop>
                .CreateNew()
                .Build();

            bool hopWasDeleted = _hopRepository.Delete(hop);

            _context.Hops.Count().Should().Be(0);
            hopWasDeleted.Should().BeFalse();
        }

        [Fact]
        public void GivenMultipleHopsWithTheSameCodeExist_WhenDeletingHop_ThenASingleOrDefaultExceptionIsThrown()
        {
            var hopA = Builder<Hop>
                .CreateNew()
                .Build();

            var hopB = Builder<Hop>
                .CreateNew()
                .With(h => h.Code = hopA.Code)
                .Build();

            _context.Hops.Add(hopA);
            _context.Hops.Add(hopB);

            Action deleteHop = () => _hopRepository.Delete(hopA);

            deleteHop.Should().Throw<SingleOrDefaultException>();
        }

        [Fact]
        public void GivenADbUpdateExceptionExceptionIsThrown_WhenDeletingHop_ThenADataAccessExceptionIsThrown()
        {
            var hop = Builder<Hop>
                .CreateNew()
                .Build();

            _context.Hops.Add(hop);
            A.CallTo(() => _context.Hops.Remove(A<Hop>._)).Throws<DbUpdateException>();
            Action delete = () => _hopRepository.Delete(hop);

            delete.Should().Throw<DataAccessException>();
        }

        [Fact]
        public void GivenADbUpdateConcurrencyExceptionIsThrown_WhenDeletingHop_ThenADataAccessExceptionIsThrown()
        {
            var hop = Builder<Hop>
                .CreateNew()
                .Build();

            _context.Hops.Add(hop);
            A.CallTo(() => _context.Hops.Remove(A<Hop>._)).Throws<DbUpdateConcurrencyException>();
            Action create = () => _hopRepository.Delete(hop);

            create.Should().Throw<DataAccessException>();
        }

        [Fact]
        public void GivenAHopExists_WhenGettingHop_ThenReturnHop()
        {
            var storedHop = Builder<Hop>
                .CreateNew()
                .Build();

            _context.Hops.Add(storedHop);
            var hop = _hopRepository.GetByCode(storedHop.Code);

            hop.Should().Be(storedHop);
        }

        [Fact]
        public void GivenAHopDoesNotExist_WhenGettingHop_ThenReturnNull()
        {
            const string hopCode = "hop_code";

            var hop = _hopRepository.GetByCode(hopCode);

            hop.Should().BeNull();
        }

        [Fact]
        public void GivenAInvalidOperationExceptionIsThrown_WhenGettingHop_ThenASingleOrDefaultExceptionIsThrown()
        {
            const string hopCode = "hop_code";

            A.CallTo(() => _context.Hops).Throws<InvalidOperationException>();
            Action getByCode = () => _hopRepository.GetByCode(hopCode);

            getByCode.Should().Throw<SingleOrDefaultException>();
        }

        [Fact]
        public void GivenAWarehouseExist_WhenGettingAllWarehouses_ThenReturnRootWarehouse()
        {
            var storedWarehouse = Builder<Warehouse>
                .CreateNew()
                .With(w => w.Level = 0)
                .Build();

            _context.Warehouses.Add(storedWarehouse);
            var rootWarehouse = _hopRepository.GetAllWarehouses();

            rootWarehouse.Should().Be(storedWarehouse);
        }

        [Fact]
        public void GivenAWarehouseDoesNotExist_WhenGettingAllWarehouses_ThenReturnNull()
        {
            var rootWarehouse = _hopRepository.GetAllWarehouses();
            rootWarehouse.Should().BeNull();
        }

        [Fact]
        public void GivenAInvalidOperationExceptionIsThrown_WhenGettingAllWarehouses_ThenASingleOrDefaultExceptionIsThrown()
        {
            A.CallTo(() => _context.Warehouses).Throws<InvalidOperationException>();
            Action getAllWarehouses = () => _hopRepository.GetAllWarehouses();

            getAllWarehouses.Should().Throw<SingleOrDefaultException>();
        }

        [Fact]
        public void GivenAWarehouseExist_WhenGettingWarehouseByCode_ThenReturnWarehouse()
        {
            var storedWarehouse = Builder<Warehouse>
                .CreateNew()
                .With(w => w.Level = 0)
                .Build();

            _context.Hops.Add(storedWarehouse);
            var warehouse = _hopRepository.GetWarehouseByCode(storedWarehouse.Code);

            warehouse.Should().Be(storedWarehouse);
        }

        [Fact]
        public void GivenAInvalidOperationExceptionIsThrown_WhenGettingWarehouseByCode_ThenASingleOrDefaultExceptionIsThrown()
        {
            const string warehouseCode = "warehouse_code";
            A.CallTo(() => _context.Hops).Throws<InvalidOperationException>();

            Action getAllWarehouses = () => _hopRepository.GetWarehouseByCode(warehouseCode);

            getAllWarehouses.Should().Throw<SingleOrDefaultException>();
        }
    }
}