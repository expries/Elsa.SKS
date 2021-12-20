using System.Collections.Generic;
using Elsa.SKS.Backend.DataAccess.Entities;

namespace Elsa.SKS.Backend.DataAccess.Interfaces
{
    public interface ISubscriberRepository
    {
        public Subscription Create(Subscription subscription);

        public Subscription Update(Subscription subscription);

        public bool Delete(long? id);

        public bool DeleteAllByTrackingId(string trackingId);

        public IEnumerable<Subscription> GetByTrackingId(string trackingId);

        public Subscription? GetById(long? id);


    }
}