﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MinhasTarefasAPI.Models
{
    public class UsuarioDTO
    {
        [Required]
        public string Nome { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Senha { get; set; }
        
        [Required]
        [Compare("Senha")]
        public string ConfirmacaoSenha { get; set; }
    }
}
