using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.DataAccess.Interfaces;
using FluentValidation;
using Warehouse = Elsa.SKS.Package.BusinessLogic.Entities.Warehouse;

namespace Elsa.SKS.Package.BusinessLogic
{
    public class WarehouseLogic : IWarehouseLogic
    {
        private readonly IValidator<Warehouse> _warehouseValidator;

        private IWarehouseRepository _warehouseRepository;
        
        private readonly IMapper _mapper;
        
        private Warehouse _rootWarehouse= new Warehouse();

        private bool _hierarchyIsLoaded = true;
        

        public WarehouseLogic(IWarehouseRepository warehouseRepository, IMapper mapper, IValidator<Warehouse> warehouseValidator)
        {
            _warehouseValidator = warehouseValidator;
            _mapper = mapper;
            _warehouseRepository = warehouseRepository;
        }

        public static WarehouseLogic CreateWithFaultyWarehouse(IWarehouseRepository warehouseRepository, IMapper mapper, IValidator<Warehouse> warehouseValidator)
        {
            var warehouseLogic = new WarehouseLogic(warehouseRepository, mapper, warehouseValidator) { _rootWarehouse = null };
            return warehouseLogic;
        }

        public static WarehouseLogic CreateWithoutLoadedHierarchy(IWarehouseRepository warehouseRepository, IMapper mapper, IValidator<Warehouse> warehouseValidator)
        {
            var warehouseLogic = new WarehouseLogic(warehouseRepository, mapper, warehouseValidator) { _hierarchyIsLoaded = false };
            return warehouseLogic;
        }
        
        public Warehouse ExportWarehouses()
        {
            var warehouseHierarchy = _warehouseRepository.GetAllWarehouses();
            
            var result = _mapper.Map<Warehouse>(warehouseHierarchy);

            if (result == null)
            {
                throw new WarehouseHierarchyNotLoadedException("Warehouse hierarchy was not loaded yet");
            }
            
            if (result.Level.Value == 0)
            {
                throw new InvalidWarehouseException("Root warehouse is null");
            }

            return result;
            
            /*
            if (!_hierarchyIsLoaded)
            {
                throw new WarehouseHierarchyNotLoadedException("Warehouse hierarchy was not loaded yet");
            }
            
            if (_rootWarehouse is null)
            {
                throw new InvalidWarehouseException("Root warehouse is null");
            }

            return _rootWarehouse;
            
            */
        }

        public Warehouse GetWarehouse(string code)
        {
            if (!_warehouseRepository.DoesExist(code))
            {
                throw new WarehouseNotFoundException($"Warehouse with code {code} can not be found");
            }

            if (!_warehouseRepository.IsValidWarehouseCode(code))
            {
                throw new InvalidWarehouseException("Warehouse code is faulty");
            }

            var warehouseEntity = _warehouseRepository.GetWarehouseByCode(code);
            var result = _mapper.Map<Warehouse>(warehouseEntity);
            
            return result;

            /*
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
            */
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

            var warehouseDal = _mapper.Map<Elsa.SKS.Package.DataAccess.Entities.Warehouse>(warehouse);
            _warehouseRepository.Create(warehouseDal);
        }
    }
}