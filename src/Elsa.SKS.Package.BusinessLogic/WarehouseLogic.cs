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

        private IHopRepository _hopRepository;
        
        private readonly IMapper _mapper;
        
        private Warehouse _rootWarehouse= new Warehouse();

        private bool _hierarchyIsLoaded = true;
        

        public WarehouseLogic(IHopRepository hopRepository, IMapper mapper, IValidator<Warehouse> warehouseValidator)
        {
            _hopRepository = hopRepository;
            _warehouseValidator = warehouseValidator;
            _mapper = mapper;
        }

        public static WarehouseLogic CreateWithFaultyWarehouse(IHopRepository hopRepository, IMapper mapper, IValidator<Warehouse> warehouseValidator)
        {
            var warehouseLogic = new WarehouseLogic(hopRepository, mapper, warehouseValidator) { _rootWarehouse = null };
            return warehouseLogic;
        }

        public static WarehouseLogic CreateWithoutLoadedHierarchy(IHopRepository hopRepository, IMapper mapper, IValidator<Warehouse> warehouseValidator)
        {
            var warehouseLogic = new WarehouseLogic(hopRepository, mapper, warehouseValidator) { _hierarchyIsLoaded = false };
            return warehouseLogic;
        }
        
        public Warehouse ExportWarehouses()
        {
            var warehouseHierarchy = _hopRepository.GetAllWarehouses();
            
            var result = _mapper.Map<Warehouse>(warehouseHierarchy);

            if (result == null)
            {
                throw new WarehouseHierarchyNotLoadedException("Warehouse hierarchy was not loaded yet");
            }
            
            if (_rootWarehouse is null)
            {
                throw new InvalidWarehouseException("Root warehouse is null");
            }

            return result;
        }

        public Warehouse GetWarehouse(string code)
        {
            if (!_hopRepository.DoesWarehouseExist(code))
            {
                throw new WarehouseNotFoundException($"Warehouse with code {code} can not be found");
            }

            if (!_hopRepository.IsValidWarehouseCode(code))
            {
                throw new InvalidWarehouseException("Warehouse code is faulty");
            }

            var warehouseEntity = _hopRepository.GetWarehouseByCode(code);
            var result = _mapper.Map<Warehouse>(warehouseEntity);
            
            return result;
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
            _hopRepository.Create(warehouseDal);
        }
    }
}