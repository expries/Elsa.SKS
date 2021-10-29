using Microsoft.EntityFrameworkCore;

namespace Elsa.SKS.Package.DataAccess.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        
        public DbSet<GeoCoordinate> GeoCoordinates { get; set; }
        public DbSet<Hop> Hops { get; set; }
        public DbSet<HopArrival> HopArrivals { get; set; }
        public DbSet<Parcel> Parcels { get; set; }
        public DbSet<TransferWarehouse> TransferWarehouses { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseNextHops> WarehouseNextHops { get; set; }
        


    }
}