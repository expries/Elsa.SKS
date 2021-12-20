using Elsa.SKS.Backend.BusinessLogic.Entities;

namespace Elsa.SKS.Backend.BusinessLogic.Interfaces
{
    public interface IWarehouseLogic
    {
        public Warehouse ExportWarehouses();

        public Warehouse GetWarehouse(string code);

        public void ImportWarehouses(Warehouse warehouse);
    }
}