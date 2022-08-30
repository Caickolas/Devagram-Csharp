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

        public Usuario GetUsuarioPorLoginSenha(string email, string senha)
        {
            return _context.Usuarios.FirstOrDefault(U =>U.Email == email && U.Senha == senha);
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
