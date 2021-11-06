using System;
using System.Data;
using System.Linq;
using Elsa.SKS.Package.DataAccess.Entities;
using Elsa.SKS.Package.DataAccess.Interfaces;
using Elsa.SKS.Package.DataAccess.Sql.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Elsa.SKS.Package.DataAccess.Sql
{
    public class SqlParcelRepository : IParcelRepository
    {
        private readonly IAppDbContext _context;

        public SqlParcelRepository(IAppDbContext context)
        {
            _context = context;
        }
        
        public Parcel Create(Parcel parcel)
        {
            try
            {
                _context.Parcels.Add(parcel);
                _context.SaveChanges();
                return parcel;
            }
            catch (Exception ex) when (ex is DbUpdateException or DbUpdateConcurrencyException)
            {
                throw new DataAccessException("A database error occurred, see inner exception for details.", ex);
            }
        }

        public Parcel Update(Parcel parcel)
        {
            try
            {
                _context.Parcels.Update(parcel);
                _context.SaveChanges();
                return parcel;
            }
            catch (Exception ex) when (ex is DbUpdateException or DbUpdateConcurrencyException)
            {
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
                throw new SingleOrDefaultException("More than one parcel with this ID exists.", ex);
            }
            catch (Exception ex) when (ex is DbUpdateException or DbUpdateConcurrencyException)
            {
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
                throw new SingleOrDefaultException("More than one parcel with this ID exists.", ex);
            }
        }
    }
}