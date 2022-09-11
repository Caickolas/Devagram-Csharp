using Devagram_Csharp.Dtos;
using Devagram_Csharp.Models;
using Devagram_Csharp.Repository;
using Devagram_Csharp.Services;
using Microsoft.AspNetCore.Mvc;

namespace Devagram_Csharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublicacaoController : BaseController
    {
        private readonly ILogger<PublicacaoController> _logger;
        private readonly IPublicacaoRepository _publicacaoRepository;

        public PublicacaoController(ILogger<PublicacaoController> logger,
            IPublicacaoRepository publicacaoRepository, IUsuarioRepository usuarioRepository) : base(usuarioRepository)
        {
            _logger = logger;
            _publicacaoRepository = publicacaoRepository;
        }

        [HttpPost]
        public IActionResult Publicar([FromForm] PublicacaoRequisicaoDto publicacaodto)
        {
            try
            {
                Usuario usuario = LerToken();
                CosmicService cosmicservice = new CosmicService();
                if (publicacaodto != null)
                {
                    if(string.IsNullOrEmpty(publicacaodto.Descricao) &&
                        string.IsNullOrWhiteSpace(publicacaodto.Descricao))
                    {
                        _logger.LogError("A descricao esta invalida");
                        return BadRequest("Voce precisa Preencher a descrição");
                    }
                    if (publicacaodto.Foto == null)
                    {
                        _logger.LogError("A foto esta invalida");
                        return BadRequest("è obrigatorio a foto na publicacao");
                    }

                    Publicacao publicacao = new Publicacao()
                    {
                        Descricao = publicacaodto.Descricao,
                        IdUsuario = usuario.Id,
                        Foto = cosmicservice.EnviarImagem(new ImagemDTO { Imagem = publicacaodto.Foto, Nome = "publicacao" })
                    };
                    _publicacaoRepository.Publicar(publicacao);
                }
                return Ok("Publicação salva com sucesso");
            }
            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro ao Publicar" );
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu o seguinte erro:" + e.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

    }
}
