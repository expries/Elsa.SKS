using System;
using System.Collections.Generic;
using System.Linq;
using Elsa.SKS.Package.DataAccess.Entities;
using Elsa.SKS.Package.DataAccess.Interfaces;
using Elsa.SKS.Package.DataAccess.Sql;
using Elsa.SKS.Package.DataAccess.Sql.Exceptions;
using FakeItEasy;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Elsa.SKS.Package.DataAccess.Tests
{
    public class SqlParcelRepositoryTests : DataAccessTests
    {
        private readonly IAppDbContext _context;
        
        private readonly IParcelRepository _parcelRepository;
        
        private readonly ILogger<SqlParcelRepository> _logger;

        public SqlParcelRepositoryTests()
        {
            _context = GetMockedAppDbContext();
            _logger = A.Fake<ILogger<SqlParcelRepository>>();
            _parcelRepository = new SqlParcelRepository(_context, _logger);
        }

        [Fact]
        public void GivenAParcel_WhenCreatingParcel_ThenAddParcelAndReturnIt()
        {
            var parcel = Builder<Parcel>
                .CreateNew()
                .With(_ => _.FutureHops = new List<HopArrival>())
                .With(_ => _.VisitedHops = new List<HopArrival>())
                .Build();
            
            _parcelRepository.Create(parcel);

            _context.Parcels.Count().Should().Be(1);
            _context.Parcels.Should().Contain(parcel);
        }
        
        [Fact]
        public void GivenADbUpdateExceptionIsThrown_WhenCreatingParcel_ThenThrowADataAccessException()
        {
            var parcel = Builder<Parcel>
                .CreateNew()
                .With(_ => _.FutureHops = new List<HopArrival>())
                .With(_ => _.VisitedHops = new List<HopArrival>())
                .Build();

            A.CallTo(() => _context.Parcels.Add(A<Parcel>._)).Throws<DbUpdateException>();
            
            Action createParcel = () => _parcelRepository.Create(parcel);

            createParcel.Should().Throw<DataAccessException>();
        }
        
        [Fact]
        public void GivenADbUpdateConcurrencyExceptionIsThrown_WhenCreatingParcel_ThenThrowADataAccessException()
        {
            var parcel = Builder<Parcel>
                .CreateNew()
                .With(_ => _.FutureHops = new List<HopArrival>())
                .With(_ => _.VisitedHops = new List<HopArrival>())
                .Build();

            A.CallTo(() => _context.Parcels.Add(A<Parcel>._)).Throws<DbUpdateConcurrencyException>();
            
            Action createParcel = () => _parcelRepository.Create(parcel);

            createParcel.Should().Throw<DataAccessException>();
        }
        
        [Fact]
        public void GivenAParcel_WhenUpdatingParcel_ThenUpdateParcelAndReturnIt()
        {
            var parcel = Builder<Parcel>
                .CreateNew()
                .With(_ => _.FutureHops = new List<HopArrival>())
                .With(_ => _.VisitedHops = new List<HopArrival>())
                .Build();

            _context.Parcels.Update(parcel);
            var updatedParcel = _parcelRepository.Update(parcel);

            _context.Parcels.Count().Should().Be(1);
            _context.Parcels.First().Should().Be(updatedParcel);
        }
        
        [Fact]
        public void GivenADbUpdateExceptionIsThrown_WhenUpdatingParcel_ThenThrowADataAccessException()
        {
            var parcel = Builder<Parcel>
                .CreateNew()
                .With(_ => _.FutureHops = new List<HopArrival>())
                .With(_ => _.VisitedHops = new List<HopArrival>())
                .Build();

            A.CallTo(() => _context.Parcels.Update(A<Parcel>._)).Throws<DbUpdateException>();
            
            Action updateParcel = () => _parcelRepository.Update(parcel);

            updateParcel.Should().Throw<DataAccessException>();
        }
        
        [Fact]
        public void GivenADbUpdateConcurrencyExceptionIsThrown_WhenUpdatingParcel_ThenThrowADataAccessException()
        {
            var parcel = Builder<Parcel>
                .CreateNew()
                .With(_ => _.FutureHops = new List<HopArrival>())
                .With(_ => _.VisitedHops = new List<HopArrival>())
                .Build();

            A.CallTo(() => _context.Parcels.Update(A<Parcel>._)).Throws<DbUpdateConcurrencyException>();
            
            Action updateParcel = () => _parcelRepository.Update(parcel);

            updateParcel.Should().Throw<DataAccessException>();
        }
        
        [Fact]
        public void GivenAParcelExists_WhenDeletingParcel_ThenReturnTrue()
        {
            var parcel = Builder<Parcel>
                .CreateNew()
                .Build();

            _context.Parcels.Add(parcel);
            
            bool deleteParcel = _parcelRepository.Delete(parcel);

            deleteParcel.Should().BeTrue();
            _context.Parcels.Count().Should().Be(0);
        }
        
        [Fact]
        public void GivenAParcelDoesNotExist_WhenDeletingParcel_ThenReturnFalse()
        {
            var parcel = Builder<Parcel>
                .CreateNew()
                .Build();
            
            bool deleteParcel = _parcelRepository.Delete(parcel);

            deleteParcel.Should().BeFalse();
        }
        
        [Fact]
        public void GivenMultipleParcelsWithTheSameIdExist_WhenDeletingParcel_ThenThrowASingleOrDefaultException()
        {
            var parcelA = Builder<Parcel>
                .CreateNew()
                .Build();
            
            var parcelB = Builder<Parcel>
                .CreateNew()
                .With(p => p.Id == parcelA.Id)
                .Build();

            _context.Parcels.Add(parcelA);
            _context.Parcels.Add(parcelB);
            
            Action deleteParcel = () => _parcelRepository.Delete(parcelA);

            deleteParcel.Should().Throw<SingleOrDefaultException>();
        }
        
        [Fact]
        public void GivenADbUpdateExceptionIsThrown_WhenDeletingParcel_ThenThrowADataAccessException()
        {
            var parcel = Builder<Parcel>
                .CreateNew()
                .Build();

            A.CallTo(() => _context.Parcels).Throws<DbUpdateException>();
            
            Action deleteParcel = () => _parcelRepository.Delete(parcel);

            deleteParcel.Should().Throw<DataAccessException>();
        }
        
        [Fact]
        public void GivenADbUpdateConcurrencyException_WhenDeletingParcel_ThenThrowADataAccessException()
        {
            var parcel = Builder<Parcel>
                .CreateNew()
                .Build();

            A.CallTo(() => _context.Parcels).Throws<DbUpdateConcurrencyException>();
            
            Action deleteParcel = () => _parcelRepository.Delete(parcel);

            deleteParcel.Should().Throw<DataAccessException>();
        }

        [Fact]
        public void GivenAParcelExists_WhenGettingParcel_ThenReturnAParcel()
        {
            var storedParcel = Builder<Parcel>
                .CreateNew()
                .Build();

            _context.Parcels.Add(storedParcel);

            var parcel = _parcelRepository.GetByTrackingId(storedParcel.TrackingId);

            parcel.Should().Be(storedParcel);
        }
        
        [Fact]
        public void GivenAParcelDoesNotExist_WhenGettingParcel_ThenReturnNull()
        {
            const string trackingId = "tracking_id";
            
            var parcel = _parcelRepository.GetByTrackingId(trackingId);

            parcel.Should().BeNull();
        }

        [Fact] public void GivenMultipleParcelsWithTheSameTrackingIdExist_WhenGettingParcel_ThenThrowSingleOrDefaultException()
        {
            var parcelA = Builder<Parcel>
                .CreateNew()
                .Build();
            
            var parcelB = Builder<Parcel>
                .CreateNew()
                .With(p => p.Id == parcelA.Id)
                .Build();

            _context.Parcels.Add(parcelA);
            _context.Parcels.Add(parcelB);
            
            Action deleteParcel = () => _parcelRepository.GetByTrackingId(parcelA.TrackingId);

            deleteParcel.Should().Throw<SingleOrDefaultException>();
        }
    }
}