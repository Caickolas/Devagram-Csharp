using Devagram_Csharp.Dtos;
using Devagram_Csharp.Models;
using Devagram_Csharp.Repository;
using Devagram_Csharp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Devagram_Csharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UsuarioController : BaseController
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly IPublicacaoRepository _publicacaoRepository;
        private readonly ISeguidorRepository _seguidorRepository;


        public UsuarioController(ILogger<UsuarioController> logger, IUsuarioRepository usuarioRepository,
           IPublicacaoRepository publicacaoRepository, ISeguidorRepository seguidorRepository) : base(usuarioRepository)
        {
            _logger = logger;
            _publicacaoRepository = publicacaoRepository;
            _seguidorRepository = seguidorRepository;
        }

        [HttpGet]
        public IActionResult ObterUsuario()
        {
            try
            {
                Usuario usuario = LerToken();

                return Ok(new UsuarioRespostaDto
                {
                    Nome = usuario.Nome,
                    Email = usuario.Email
                });
            }
            catch(Exception e)
            {
                _logger.LogError("Ocorreu um erro ao obter o usuario");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu o seguinte erro:" + e.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
            
        }

        [HttpPut]
        public IActionResult AtualizarUsuario([FromForm] UsuarioRequisicaoDto usuariodto)
        {
            try 
            {
                Usuario usuario = LerToken();
                if (usuariodto != null)
                {
                    var erros = new List<string>();

                    if (string.IsNullOrEmpty(usuariodto.Nome) || string.IsNullOrWhiteSpace(usuariodto.Nome))
                    {
                        erros.Add("Nome invalido");
                    }

                    if (erros.Count > 0)
                    {
                        return BadRequest(new ErrorRespostaDto()
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Erros = erros
                        });
                    }
                    else
                    {



                        CosmicService cosmicservice = new CosmicService();

                        usuario.FotoPerfil = cosmicservice.EnviarImagem(new ImagemDTO {Imagem = usuariodto.FotoPerfil, Nome = usuariodto.Nome.Replace(" ", "")});
                        usuario.Nome = usuariodto.Nome;

                        _usuarioRepository.AtualizarUsuario(usuario);

                    }
                }
                        return Ok("Usuario foi salvo com sucesso!");

                }
            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro ao salvar o usuario");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu o seguinte erro:" + e.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult SalvarUsuario([FromForm] UsuarioRequisicaoDto usuariodto)
        {
            try
            {
                if(usuariodto != null)
                {
                    var erros = new List<string>();

                    if(string.IsNullOrEmpty(usuariodto.Nome) || string.IsNullOrWhiteSpace(usuariodto.Nome))
                    {
                        erros.Add("Nome invalido");
                    }

                    if (string.IsNullOrEmpty(usuariodto.Email) || string.IsNullOrWhiteSpace(usuariodto.Email) || !usuariodto.Email.Contains("@" ))                    {
                        erros.Add("Email invalido");
                    }

                    if (string.IsNullOrEmpty(usuariodto.Senha) || string.IsNullOrWhiteSpace(usuariodto.Senha))
                    {
                        erros.Add("senha invalido");
                    }

                    if (erros.Count > 0)
                    {
                        return BadRequest(new ErrorRespostaDto()
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Erros = erros
                        });
                    }

                    CosmicService cosmicservice = new CosmicService();

                    Usuario usuario = new Usuario()
                    {
                        Email = usuariodto.Email,
                        Senha = usuariodto.Senha,
                        Nome = usuariodto.Nome,
                        FotoPerfil = cosmicservice.EnviarImagem(new ImagemDTO { Imagem = usuariodto.FotoPerfil, Nome = usuariodto.Nome.Replace(" ","") })
                    };

                    usuario.Senha = Utils.MD5Utils.GerarHashMD5(usuario.Senha);
                    usuario.Email = usuario.Email.ToLower();

                    if (!_usuarioRepository.VerificarEmail(usuario.Email))
                    {
                        _usuarioRepository.Salvar(usuario);
                    }
                    else
                    {
                        return BadRequest(new ErrorRespostaDto()
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Descricao = "Usuario já está cadastrado"
                        });
                    }

                }

                return Ok("Usuario foi salvo com sucesso");
            }
            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro ao salvar o usuario");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu o seguinte erro:" + e.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpGet]
        [Route("pesquisaid")]
        public IActionResult PesquisarUsuarioID(int idUsuario)
        {
            try
            {
                Usuario usuario = _usuarioRepository.GetUsuarioPorId(idUsuario);

                int qtdepublicacoes = _usuarioRepository.GetQtdePublicacoes(idUsuario);

                int qtdeseguidores = _seguidorRepository.GetQtdeSeguidores(idUsuario);

                int qtdeseguindo = _seguidorRepository.GetQtdeSeguindo(idUsuario);

                return Ok(new UsuarioRespostaDto
                {
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Avatar = usuario.FotoPerfil,
                    IdUsuario = usuario.Id,
                    QtdePublicacoes = qtdepublicacoes,
                    QtdeSeguidores = qtdeseguidores,
                    QtdeSeguindo = qtdeseguindo
                }); 
            }
            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro ao pesquisar o usuario");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu o seguinte erro:" + e.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpGet]
        [Route("pesquisanome")]
        public IActionResult PesquisarUsuarioNome(string nome)
        {
            try
            {
                List<Usuario> usuarios = _usuarioRepository.GetUsuarioPorNome(nome);

                List<UsuarioRespostaDto> usuariosresposta = new List<UsuarioRespostaDto>();

                foreach (Usuario usuario in usuarios)
                {
                    int qtdepublicacoes = _usuarioRepository.GetQtdePublicacoes(usuario.Id);
                    int qtdeseguidores = _seguidorRepository.GetQtdeSeguidores(usuario.Id);
                    int qtdeseguindo = _seguidorRepository.GetQtdeSeguindo(usuario.Id);

                    usuariosresposta.Add(new UsuarioRespostaDto
                    {
                        Nome = usuario.Nome,
                        Email = usuario.Email,
                        Avatar = usuario.FotoPerfil,
                        IdUsuario = usuario.Id,
                        QtdePublicacoes = qtdepublicacoes,
                        QtdeSeguidores = qtdeseguidores,
                        QtdeSeguindo = qtdeseguindo
                    });
                }
                

                return Ok(usuariosresposta);
            }
            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro ao pesquisar o usuario");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu o seguinte erro:" + e.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }

}
