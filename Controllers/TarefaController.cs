using Microsoft.AspNetCore.Mvc;
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



        [HttpGet("{id}")]
        public IActionResult RetornaTarefa(int id)
        {
            var clientes = HttpContext.Session.GetString("IdCliente");

            if (clientes == null)
                return Unauthorized("Não autenticado");
            var tarefas = _context.Tarefas.Find(id);
            if (tarefas == null)
            {
                return NotFound("Tarefa não encontrada");
            }
            return Ok(tarefas);
        }

        [HttpGet("tarefasCliente/{id}")]
        public IActionResult TarefasCliente(int Id)
        {
            var clientes = HttpContext.Session.GetString("IdCliente");

            if (clientes == null)
                return Unauthorized("Não autenticado");
            var resultado = from c in _context.Clientes
                            join t in _context.Tarefas
                            on c.Id equals t.Propietario
                            where Id == c.Id
                            select new
                            {
                                Cliente = c.Nome,
                                c.Email,
                                c.Senha,
                                Tarefas = t.Descricao,
                                t.Status,
                                t.Propietario

                            };
            return Ok(resultado.ToList());
        }

        [HttpPost]
        public IActionResult CadastraTarefa(Tarefa tarefa)
        {
            var clientes = HttpContext.Session.GetString("IdCliente");

            if (clientes == null)
                return Unauthorized("Não autenticado");

            int id = int.Parse(clientes);

            tarefa.Dono = id;

            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();

            return Created("", tarefa);
        }
        [HttpPut("{id}")]
        public IActionResult AtualizaTarefa(int id, Tarefa tarefa)

        {

            var clientes = HttpContext.Session.GetString("IdCliente");
            if (clientes == null)
            {
                return Unauthorized("Não autenticado no sistem!");

            }

            var tarefaDoBanco = _context.Tarefas.Find(id);
            if (tarefaDoBanco == null)
            {
                return NotFound("Tarefa não existe no banco!");
            }
            tarefaDoBanco.Descricao = tarefa.Descricao;
            tarefaDoBanco.Status = tarefa.Status;

            _context.SaveChanges();
            return Ok("Atualizado");
        }

        [HttpDelete("{id}")]
        public IActionResult DeletaTarefa(int id)
        {

            var clientes = HttpContext.Session.GetString("IdCliente");
            if (clientes == null)
            {
                return Unauthorized("Não autenticado no sistem!");

            }

            var tarefaDoBanco = _context.Tarefas.Find(id);
            if (tarefaDoBanco == null)
            {
                return NotFound("Não encontrado!");
            }
            _context.Remove(tarefaDoBanco);
            _context.SaveChanges();
            return Ok("Deletado");
        }
    }
}