using Elsa.SKS.Package.BusinessLogic.Entities;

namespace Elsa.SKS.Package.DataAccess.Interfaces
{
    public interface IWarehouseRepository
    {
        public Warehouse Create(Warehouse warehouse);
        public bool Update(Warehouse warehouse);
        public bool Delete(Warehouse warehouse);
        public Warehouse GetWarehouseByCode(string code);
    }
}