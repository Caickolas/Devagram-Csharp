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
        private readonly IComentarioRepository _comentarioRepository;
        private readonly ICurtidaRepository _curtidaRepository;

        public PublicacaoController(ILogger<PublicacaoController> logger,
            IPublicacaoRepository publicacaoRepository, IUsuarioRepository usuarioRepository, 
            IComentarioRepository comentarioRepository, ICurtidaRepository curtidaRepository) : base(usuarioRepository)
        {
            _logger = logger;
            _publicacaoRepository = publicacaoRepository;
            _comentarioRepository = comentarioRepository;
            _curtidaRepository = curtidaRepository;
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

        [HttpGet]
        [Route("Feed")]
        public IActionResult FeedHome()
        {
            try
            {
                List<PublicacaoFeedRespostaDto> feed = _publicacaoRepository.GetPublicacoesFeed(LerToken().Id);

                foreach (PublicacaoFeedRespostaDto feedResposta in feed)
                {
                    Usuario usuario = _usuarioRepository.GetUsuarioPorId(feedResposta.IdUsuario);
                    UsuarioRespostaDto usuarioRespostaDto = new UsuarioRespostaDto()
                    {
                        Nome = usuario.Nome,
                        Avatar = usuario.FotoPerfil,
                        IdUsuario = usuario.Id

                    };
                    feedResposta.Usuario = usuarioRespostaDto;

                    List<Comentario> comentarios = _comentarioRepository.GetComentarioPorPublicacao(feedResposta.IdPublicacao);
                    feedResposta.Comentarios = comentarios;

                    List<Curtida> curtidas = _curtidaRepository.GetCurtidaPorPublicacao(feedResposta.IdPublicacao);
                    feedResposta.Curtidas = curtidas;

                }
                return Ok(feed);
            }
            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro ao Obter Feed");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu o seguinte erro:" + e.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpGet]
        [Route("FeedUsuario")]
        public IActionResult FeedUsuario(int idUsuario)
        {
            try
            {
                List<PublicacaoFeedRespostaDto> feed = _publicacaoRepository.GetPublicacoesFeedUsuario(idUsuario);

                foreach (PublicacaoFeedRespostaDto feedResposta in feed)
                {
                    Usuario usuario = _usuarioRepository.GetUsuarioPorId(feedResposta.IdUsuario);
                    UsuarioRespostaDto usuarioRespostaDto = new UsuarioRespostaDto()
                    {
                        Nome = usuario.Nome,
                        Avatar = usuario.FotoPerfil,
                        IdUsuario = usuario.Id

                    };
                    feedResposta.Usuario = usuarioRespostaDto;

                    List<Comentario> comentarios = _comentarioRepository.GetComentarioPorPublicacao(feedResposta.IdPublicacao);
                    feedResposta.Comentarios = comentarios;

                    List<Curtida> curtidas = _curtidaRepository.GetCurtidaPorPublicacao(feedResposta.IdPublicacao);
                    feedResposta.Curtidas = curtidas;

                }
                return Ok(feed);
            }
            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro ao Obter Feed do Usuario");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu o seguinte erro:" + e.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}
