using System.Collections.Generic;
using Elsa.SKS.Backend.DataAccess.Entities;

namespace Elsa.SKS.Backend.DataAccess.Interfaces
{
    public interface IHopRepository
    {
        public Hop Create(Hop hop);

        public Hop Update(Hop hop);

        public bool Delete(Hop hop);

        public void DeleteAll();

        public Hop? GetByCode(string code);

        public Warehouse? GetWarehouseByCode(string code);

        public Warehouse? GetAllWarehouses();

        public IEnumerable<Truck> GetAllTrucks();

        public IEnumerable<TransferWarehouse> GetAllTransferWarehouses();
    }
}