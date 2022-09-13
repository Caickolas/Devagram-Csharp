using Devagram_Csharp.Dtos;
using Devagram_Csharp.Models;
using Devagram_Csharp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Devagram_Csharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ComentarioController : BaseController
    {
        private readonly ILogger<ComentarioController> _logger;
        public readonly IComentarioRepository _comentarioRepository;

        public ComentarioController (ILogger<ComentarioController> logger, 
                                        IComentarioRepository comentarioRepository, IUsuarioRepository usuarioRepository) : base(usuarioRepository)
        {
            _logger = logger;
            _comentarioRepository = comentarioRepository;
        }

        [HttpPut]

        public IActionResult Comentar([FromBody] ComentarioRequisicaoDto comentariodto)
        {
            try
            {
                if (comentariodto != null)
                {
                    if (String.IsNullOrEmpty(comentariodto.Descricao) || String.IsNullOrWhiteSpace(comentariodto.Descricao))
                    {
                        _logger.LogError("Por Favor coloque seu comentario");
                        return BadRequest("Por Favor coloque seu comentario");
                    }

                    Comentario comentario = new Comentario();
                    comentario.Descricao = comentariodto.Descricao;
                    comentario.IdPublicacao = comentariodto.IdPublicacao;
                    comentario.IdUsuario = LerToken().Id;

                    _comentarioRepository.Comentar(comentario);
                }
                return Ok("Comentario Salvo com sucesso");
            }
            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro ao Comentar");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu o seguinte erro:" + e.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}
