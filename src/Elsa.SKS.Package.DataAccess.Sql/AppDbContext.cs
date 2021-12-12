using System.Diagnostics.CodeAnalysis;
using Elsa.SKS.Package.DataAccess.Entities;
using Elsa.SKS.Package.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Elsa.SKS.Package.DataAccess.Sql
{
    [ExcludeFromCodeCoverage]
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Hop>()
                .HasOne(_ => _.ParentHop)
                .WithOne(_ => _.NextHop)
                .HasForeignKey<WarehouseNextHop>("NextHopId");

            builder.Entity<GeoCoordinates>()
                .HasOne(_ => _.Hop)
                .WithOne(_ => _.LocationCoordinates)
                .HasForeignKey<GeoCoordinates>("HopId");
        }

        public virtual DbSet<Hop> Hops { get; set; }
        
        public virtual DbSet<Parcel> Parcels { get; set; }
        
        public virtual DbSet<Warehouse> Warehouses { get; set; }
        
        public virtual DbSet<TransferWarehouse> TransferWarehouses { get; set; }
        
        public virtual DbSet<Truck> Trucks { get; set; }

        public virtual DbSet<Subscription> Subscriptions { get; set; }

    }
}