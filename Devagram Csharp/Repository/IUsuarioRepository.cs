using Devagram_Csharp.Models;

namespace Devagram_Csharp.Repository
{
    public interface IUsuarioRepository
    {
        public void Salvar(Usuario usuario);

        public bool VerificarEmail(string email);
    }


}
