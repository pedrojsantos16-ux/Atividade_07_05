console.log("JS carregado");
const myForm = document.getElementById('cadastroCliente');

myForm.addEventListener('submit', function (event) {

    event.preventDefault();

     console.log("Botão clicado");

    fetch('https://localhost:7214/cliente', {

        method: 'POST',

        credentials: 'include',

        headers: {
            'Content-Type': 'application/json',
        },

        body: JSON.stringify({

            nome: document.getElementById("nome").value,

            email: document.getElementById("email").value,

            senha: document.getElementById("senha").value

        }),

    })

    .then(response => {

        if (!response.ok) {
            throw new Error("Erro ao cadastrar cliente");
        }

        return response.json();

    })

    .then(data => {

        document.getElementById("Mensagem").innerHTML =
            "<h4>Cliente cadastrado com sucesso! <br>" +
            "Seu ID gerado foi: " + data.id + "</h4>";

    })

    .catch(error => {

        document.getElementById("Mensagem").innerHTML =
            "<h4 style='color:red'>Erro ao cadastrar cliente</h4>";

        console.error(error);

    });

});