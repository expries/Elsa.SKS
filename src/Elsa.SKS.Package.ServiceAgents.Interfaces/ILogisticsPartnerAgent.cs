using Elsa.SKS.Package.BusinessLogic.Entities;

namespace Elsa.SKS.Package.ServiceAgents.Interfaces
{
    public interface ILogisticsPartnerAgent
    {
        public void TransferParcel(TransferWarehouse warehouse, Parcel parcel);
    }
}