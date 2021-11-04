using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.DataAccess.Interfaces;
using Elsa.SKS.Package.DataAccess.Sql.Exceptions;
using FluentValidation;
using Warehouse = Elsa.SKS.Package.BusinessLogic.Entities.Warehouse;
using DataAccessWarehouse = Elsa.SKS.Package.DataAccess.Entities.Warehouse;

namespace Elsa.SKS.Package.BusinessLogic
{
    public class WarehouseLogic : IWarehouseLogic
    {
        private readonly IHopRepository _hopRepository;

        private readonly IValidator<Warehouse> _warehouseValidator;

        private readonly IMapper _mapper;
        
        public WarehouseLogic(IHopRepository hopRepository, IValidator<Warehouse> warehouseValidator, IMapper mapper)
        {
            _hopRepository = hopRepository;
            _warehouseValidator = warehouseValidator;
            _mapper = mapper;
        }

        public Warehouse ExportWarehouses()
        {
            try
            {
                var warehouseHierarchy = _hopRepository.GetAllWarehouses();

                if (warehouseHierarchy is null)
                {
                    throw new WarehouseHierarchyNotLoadedException("Warehouse hierarchy was not loaded yet");
                }
            
                var result = _mapper.Map<Warehouse>(warehouseHierarchy);
                return result;
            }
            catch (SingleOrDefaultException ex)
            {
                throw new InvalidWarehouseException("Root warehouse is not unique.", ex);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("A database has occurred.", ex);
            }
        }

        public Warehouse GetWarehouse(string code)
        {
            try
            {
                var warehouseEntity = _hopRepository.GetWarehouseByCode(code);

                if (warehouseEntity is null)
                {
                    throw new WarehouseNotFoundException($"Warehouse with code {code} can not be found");
                }

                var warehouse = _mapper.Map<Warehouse>(warehouseEntity);
                return warehouse;
            }
            catch (SingleOrDefaultException ex)
            {
                throw new InvalidWarehouseException("Warehouse is not unique.", ex);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("A database has occurred.", ex);
            }
        }

        public void ImportWarehouses(Warehouse warehouse)
        {
            var validation = _warehouseValidator.Validate(warehouse);

            if (!validation.IsValid)
            {
                throw new InvalidWarehouseException(validation.ToString(" "));
            }

            try
            {
                var warehouseDal = _mapper.Map<DataAccessWarehouse>(warehouse);
                _hopRepository.Create(warehouseDal);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("A database has occurred.", ex);
            }
        }
    }
    
}