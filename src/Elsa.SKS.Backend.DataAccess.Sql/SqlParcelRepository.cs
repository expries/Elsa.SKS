using System;
using System.Linq;
using Elsa.SKS.Backend.DataAccess.Entities;
using Elsa.SKS.Backend.DataAccess.Interfaces;
using Elsa.SKS.Backend.DataAccess.Sql.Exceptions;
using Microsoft.Extensions.Logging;

namespace Elsa.SKS.Backend.DataAccess.Sql
{
    public class SqlParcelRepository : IParcelRepository
    {
        private readonly IAppDbContext _context;

        private readonly ILogger<SqlParcelRepository> _logger;

        public SqlParcelRepository(IAppDbContext context, ILogger<SqlParcelRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Parcel Create(Parcel parcel)
        {
            try
            {
                parcel.FutureHops.ForEach(ha => ha.Hop = _context.Hops.Single(_ => _.Id == ha.Hop.Id));
                parcel.VisitedHops.ForEach(ha => ha.Hop = _context.Hops.Single(_ => _.Id == ha.Hop.Id));
                _context.Parcels.Add(parcel);
                _context.SaveChanges();
                return parcel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error");
                throw new DataAccessException("A database error occurred, see inner exception for details.", ex);
            }
        }

        public Parcel Update(Parcel parcel)
        {
            try
            {
                parcel.FutureHops.ForEach(ha => ha.Hop = _context.Hops.Single(_ => _.Id == ha.Hop.Id));
                parcel.VisitedHops.ForEach(ha => ha.Hop = _context.Hops.Single(_ => _.Id == ha.Hop.Id));
                var storedParcel = _context.Parcels.FirstOrDefault(_ => _.TrackingId == parcel.TrackingId);

                if (storedParcel is not null)
                {
                    _context.Parcels.Remove(storedParcel);
                    _context.SaveChanges();
                }

                _context.Parcels.Update(parcel);
                _context.SaveChanges();
                return parcel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error");
                throw new DataAccessException("A database error occurred, see inner exception for details.", ex);
            }
        }

        public bool Delete(Parcel parcel)
        {
            try
            {
                var result = _context.Parcels.SingleOrDefault(p => p.Id == parcel.Id);

                if (result is null)
                {
                    return false;
                }

                _context.Parcels.Remove(result);
                _context.SaveChanges();
                return true;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Parcel ID error");
                throw new SingleOrDefaultException("More than one parcel with this ID exists.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error");
                throw new DataAccessException("A database error occurred, see inner exception for details.", ex);
            }
        }

        public Parcel? GetByTrackingId(string trackingId)
        {
            try
            {
                var parcel = _context.Parcels.SingleOrDefault(p => p.TrackingId == trackingId);
                return parcel;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Parcel ID error");
                throw new SingleOrDefaultException("More than one parcel with this ID exists.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error");
                throw new DataAccessException("A database error occurred, see inner exception for details.", ex);
            }
        }
    }
}