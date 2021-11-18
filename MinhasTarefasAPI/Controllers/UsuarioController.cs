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
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MinhasTarefasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsuarioController(IConfiguration config,
                                 IUsuarioRepository usuarioRepository,
                                 SignInManager<ApplicationUser> signInManager,
                                 UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _usuarioRepository = usuarioRepository;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("login")]
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

            return Ok(BuildToken(usuario));

        }

        [HttpPost("")]
        public ActionResult Cadastrar([FromBody] UsuarioDTO usuarioDTO)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);


            ApplicationUser usuario = new ApplicationUser();
            usuario.FullName = usuarioDTO.Nome;
            usuario.UserName = usuarioDTO.Email;
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

        private object BuildToken(ApplicationUser usuario)
        {
            var id = usuario.Id;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email)
            };
            var appSettingsKey = _config["Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettingsKey));
            var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expire = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expire,
                signingCredentials: sign
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new { token =tokenString , expiration = expire };

        }
    }
}
