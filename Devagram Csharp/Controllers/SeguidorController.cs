using Devagram_Csharp.Dtos;
using Devagram_Csharp.Models;
using Devagram_Csharp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Devagram_Csharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeguidorController : BaseController
    {
        private readonly ILogger<SeguidorController> _logger;
        private readonly ISeguidorRepository _seguidorRepository;

        public SeguidorController (ILogger<SeguidorController> logger, 
            ISeguidorRepository seguidorRepository, IUsuarioRepository usuarioRepository) : base(usuarioRepository)
        {
            _logger = logger;
            _seguidorRepository = seguidorRepository;
        }

        [HttpPut]
        public IActionResult Seguir (int idSeguido)
        {
            try
            {
                Usuario usuarioseguido = _usuarioRepository.GetUsuarioPorId(idSeguido);
                Usuario usuarioseguidor = LerToken();

                if (usuarioseguido != null)
                {
                    Seguidor seguidor = _seguidorRepository.GetSeguidor(usuarioseguidor.Id, usuarioseguido.Id);
                    if (seguidor!= null)
                    {
                        _seguidorRepository.Desseguir(seguidor);
                        return Ok("Voce desseguiu um usuario");
                    }
                    else
                    {

                    Seguidor seguidornovo = new Seguidor()
                    {
                        IdUsuarioSeguido = usuarioseguido.Id,
                        IdUsuarioSeguidor = usuarioseguidor.Id

                    };
                    _seguidorRepository.Seguir(seguidornovo);
                        return Ok("Voce seguiu um usuario");
                    }
                }
                else
                {
                    return BadRequest("Ocorreu um erro ao Seguir/desseguir");
                }
               
            } 
            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro ao Seguir/Desseguir");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu o seguinte erro:" + e.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}
