document.getElementById("loginForm").addEventListener("submit", async function (e) {

    e.preventDefault();

    const email = document.getElementById("loginEmail").value;

    const senha = parseInt(document.getElementById("loginSenha").value);

    try {

        const response = await fetch('https://localhost:7214/Cliente/login', {

            method: 'POST',

            credentials: 'include',

            headers: {
                'Content-Type': 'application/json'
            },

            body: JSON.stringify({

                email: email,

                senha: senha

            })

        });

        if (!response.ok) {

            throw new Error("Usuário inválido");

        }

        const usuario = await response.json();

        console.log(usuario);

        sessionStorage.setItem("clienteId", usuario.id);

        alert("Login realizado!");

        window.location.href = "tarefas.html";

    } catch (error) {

        console.error(error);

        alert("E-mail ou senha incorretos!");

    }

});