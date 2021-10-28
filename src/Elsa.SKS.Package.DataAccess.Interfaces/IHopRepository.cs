using System.Collections.Generic;
using Elsa.SKS.Package.DataAccess.Entities;

namespace Elsa.SKS.Package.DataAccess.Interfaces
{
    public interface IHopRepository
    {
        public Hop Create(Hop hop);
        public bool Update(Hop hop);
        public bool Delete(Hop hop);
        public Warehouse GetAllWarehouses();
        public bool DoesHopExist(string code);
        public bool DoesWarehouseExist(string code);
        public bool IsValidHopCode(string code);
        public bool IsValidWarehouseCode(string code);
        public Warehouse GetWarehouseByCode(string code);

        /*
        public Warehouse Create(Warehouse warehouse);
        public bool Update(Warehouse warehouse);
        public bool Delete(Warehouse warehouse);
        public Warehouse GetWarehouseByCode(string code);
        public Warehouse GetAllWarehouses();
        public bool DoesExist(string code);
        public bool IsValidWarehouseCode(string code);
        */
    }
}