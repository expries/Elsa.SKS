using System;
using System.Collections.Generic;
using System.Linq;
using Elsa.SKS.Package.DataAccess.Entities;
using Elsa.SKS.Package.DataAccess.Sql;
using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Elsa.SKS.Package.DataAccess.Tests
{
    public class DataAccessTests
    {
        protected static AppDbContext GetMockedAppDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dbContext = A.Fake<AppDbContext>(o => 
                o.WithArgumentsForConstructor(new object[] { options }));
            
            var hopSet = GetQueryableMockDbSet<Hop>();
            var parcelSet = GetQueryableMockDbSet<Parcel>();
            var warehouseSet = GetQueryableMockDbSet<Warehouse>();

            A.CallTo(() => dbContext.Hops).Returns(hopSet);
            A.CallTo(() => dbContext.Parcels).Returns(parcelSet);
            A.CallTo(() => dbContext.Warehouses).Returns(warehouseSet);
            
            return dbContext;
        }
        
        private static DbSet<T> GetQueryableMockDbSet<T>() where T : class
        {
            var list = new List<T>();
            return GetQueryableMockDbSet(list);
        }
        
        private static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var dbSet = A.Fake<DbSet<T>>(option => option.Implements<IQueryable<T>>());
            
            A.CallTo(() => dbSet.As<IQueryable<T>>().Provider).Returns(queryable.Provider);
            A.CallTo(() => dbSet.As<IQueryable<T>>().Expression).Returns(queryable.Expression);
            A.CallTo(() => dbSet.As<IQueryable<T>>().ElementType).Returns(queryable.ElementType);
            A.CallTo(() => dbSet.As<IQueryable<T>>().GetEnumerator()).ReturnsLazily(() => queryable.GetEnumerator());
            A.CallTo(() => dbSet.Add(A<T>._)).Invokes((T s) => sourceList.Add(s));
            A.CallTo(() => dbSet.Remove(A<T>._)).Invokes((T s) => sourceList.Remove(s));
            A.CallTo(() => dbSet.Update(A<T>._)).Invokes((T s) =>
            {
                sourceList.Remove(s);
                sourceList.Add(s);
            });
            
            return dbSet;
        }
    }
}