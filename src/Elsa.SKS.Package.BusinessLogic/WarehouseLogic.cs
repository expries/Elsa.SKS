using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.BusinessLogic.Validators;
using FluentValidation;
using Warehouse = Elsa.SKS.Package.BusinessLogic.Entities.Warehouse;

namespace Elsa.SKS.Package.BusinessLogic
{
    public class WarehouseLogic : IWarehouseLogic
    {
        private readonly IValidator<Warehouse> _warehouseValidator;
        
        private Warehouse _rootWarehouse= new Warehouse();

        private bool _hierarchyIsLoaded = true;

        public WarehouseLogic(IValidator<Warehouse> warehouseValidator)
        {
            _warehouseValidator = warehouseValidator;
        }

        public static WarehouseLogic CreateWithFaultyWarehouse(IValidator<Warehouse> warehouseValidator)
        {
            var warehouseLogic = new WarehouseLogic(warehouseValidator) { _rootWarehouse = null };
            return warehouseLogic;
        }

        public static WarehouseLogic CreateWithoutLoadedHierarchy(IValidator<Warehouse> warehouseValidator)
        {
            var warehouseLogic = new WarehouseLogic(warehouseValidator) { _hierarchyIsLoaded = false };
            return warehouseLogic;
        }
        
        public Warehouse ExportWarehouses()
        {
            if (!_hierarchyIsLoaded)
            {
                throw new WarehouseHierarchyNotLoadedException("Warehouse hierarchy was not loaded yet");
            }
            
            if (_rootWarehouse is null)
            {
                throw new InvalidWarehouseException("Root warehouse is null");
            }

            return _rootWarehouse;
        }

        public Warehouse GetWarehouse(string code)
        {
            switch (code)
            {
                case TestConstants.NonExistentWarehouseCode:
                    throw new WarehouseNotFoundException($"Warehouse with code {code} can not be found");
                
                case TestConstants.FaultyWarehouseCode:
                    throw new InvalidWarehouseException("Warehouse code is faulty");
                
                default:
                    var warehouse = new Warehouse();
                    return warehouse;
            } 
        }

        public void ImportWarehouses(Warehouse warehouse)
        {
            var validation = _warehouseValidator.Validate(warehouse);

            if (!validation.IsValid)
            {
                throw new InvalidWarehouseException(validation.ToString(" "));
            }
            
            if (string.IsNullOrEmpty(warehouse.Code))
            {
                throw new InvalidWarehouseException("Warehouse code is null or empty");
            }
        }
    }
}