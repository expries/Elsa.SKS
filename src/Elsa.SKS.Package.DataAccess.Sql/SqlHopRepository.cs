using Elsa.SKS.Package.DataAccess.Entities;
using Elsa.SKS.Package.DataAccess.Interfaces;

namespace Elsa.SKS.Package.DataAccess.Sql
{
    public class SqlHopRepository : IHopRepository
    {
        public Hop Create(Hop hop)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(Hop hop)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(Hop hop)
        {
            throw new System.NotImplementedException();
        }

        public Warehouse GetAllWarehouses()
        {
            throw new System.NotImplementedException();
        }

        public bool DoesHopExist(string code)
        {
            throw new System.NotImplementedException();
        }

        public bool DoesWarehouseExist(string code)
        {
            throw new System.NotImplementedException();
        }

        public bool IsValidHopCode(string code)
        {
            throw new System.NotImplementedException();
        }

        public bool IsValidWarehouseCode(string code)
        {
            throw new System.NotImplementedException();
        }

        public Warehouse GetWarehouseByCode(string code)
        {
            throw new System.NotImplementedException();
        }
    }
}