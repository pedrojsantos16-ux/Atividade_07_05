using Atividade_07_05.Models;
using Microsoft.AspNetCore.Mvc;

namespace Atividade_07_05.Controllers

{
    [ApiController]
    [Route("[Controller]")]
    public class LoginController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login(Login login)
        {
            string emailPadrao = "andre@senai.br";
            string senhaPadrao = "123456";

            if (!(login.Senha == senhaPadrao && login.Email == emailPadrao))
            {
                return Unauthorized("Usuário ou senha incorretos");
            }

            HttpContext.Session.SetString("EmailLogin", login.Email);

            Response.Cookies.Append("Usuario", login.Email,
                new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(30),
                    HttpOnly = true
                });

            return Ok("login realizado com sucesso!");
        }
        [HttpGet("Inicial")]
        public IActionResult Inicial()
        {
            var usuario = HttpContext.Session.GetString("EmailLogin");
            if (usuario == null)
            {
                return Unauthorized("Não autenticado no sistema!!!");
            }
            return Ok(new { mensagem = "Usário Logado", email = usuario });

        }
        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(".AspNetCore.Session");
            HttpContext.Session.Clear();
            Response.Cookies.Delete("Email");
            return Ok("Logout realizado com sucesso!!!");
        }
    }
}


