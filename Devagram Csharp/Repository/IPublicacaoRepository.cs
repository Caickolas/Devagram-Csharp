using Devagram_Csharp.Dtos;
using Devagram_Csharp.Models;

namespace Devagram_Csharp.Repository
{
    public interface IPublicacaoRepository
    {
        List<PublicacaoFeedRespostaDto> GetPublicacoesFeed(int idUsuario);
        public void Publicar(Publicacao publicacao);
    }
}
