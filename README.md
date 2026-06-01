# Catálogo Comercial

Projeto Full Stack desenvolvido como desafio técnico utilizando ASP.NET Core 8, Entity Framework Core, SQL Server e Nuxt 3.

## Tecnologias Utilizadas

### Backend

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server LocalDB
* Swagger

### Frontend

* Nuxt 3
* Vue 3
* Composition API
* TypeScript

---

## Funcionalidades

### Categorias

* Listagem de categorias
* Cadastro de categorias
* Edição de categorias
* Exclusão de categorias
* Validação de nome (mínimo 5 caracteres)
* Bloqueio de exclusão quando existirem produtos vinculados

### Produtos

* Listagem de produtos
* Cadastro de produtos
* Edição de produtos
* Exclusão de produtos
* Seleção de categoria através de Select carregado pela API
* Exibição da categoria vinculada
* Validação de nome (mínimo 5 caracteres)

---

## Documentação do desafio

- [Modelagem de Dados](./docs/modelagem-dados.md)
- [Protótipo de Interface](./docs/prototipo-interface.md)

---

## Estrutura do Projeto

```text
Backend/
├── CatalogoComercial.Api

Frontend/
├── pages
├── types
```

---

## Configuração do Banco de Dados

Ajuste a Connection String no arquivo:

```text
Backend/appsettings.json
```

Exemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=CatalogoComercialDb;Trusted_Connection=True;"
  }
}
```

---

## Executando o Backend

Acesse a pasta da API:

```bash
cd Backend
```

Restaurar pacotes:

```bash
dotnet restore
```

Aplicar migrations:

```bash
dotnet ef database update
```

Executar a API:

```bash
dotnet run
```

Swagger:

```text
https://localhost:7207/swagger
```

---

## Executando o Frontend

Acesse a pasta do frontend:

```bash
cd Frontend
```

Instalar dependências:

```bash
npm install
```

Executar aplicação:

```bash
npm run dev
```

Frontend:

```text
http://localhost:3000
```

---

## Endpoints Principais

### Categorias

```http
GET    /api/categorias
POST   /api/categorias
PUT    /api/categorias/{id}
DELETE /api/categorias/{id}
```

### Produtos

```http
GET    /api/produtos
POST   /api/produtos
PUT    /api/produtos/{id}
DELETE /api/produtos/{id}
```

---

## Regras de Negócio

* O nome de Categorias e Produtos deve possuir no mínimo 5 caracteres.
* Não é permitido excluir categorias que possuam produtos vinculados.
* O frontend atualiza os dados de forma reativa, sem recarregar a página.
* Todas as operações são realizadas através da API REST.

---

## Autor

Fabio Fogaça
