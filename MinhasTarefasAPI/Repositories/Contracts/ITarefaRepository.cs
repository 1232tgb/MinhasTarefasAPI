﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinhasTarefasAPI.Models;

namespace MinhasTarefasAPI.Repositories.Contracts
{
    public interface ITarefaRepository
    {
        List<Tarefa> Sincronizacao(List<Tarefa> tarefas);
        List<Tarefa> Restauracao(ApplicationUser usuario, DateTime dataUltimaSincronizacao);
     }
}
