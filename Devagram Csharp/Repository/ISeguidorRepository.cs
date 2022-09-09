using Devagram_Csharp.Models;

namespace Devagram_Csharp.Repository
{
    public interface ISeguidorRepository
    {
        public bool Seguir(Seguidor seguidor);
        public bool Desseguir(Seguidor seguidor);
        
        public Seguidor GetSeguidor(int idseguidor, int idseguido);
    }
}
