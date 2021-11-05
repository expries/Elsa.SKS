using System;
using System.Data;
using System.Linq;
using Elsa.SKS.Package.DataAccess.Entities;
using Elsa.SKS.Package.DataAccess.Interfaces;
using Elsa.SKS.Package.DataAccess.Sql.Exceptions;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                _context.Hops.Add(hop);
                _context.SaveChanges();
                return hop;
            }
            catch (Exception ex) when (ex is DbUpdateException or DbUpdateConcurrencyException)
            {
                throw new DataAccessException("A database error occurred, see inner exception for details.", ex);
            }
        }

        public Hop Update(Hop hop)
        {
            try
            {
                _context.Hops.Update(hop);
                _context.SaveChanges();
                return hop;
            }
            catch (Exception ex) when (ex is DbUpdateException or DbUpdateConcurrencyException)
            {
                throw new DataAccessException("A database error occurred, see inner exception for details.", ex);
            }
        }

        public bool Delete(Hop hop)
        {
            try
            {
                var result = _context.Hops.SingleOrDefault(h => h.Code == hop.Code);
                
                if (result is null)
                {
                    return false;
                }

                _context.Hops.Remove(result);
                _context.SaveChanges();
                return true;
            }
            catch (InvalidOperationException ex)
            {
                throw new SingleOrDefaultException("More than one hop with this code exists.", ex);
            }
            catch (Exception ex) when (ex is DbUpdateException or DbUpdateConcurrencyException)
            {
                throw new DataAccessException("A database error occurred, see inner exception for details.", ex);
            }
        }

        public Hop? GetByCode(string code)
        {
            try
            {
                var hop = _context.Hops.SingleOrDefault(h => h.Code == code);
                return hop;
            }
            catch (InvalidOperationException ex)
            {
                throw new SingleOrDefaultException("More than one hop with this code exists.", ex);
            }
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