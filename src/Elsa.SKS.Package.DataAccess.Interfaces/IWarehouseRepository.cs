using System.Collections.Generic;
using Elsa.SKS.Package.DataAccess.Entities;

namespace Elsa.SKS.Package.DataAccess.Interfaces
{
    public interface IWarehouseRepository
    {
        public Warehouse Create(Warehouse warehouse);
        public bool Update(Warehouse warehouse);
        public bool Delete(Warehouse warehouse);
        public Warehouse GetWarehouseByCode(string code);
        public Warehouse GetAllWarehouses();
        public bool DoesExist(string code);
        public bool IsValidWarehouseCode(string code);
    }
}