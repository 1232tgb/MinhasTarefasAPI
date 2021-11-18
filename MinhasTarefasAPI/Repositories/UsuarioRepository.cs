using Microsoft.AspNetCore.Identity;
using MinhasTarefasAPI.Models;
using MinhasTarefasAPI.Repositories.Contracts;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasTarefasAPI.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsuarioRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        public ApplicationUser Obter(string email, string senha)
        {
            var usuario = _userManager.FindByEmailAsync(email).Result;

            if (_userManager.CheckPasswordAsync(usuario, senha).Result)
                return usuario;

            throw new Exception("Usuario não localizado");
        }

        public void Cadastrar(ApplicationUser usuario, string senha)
        {
            var response = _userManager.CreateAsync(usuario, senha).Result;
            if (!response.Succeeded)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var erro in response.Errors)
                    sb.Append(erro.Description);

                throw new Exception($"Usuario não foi criado. Erro = {sb.ToString()}");

            }
        }

    }
}
