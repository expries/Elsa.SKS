using System;
using System.Linq;
using Elsa.SKS.Package.DataAccess.Entities;
using Elsa.SKS.Package.DataAccess.Interfaces;
using Elsa.SKS.Package.DataAccess.Sql.Exceptions;
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
            var result = _context.Hops.Find(hop);
            if (result == null)
            {
                return false;
            }

            _context.Hops.Remove(result);
            _context.SaveChanges();

            return true;
        }

        public Warehouse? GetAllWarehouses()
        {
            try
            {
                var rootWarehouse = _context.Warehouses.SingleOrDefault(w => w.Level == 0);
                return rootWarehouse;
            }
            catch (InvalidOperationException ex)
            {
                throw new SingleOrDefaultException("More than one root warehouse exists.", ex);
            }
        }

        public bool DoesHopExist(string code)
        {
            return _context.Hops.Find(code) is not null;
        }

        public bool DoesWarehouseExist(string code)
        {
            return _context.Warehouses.Find(code) is not null;
        }

        public bool IsValidHopCode(string code)
        {
            throw new System.NotImplementedException();
        }

        public bool IsValidWarehouseCode(string code)
        {
            throw new System.NotImplementedException();
        }

        public Warehouse? GetWarehouseByCode(string code)
        {
            try
            {
                return _context.Warehouses.SingleOrDefault(w => w.Code == code);
            }
            catch (InvalidOperationException ex)
            {
                throw new SingleOrDefaultException("More than one warehouse with this code exists.", ex);
            }

        }
    }
    
}