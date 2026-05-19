using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Atividade_07_05.Models;
using Atividade_07_05.Data;

namespace Gerenciador.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly GerenciadorContext _context;

        public TarefaController(GerenciadorContext context)
        {
            _context = context;
        }

        [HttpGet("tarefasCliente")]
        public IActionResult TarefasCliente()
        {   
            var sessaoUsuario = HttpContext.Session.GetString("IdCliente");

            if (sessaoUsuario == null)
            {
                return Unauthorized("Faça login antes");
            }

            int idLogado = int.Parse(sessaoUsuario);

            var resultado = from c in _context.Clientes
                            join t in _context.Tarefas
                            on c.Id equals t.Propietario

                            where c.Id == idLogado

                            select new
                            {
                                Id = t.Id,

                                Cliente = c.Nome,

                                Email = c.Email,

                                Descricao = t.Descricao,

                                Status = t.Status
                            };

            return Ok(resultado.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult RetornaTarefa(int id)
        {
            var sessaoUsuario = HttpContext.Session.GetString("IdCliente");

            if (sessaoUsuario == null)
            {
                return Unauthorized("Não autenticado");
            }

            var tarefa = _context.Tarefas.Find(id);

            if (tarefa == null)
            {
                return NotFound("Tarefa não encontrada");
            }

            return Ok(tarefa);
        }


        [HttpPost]
        public IActionResult CadastraTarefa([FromBody] Tarefa tarefa)
        {
            var sessaoUsuario = HttpContext.Session.GetString("IdCliente");

            if (sessaoUsuario == null)
            {
                return Unauthorized("Não autenticado");
            }

            tarefa.Propietario = int.Parse(sessaoUsuario);

            _context.Tarefas.Add(tarefa);

            _context.SaveChanges();

            return Ok(tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizaTarefa(int id, [FromBody] Tarefa tarefa)
        {
            var sessaoUsuario = HttpContext.Session.GetString("IdCliente");

            if (sessaoUsuario == null)
            {
                return Unauthorized("Não autenticado");
            }

            var tarefaDoBanco = _context.Tarefas.Find(id);

            if (tarefaDoBanco == null)
            {
                return NotFound("Tarefa não encontrada");
            }

            tarefaDoBanco.Descricao = tarefa.Descricao;

            tarefaDoBanco.Status = tarefa.Status;

            _context.SaveChanges();

            return Ok("Tarefa atualizada");
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarTarefa(int id)
        {
            var sessaoUsuario = HttpContext.Session.GetString("IdCliente");

            if (sessaoUsuario == null)
            {
                return Unauthorized("Não autenticado");
            }

            var tarefa = _context.Tarefas.Find(id);

            if (tarefa == null)
            {
                return NotFound("Tarefa não encontrada");
            }

            _context.Tarefas.Remove(tarefa);

            _context.SaveChanges();

            return Ok("Tarefa deletada");
        }


        [HttpGet]
        public IActionResult Listar()
        {
            return Ok(_context.Tarefas.ToList());
        }
    }
}