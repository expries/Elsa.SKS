using System.Collections.Generic;
using Elsa.SKS.Package.DataAccess.Entities;

namespace Elsa.SKS.Package.DataAccess.Interfaces
{
    public interface IHopRepository
    {
        public Hop Create(Hop hop);

        public Hop Update(Hop hop);
        
        public bool Delete(Hop hop);

        public void DeleteAll();
        
        public Hop? GetByCode(string code);

        public Warehouse? GetAllWarehouses();

        public IEnumerable<Truck> GetAllTrucks();
        
        public Warehouse? GetWarehouseByCode(string code);
    }
}