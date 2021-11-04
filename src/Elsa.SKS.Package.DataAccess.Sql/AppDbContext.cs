﻿using Elsa.SKS.Package.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elsa.SKS.Package.DataAccess.Sql
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Hop> Hops { get; set; }
        
        public DbSet<HopArrival> HopsArrivals { get; set; }
        public DbSet<Parcel> Parcels { get; set; }
        
        public DbSet<Truck> Trucks { get; set; }

        public DbSet<Warehouse> Warehouses { get; set; }
        
        public DbSet<TransferWarehouse> TransferWarehouses { get; set; }
    }
}