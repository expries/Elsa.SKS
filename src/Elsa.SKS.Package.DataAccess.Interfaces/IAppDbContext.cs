using Elsa.SKS.Package.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elsa.SKS.Package.DataAccess.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<Parcel> Parcels { get; }
        
        public DbSet<Hop> Hops { get; }
        
        public DbSet<Warehouse> Warehouses { get; }
        
        public DbSet<TransferWarehouse> TransferWarehouses { get; }
        
        public DbSet<Truck> Trucks { get; }

        public int SaveChanges();
    }
}