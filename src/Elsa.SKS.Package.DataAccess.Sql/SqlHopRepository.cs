using System;
using System.Collections.Generic;
using System.Linq;
using Elsa.SKS.Package.DataAccess.Entities;
using Elsa.SKS.Package.DataAccess.Interfaces;
using Elsa.SKS.Package.DataAccess.Sql.Exceptions;
using Microsoft.Extensions.Logging;

namespace Elsa.SKS.Package.DataAccess.Sql
{
    public class SqlHopRepository : IHopRepository
    {
        private readonly IAppDbContext _context;
        
        private readonly ILogger<SqlHopRepository> _logger;

        public SqlHopRepository(IAppDbContext context, ILogger<SqlHopRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Hop Create(Hop hop)
        {
            try
            {
                _context.Hops.Add(hop);
                _context.SaveChanges();
                return hop;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error");
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
                _logger.LogWarning(ex, "Hop not unique error");
                throw new SingleOrDefaultException("More than one hop with this code exists.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error");
                throw new DataAccessException("A database error occurred, see inner exception for details.", ex);
            }
        }

        public void DeleteAll()
        {
            try
            {
                // solve foreign-key constraints for warehouses
                foreach (var warehouse in _context.Warehouses.ToList())
                {
                    warehouse.NextHops.ToList().ForEach(r => r.NextHop = null);
                }

                // solve foreign-key constraints for parcels
                foreach (var parcel in _context.Parcels.ToList())
                {
                    parcel.VisitedHops.ToList().ForEach(r => r.Hop = null);
                    parcel.FutureHops.ToList().ForEach(r => r.Hop = null);
                }
                
                _context.Hops.RemoveRange(_context.Hops);
                _context.Parcels.RemoveRange(_context.Parcels);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Failed to clear database.", ex);
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
                _logger.LogWarning(ex, "Hop not unique");
                throw new SingleOrDefaultException("More than one hop with this code exists.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error");
                throw new DataAccessException("A database error occurred, see inner exception for details.", ex);
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
                _logger.LogWarning(ex, "Root warehouse not unique");
                throw new SingleOrDefaultException("More than one root warehouse exists.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error");
                throw new DataAccessException("A database error occurred, see inner exception for details.", ex);
            }
        }

        public IEnumerable<Truck> GetAllTrucks()
        {
            try
            {
                return _context.Trucks;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Failed to get all trucks.", ex);
            }
        }

        public Warehouse? GetWarehouseByCode(string code)
        {
            try
            {
                return _context.Hops.OfType<Warehouse>().SingleOrDefault(h => h.Code == code);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Warehouse not unique");
                throw new SingleOrDefaultException("More than one warehouse with this code exists.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error");
                throw new DataAccessException("A database error occurred, see inner exception for details.", ex);
            }
        }
    }
    
}