using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MinhasTarefasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasTarefasAPI.Database
{
    public class MinhasTarefasContext : IdentityDbContext<ApplicationUser>
    {
        public MinhasTarefasContext(DbContextOptions<MinhasTarefasContext> opt) : base(opt)
        {
        }

        public DbSet<Tarefa> Tarefas { get; set; }
    }
}
