using Elsa.SKS.Backend.BusinessLogic.Entities;

namespace Elsa.SKS.Backend.ServiceAgents.Interfaces
{
    public interface ILogisticsPartnerAgent
    {
        public void TransferParcel(TransferWarehouse warehouse, Parcel parcel);
    }
}