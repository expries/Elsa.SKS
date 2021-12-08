using System;
using System.Linq;
using System.Collections.Generic;
using Elsa.SKS.Package.DataAccess.Entities;
using Elsa.SKS.Package.DataAccess.Interfaces;
using Elsa.SKS.Package.DataAccess.Sql.Exceptions;
using Microsoft.Extensions.Logging;

namespace Elsa.SKS.Package.DataAccess.Sql
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly IAppDbContext _context;
        
        private readonly ILogger<SubscriberRepository> _logger;
        
        public SubscriberRepository(IAppDbContext context, ILogger<SubscriberRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public Subscription Create(Subscription subscription)
        {
            try
            {
                _context.Subscriptions.Add(subscription);
                _context.SaveChanges();
                return subscription;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error");
                throw new DataAccessException("A database error occurred, see inner exception for details.", ex);
            }
        }

        public Subscription Update(Subscription subscription)
        {
            try
            {
                _context.Subscriptions.Update(subscription);
                _context.SaveChanges();
                return subscription;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error");
                throw new DataAccessException("A database error occurred, see inner exception for details.", ex);
            }
        }

        public bool Delete(long? id)
        {
            try
            {
                var result = _context.Subscriptions.SingleOrDefault(s => s.Id == id);
                
                if (result is null)
                {
                    return false;
                }

                _context.Subscriptions.Remove(result);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error");
                throw new DataAccessException("A database error occurred, see inner exception for details.", ex);
            }
        }
        
        public bool DeleteAllByTrackingId(string trackingId)
        {
            try
            {
                var result = _context.Subscriptions.Where(s => s.TrackingId == trackingId);
                
                if (!result.Any())
                {
                    _logger.LogInformation("No subscriptions found for this id");
                }

                _context.Subscriptions.RemoveRange(result);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error");
                throw new DataAccessException("A database error occurred, see inner exception for details.", ex);
            }
        }

        public IEnumerable<Subscription> GetByTrackingId(string trackingId)
        {
            try
            {
                return _context.Subscriptions.Where(s => s.TrackingId == trackingId);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Failed to get subscriptions for trackingId.", ex);
            }
        }

        public Subscription? GetById(long? id)
        {
            try
            {
                return _context.Subscriptions.SingleOrDefault(s => s.Id == id);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Failed to get subscriptions for trackingId.", ex);
            }
        }
        
    }
}