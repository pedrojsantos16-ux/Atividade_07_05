const tarefaForm = document.getElementById("tarefaForm");

const listaTarefas = document.getElementById("listaTarefas");

let tarefaEditandoId = null;

async function listarTarefas() {

    try {

        const response = await fetch(
            'https://localhost:7214/Tarefa/tarefasCliente',
            {
                method: 'GET',

                credentials: 'include'
            }
        );

        if (!response.ok) {

            throw new Error("Erro ao buscar tarefas");

        }

        const data = await response.json();

        listaTarefas.innerHTML = "";

        data.forEach(tarefa => {

            const div = document.createElement("div");

            div.classList.add("tarefa");

            div.innerHTML = `
                <strong>Status:</strong>
                ${tarefa.status}

                <br><br>

                <strong>Descrição:</strong>
                ${tarefa.descricao}

                <br><br>
            `;


            const botaoEditar = document.createElement("button");

            botaoEditar.textContent = "Editar";

            botaoEditar.addEventListener("click", () => editar(tarefa));

 
            const botaoDeletar = document.createElement("button");

            botaoDeletar.textContent = "Deletar";

            botaoDeletar.addEventListener(
                "click",
                () => deletar(tarefa.id)
            );

            div.appendChild(botaoEditar);

            div.appendChild(botaoDeletar);

            listaTarefas.appendChild(div);

            listaTarefas.appendChild(
                document.createElement("hr")
            );

        });

    } catch (error) {

        console.error(error);

    }

}


function editar(tarefa) {

    tarefaEditandoId = tarefa.id;

    document.getElementById("status").value =
        tarefa.status;

    document.getElementById("descricao").value =
        tarefa.descricao;

}


tarefaForm.addEventListener(
    "submit",
    async function (e) {

        e.preventDefault();

        const tarefa = {

            status: document.getElementById("status").value,

            descricao: document
                .getElementById("descricao")
                .value

        };

        if (!tarefa.descricao.trim()) {

            alert("Digite uma descrição");

            return;

        }

        try {

            let response;

            if (tarefaEditandoId !== null) {

                response = await fetch(
                    `https://localhost:7214/Tarefa/${tarefaEditandoId}`,
                    {

                        method: 'PUT',

                        credentials: 'include',

                        headers: {
                            'Content-Type': 'application/json'
                        },

                        body: JSON.stringify(tarefa)

                    }
                );

            }

            else {

                response = await fetch(
                    'https://localhost:7214/Tarefa',
                    {

                        method: 'POST',

                        credentials: 'include',

                        headers: {
                            'Content-Type': 'application/json'
                        },

                        body: JSON.stringify(tarefa)

                    }
                );

            }

            if (!response.ok) {

                throw new Error(
                    "Erro ao salvar tarefa"
                );

            }

            alert(
                tarefaEditandoId !== null
                    ? "Tarefa editada!"
                    : "Tarefa cadastrada!"
            );

            tarefaForm.reset();

            tarefaEditandoId = null;

            listarTarefas();

        } catch (error) {

            console.error(error);

            alert(error.message);

        }

    }
);

async function deletar(id) {

    const confirmar = confirm(
        "Deseja realmente deletar?"
    );

    if (!confirmar) {

        return;

    }

    try {

        const response = await fetch(
            `https://localhost:7214/Tarefa/${id}`,
            {

                method: 'DELETE',

                credentials: 'include'

            }
        );

        if (!response.ok) {

            throw new Error("Erro ao deletar");

        }

        alert("Tarefa deletada!");

        listarTarefas();

    } catch (error) {

        console.error(error);

    }

}

listarTarefas();