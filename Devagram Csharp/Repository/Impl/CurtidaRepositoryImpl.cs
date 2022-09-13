using Devagram_Csharp.Models;

namespace Devagram_Csharp.Repository.Impl
{
    public class CurtidaRepositoryImpl : ICurtidaRepository
    {
        private readonly DevagramContext _context;

        public CurtidaRepositoryImpl (DevagramContext context)
        {
            _context = context;
        }

        public void Curtir(Curtida curtida)
        {
            _context.Add(curtida);
            _context.SaveChanges();
        }

        public void Descurtir(Curtida curtida)
        {
            _context.Remove(curtida);
            _context.SaveChanges();
        }

        public Curtida GetCurtida(int idPublicacao, int idUsuario)
        {
            return _context.Curtidas.FirstOrDefault(c => c.IdPublicacao == idPublicacao && c.IdUsuario == idUsuario);

        }
    }
}
