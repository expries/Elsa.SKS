using Elsa.SKS.Package.BusinessLogic.Entities;

namespace Elsa.SKS.Package.DataAccess.Interfaces
{
    public interface IHopRepository
    {
        public Hop Create(Hop hop);
        public bool Update(Hop hop);
        public bool Delete(Hop hop);
        public Hop GetHop(string code);
    }
}