using Microsoft.AspNetCore.Mvc;
using Atividade_07_05.Data;
using Atividade_07_05.Models;
namespace Gerencia.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly GerenciadorContext _context;

        public ClienteController(GerenciadorContext context)
        {
            _context = context;
        }


        [HttpGet("{id}")]
        public IActionResult RetornaCliente(int id)
        {
            var clientes = _context.Clientes.Find(id);
            if (clientes == null)
            {
                return NotFound("Não há clientes com esse Id!");
            }
            return Ok(clientes);

        }

        [HttpPost]
        public IActionResult CadastraCliente(Cliente cliente)
        {
            _context.Add(cliente);
            _context.SaveChanges();
            return Created("", cliente);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizaCliente(Cliente cliente)
        {
            var clienteDoBanco = _context.Clientes.Find("IdCliente");
            if (clienteDoBanco == null)
            {
                return NotFound("Cliente não existe no banco!");
            }
            clienteDoBanco.Nome = cliente.Nome;
            clienteDoBanco.Email = cliente.Email;
            clienteDoBanco.Senha = cliente.Senha;
            _context.SaveChanges();
            return Ok("Atualizado");
        }

        [HttpDelete("{id}")]
        public IActionResult DeletaCliente(int id)
        {
            var clienteDoBanco = _context.Clientes.Find(id);
            if (clienteDoBanco == null)
            {
                return NotFound("Não encontrado!");
            }
            _context.Remove(clienteDoBanco);
            _context.SaveChanges();
            return Ok("Deletado");
        }


        [HttpPost("login")]
        public IActionResult Login(Cliente cliente)
        {
            var clientes = _context.Clientes
                 .Where(c => c.Email.Equals(cliente.Email) &&
              c.Senha.Equals(cliente.Senha)).ToList();
            if (!clientes.Any())
            {
                return Unauthorized("Usuário ou senha Inválidos!");
            }
            HttpContext.Session.SetString("IdCliente", Convert.ToString(clientes[0].Id));
            Response.Cookies.Append("IdCliente", Convert.ToString(clientes[0].Id));
            new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(10),
                HttpOnly = true
            };
            return Ok("login realizado com sucesso");

        }
    }
}