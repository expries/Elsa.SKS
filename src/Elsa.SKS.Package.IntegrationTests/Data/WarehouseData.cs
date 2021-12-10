using System.IO;
using Elsa.SKS.Package.Services.DTOs;
using Newtonsoft.Json;

namespace Elsa.SKS.Package.IntegrationTests.Data
{
    public static class WarehouseData
    {
        public static readonly Warehouse RootWarehouse = LoadWarehouseHierarchy();

        private static Warehouse LoadWarehouseHierarchy()
        {
            var json = File.ReadAllText("Data\\warehouses.json");
            return JsonConvert.DeserializeObject<Warehouse>(json);
        }
    }
}