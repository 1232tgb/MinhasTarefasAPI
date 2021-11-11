using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinhasTarefasAPI.Models;
using MinhasTarefasAPI.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;

namespace MinhasTarefasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsuarioController(IUsuarioRepository usuarioRepository,
                                 SignInManager<ApplicationUser> signInManager,
                                 UserManager<ApplicationUser> userManager)
        {
            _usuarioRepository = usuarioRepository;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public ActionResult Login([FromBody] UsuarioDTO usuarioDTO)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("ConfirmacaoSenha");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var usuario = _usuarioRepository.Obter(usuarioDTO.Email, usuarioDTO.Senha);

            if (usuario == null)
                return NotFound("Usuario não encontrado!");

            _signInManager.SignInAsync(usuario, false);

            return Ok();

        }


        public ActionResult Cadastrar([FromBody] UsuarioDTO usuarioDTO)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);


            ApplicationUser usuario = new ApplicationUser();
            usuario.FullName = usuarioDTO.Nome;
            usuario.Email = usuarioDTO.Email;

            var resultado = _userManager.CreateAsync(usuario, usuarioDTO.Senha).Result;


            if (!resultado.Succeeded)
            {
                List<string> sb = new List<string>();
                foreach (var error in resultado.Errors)
                    sb.Add(error.Description);


                return UnprocessableEntity(sb);
            }

            return Ok(usuario);
        }

    }
}
