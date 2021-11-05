using System.Diagnostics.CodeAnalysis;
using Elsa.SKS.Package.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elsa.SKS.Package.DataAccess.Sql
{
    [ExcludeFromCodeCoverage]
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Hop> Hops { get; set; }
        
        public virtual DbSet<Parcel> Parcels { get; set; }
        
        public virtual DbSet<Warehouse> Warehouses { get; set; }
    }
}