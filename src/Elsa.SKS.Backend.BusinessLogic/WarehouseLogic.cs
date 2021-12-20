using AutoMapper;
using Elsa.SKS.Backend.BusinessLogic.Exceptions;
using Elsa.SKS.Backend.BusinessLogic.Interfaces;
using Elsa.SKS.Backend.DataAccess.Interfaces;
using Elsa.SKS.Backend.DataAccess.Sql.Exceptions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Warehouse = Elsa.SKS.Backend.BusinessLogic.Entities.Warehouse;
using DataAccessWarehouse = Elsa.SKS.Backend.DataAccess.Entities.Warehouse;

namespace Elsa.SKS.Backend.BusinessLogic
{
    public class WarehouseLogic : IWarehouseLogic
    {
        private readonly IHopRepository _hopRepository;

        private readonly IValidator<Warehouse> _warehouseValidator;

        private readonly IMapper _mapper;
        
        private readonly ILogger<WarehouseLogic> _logger;

        public WarehouseLogic(IHopRepository hopRepository, IValidator<Warehouse> warehouseValidator, IMapper mapper, ILogger<WarehouseLogic> logger)
        {
            _hopRepository = hopRepository;
            _warehouseValidator = warehouseValidator;
            _mapper = mapper;
            _logger = logger;
        }

        public Warehouse ExportWarehouses()
        {
            try
            {
                var warehouseHierarchy = _hopRepository.GetAllWarehouses();

                if (warehouseHierarchy is null)
                {
                    _logger.LogWarning("Warehouse hierarchy not loaded");
                    throw new WarehouseHierarchyNotLoadedException("Warehouse hierarchy was not loaded yet.");
                }
            
                var result = _mapper.Map<Warehouse>(warehouseHierarchy);
                return result;
            }
            catch (SingleOrDefaultException ex)
            {
                _logger.LogWarning(ex, "Root warehouse is not unique");
                throw new InvalidWarehouseException("Root warehouse is not unique.", ex);
            }
            catch (DataAccessException ex)
            {
                _logger.LogError(ex, "Database error");
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
                    _logger.LogInformation("Warehouse with code an not be found");
                    throw new WarehouseNotFoundException($"Warehouse with code {code} can not be found.");
                }

                var warehouse = _mapper.Map<Warehouse>(warehouseEntity);
                return warehouse;
            }
            catch (SingleOrDefaultException ex)
            {
                _logger.LogWarning(ex, "Warehouse is not unique");
                throw new InvalidWarehouseException("Warehouse is not unique.", ex);
            }
            catch (DataAccessException ex)
            {
                _logger.LogError(ex, "Database error");
                throw new BusinessException("A database has occurred.", ex);
            }
        }

        public void ImportWarehouses(Warehouse warehouse)
        {
            var validation = _warehouseValidator.Validate(warehouse);

            if (!validation.IsValid)
            {
                _logger.LogDebug("Validation failed");
                throw new InvalidWarehouseException(validation.ToString(" "));
            }

            try
            {
                var warehouseDal = _mapper.Map<DataAccessWarehouse>(warehouse);
                _hopRepository.DeleteAll();
                _hopRepository.Create(warehouseDal);
            }
            catch (DataAccessException ex)
            {
                _logger.LogError(ex, "Database error");
                throw new BusinessException("A database has occurred.", ex);
            }
        }
    }
    
}