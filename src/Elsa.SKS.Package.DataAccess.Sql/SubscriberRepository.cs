using System.Collections.Generic;
using Elsa.SKS.Package.DataAccess.Entities;
using Elsa.SKS.Package.DataAccess.Interfaces;

namespace Elsa.SKS.Package.DataAccess.Sql
{
    public class SubscriberRepository : ISubscriberRepository
    {
        public Subscription Create(Subscription subscription)
        {
            throw new System.NotImplementedException();
        }

        public Subscription Update(Subscription subscription)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(Subscription subscription)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Subscription> GetByTrackingId(string trackingId)
        {
            throw new System.NotImplementedException();
        }
    }
}