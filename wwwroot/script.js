const apiUrlPessoas = "http://localhost:5101/pessoas";
const apiUrlCidades = "http://localhost:5101/cidades";

// Função para carregar a lista de pessoas
async function carregarPessoas() {
  const response = await fetch(apiUrlPessoas);
  const pessoas = await response.json();
  const listaPessoas = document.getElementById("listaPessoas");
  listaPessoas.innerHTML = "";
  pessoas.forEach((pessoa) => {
    const item = document.createElement("li");
    item.textContent = `Nome: ${pessoa.nome}, Idade: ${pessoa.idade}, Email: ${pessoa.email}`;
    listaPessoas.appendChild(item);
  });
}

// Função para carregar a lista de cidades
async function carregarCidades() {
  const response = await fetch(apiUrlCidades);
  const cidades = await response.json();
  const listaCidades = document.getElementById("listaCidades");
  listaCidades.innerHTML = "";
  cidades.forEach((cidade) => {
    const item = document.createElement("li");
    item.textContent = `Nome: ${cidade.nome}, Estado: ${cidade.estado}`;
    listaCidades.appendChild(item);
  });
}

// Função para adicionar uma pessoa
document.getElementById("formPessoa").addEventListener("submit", async (e) => {
  e.preventDefault();

  const pessoa = {
    nome: document.getElementById("nome").value,
    idade: document.getElementById("idade").value,
    email: document.getElementById("email").value,
    cidadeId: document.getElementById("cidadeId").value,
  };

  const response = await fetch(apiUrlPessoas, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(pessoa),
  });

  if (response.ok) {
    alert("Pessoa cadastrada com sucesso!");
    carregarPessoas(); // Atualiza a lista de pessoas
  } else {
    alert("Erro ao cadastrar pessoa.");
  }
});

// Função para adicionar uma cidade
document.getElementById("formCidade").addEventListener("submit", async (e) => {
  e.preventDefault();

  const cidade = {
    nome: document.getElementById("nomeCidade").value,
    estado: document.getElementById("estado").value,
  };

  const response = await fetch(apiUrlCidades, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(cidade),
  });

  if (response.ok) {
    alert("Cidade cadastrada com sucesso!");
    carregarCidades(); // Atualiza a lista de cidades
  } else {
    alert("Erro ao cadastrar cidade.");
  }
});

// Carregar as listas de pessoas e cidades quando a página carregar
window.onload = async () => {
  await carregarPessoas();
  await carregarCidades();
};
