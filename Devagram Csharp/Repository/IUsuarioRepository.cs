using Devagram_Csharp.Models;

namespace Devagram_Csharp.Repository
{
    public interface IUsuarioRepository
    {
        public void Salvar(Usuario usuario);

        public bool VerificarEmail(string email);

        Usuario GetUsuarioPorLoginSenha(string email, string senha);
        Usuario GetUsuarioPorId(int id);

        public void AtualizarUsuario(Usuario usuario);
        int GetQtdePublicacoes(int idUsuario);
        List<Usuario> GetUsuarioPorNome(string nome);
    }


}
