using Devagram_Csharp.Models;

namespace Devagram_Csharp.Repository.Impl
{
    public class UsuarioRepositoryImpl : IUsuarioRepository
    {

        private readonly DevagramContext _context;

        public UsuarioRepositoryImpl ( DevagramContext context)
        {
            _context = context;
        }

        public void AtualizarUsuario(Usuario usuario)
        {
            _context.Update(usuario);
            _context.SaveChanges();

        }

        public int GetQtdePublicacoes(int idUsuario)
        {
            return _context.Publicacoes.Count(p => p.IdUsuario == idUsuario);
        }

        public Usuario GetUsuarioPorId(int id)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Id == id );
        }

        public Usuario GetUsuarioPorLoginSenha(string email, string senha)
        {
            return _context.Usuarios.FirstOrDefault(U =>U.Email == email && U.Senha == senha);
        }

        public List<Usuario> GetUsuarioPorNome(string nome)
        {
            return _context.Usuarios.Where(u => u.Nome.Contains(nome)).ToList();
        }

        public void Salvar(Usuario usuario)
        {
           _context.Add(usuario);
           _context.SaveChanges();
        }

        public bool VerificarEmail(string email)
        {
            return _context.Usuarios.Any(u => u.Email == email);
        }
    }
}
