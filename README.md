# 🏍️ Descrição do Projeto MotoApi

A **MotoApi** é uma API RESTful desenvolvida com o framework **ASP.NET Core** e o ORM **Entity Framework Core**, projetada para oferecer um sistema robusto e escalável de gerenciamento de motos, proprietários e seus respectivos registros de manutenção.

O principal objetivo é fornecer uma base sólida para aplicações que exigem controle preciso de veículos, com foco em **performance**, **segurança** e **boas práticas de desenvolvimento**.

A API foi desenhada seguindo os princípios **REST**, permitindo integração simples e eficiente com aplicações **front-end**, **mobile** ou outros serviços via HTTP. Sua arquitetura modular facilita a manutenção e evolução do sistema ao longo do tempo.

---

## 📦 Tecnologias Utilizadas

* ASP.NET Core 8
* Entity Framework Core
* Oracle (via Oracle.EntityFrameworkCore)
* AutoMapper
* Swagger (OpenAPI)

---

## 👨‍💻 Integrantes

* Ryan Fernando Lucio da Silva - 555924
* Lucas Henrique de Souza Santos - 558241
* Mariana Roberti Neri - 556284

---

## 🚀 Funcionalidades

* 📋 Cadastro, leitura, atualização e remoção de motos (CRUD)
* 👤 Gestão de proprietários
* 🛠️ Histórico de manutenção
* 🔍 Filtros e paginação nos endpoints

---

## 🛠️ Como Executar o Projeto

### Pré-requisitos

* [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
* Banco de dados Oracle configurado
* [Visual Studio 2022](https://visualstudio.microsoft.com/) ou VS Code

### Passos para execução

1. Clone o repositório:

   ```bash
   git clone https://github.com/ryanxaxxxx/Chalenge3-.Net
   ```
2. Configure a **connection string** no arquivo `appsettings.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "User Id=seu_usuario;Password=sua_senha;Data Source=seu_host"
   }
   ```
3. Aplique as **migrations** para criar o banco:

   ```bash
   dotnet ef database update
   ```
4. Rode o projeto:

   ```bash
   dotnet run
   ```

---

## 📑 Estrutura do Projeto

* **Controllers** → Exposição dos endpoints REST.
* **Services** → Regras de negócio.
* **Repositories** → Camada de acesso a dados com EF Core.
* **DTOs & AutoMapper** → Transferência e mapeamento de dados.

---

## 🌐 Documentação da API

Ao rodar o projeto, a documentação estará disponível em:

```
https://localhost:5226/swagger
```

Lá você poderá explorar todos os endpoints e realizar testes diretamente no Swagger UI.

---

## 📌 Exemplos de Endpoints

* **GET /api/motos** → Lista todas as motos (com paginação e filtros).
* **POST /api/motos** → Cadastra uma nova moto.
* **PUT /api/motos/{id}** → Atualiza os dados de uma moto.
* **DELETE /api/motos/{id}** → Remove uma moto.

---

## 🤝 Contribuindo

1. Faça um fork do projeto.
2. Crie uma branch para sua feature:

   ```bash
   git checkout -b minha-feature
   ```
3. Commit suas mudanças:

   ```bash
   git commit -m "Adiciona nova feature"
   ```
4. Faça push para a branch:

   ```bash
   git push origin minha-feature
   ```
5. Abra um Pull Request.


