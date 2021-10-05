using Elsa.SKS.Package.Services.DTOs;

namespace Elsa.SKS.Package.BusinessLogic.Interfaces
{
    public interface IWarehouseLogic
    {
        public Warehouse ExportWarehouses();
        
        public Warehouse GetWarehouse(string code);
        
        public void ImportWarehouses(Warehouse body);
    }
}