﻿using System.Diagnostics.CodeAnalysis;
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
            builder.Entity<WarehouseNextHop>()
                .HasOne(_ => _.NextHop)
                .WithOne(_ => _.PreviousHop)
                .HasForeignKey<WarehouseNextHop>(_ => _.Id);
        }

        public virtual DbSet<Hop> Hops { get; set; }
        
        public virtual DbSet<Parcel> Parcels { get; set; }
        
        public virtual DbSet<Warehouse> Warehouses { get; set; }
        
        public virtual DbSet<TransferWarehouse> TransferWarehouses { get; set; }
        
        public virtual DbSet<Truck> Trucks { get; set; }
    }
}