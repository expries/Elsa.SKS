using System.Collections.Generic;
using Elsa.SKS.Package.DataAccess.Entities;

namespace Elsa.SKS.Package.DataAccess.Interfaces
{
    public interface ISubscriberRepository
    {
        public Subscription Create(Subscription subscription);
        
        public Subscription Update(Subscription subscription);
        
        public bool Delete(Subscription subscription);
        
        public IEnumerable<Subscription> GetByTrackingId(string trackingId);
    }
}