using System.Linq;
using Elsa.SKS.Package.DataAccess.Entities;
using Elsa.SKS.Package.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Elsa.SKS.Package.DataAccess.Sql
{
    public class SqlHopRepository : IHopRepository
    {
        private readonly AppDbContext _context;
        
        public SqlHopRepository(AppDbContext context)
        {
            _context = context;
        }

        public Hop Create(Hop hop)
        {
            _context.Hops.Add(hop);
            _context.SaveChanges();
            
            // var warehouses = _context.Warehouses.ToList();
            // var hops = _context.Hops.Where(h => h is Warehouse).ToList();
            
            return hop;
        }

        public bool Update(Hop hop)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(Hop hop)
        {
            throw new System.NotImplementedException();
        }

        public Warehouse GetAllWarehouses()
        {
            throw new System.NotImplementedException();
        }

        public bool DoesHopExist(string code)
        {
            throw new System.NotImplementedException();
        }

        public bool DoesWarehouseExist(string code)
        {
            throw new System.NotImplementedException();
        }

        public bool IsValidHopCode(string code)
        {
            throw new System.NotImplementedException();
        }

        public bool IsValidWarehouseCode(string code)
        {
            throw new System.NotImplementedException();
        }

        public Warehouse GetWarehouseByCode(string code)
        {
            throw new System.NotImplementedException();
        }
    }
}