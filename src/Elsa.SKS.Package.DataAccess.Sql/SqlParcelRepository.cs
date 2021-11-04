﻿using System;
using Elsa.SKS.Package.DataAccess.Entities;
using Elsa.SKS.Package.DataAccess.Interfaces;

namespace Elsa.SKS.Package.DataAccess.Sql
{
    public class SqlParcelRepository : IParcelRepository
    {
        private readonly AppDbContext _context;

        public SqlParcelRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public Parcel Create(Parcel parcel)
        {
            _context.Parcels.Add(parcel);
            _context.SaveChanges();
            return parcel;
        }

        public Parcel Update(Parcel parcel)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Parcel parcel)
        {
            var result = _context.Parcels.Find(parcel);
            if (result == null)
            {
                return false;
            }

            _context.Parcels.Remove(result);
            _context.SaveChanges();

            return true;
        }

        public bool ReportParcelHopArrival(string trackingId)
        {
            //var nextHop = _context.Parcels.Find(trackingId).FutureHops;

            throw new NotImplementedException();
        }

        public Parcel GetParcelByTrackingId(string trackingId)
        {
            return _context.Parcels.Find(trackingId);
        }

        public bool DoesExist(string trackingId)
        {
            return _context.Parcels.Find(trackingId) is not null;
        }
    }
}