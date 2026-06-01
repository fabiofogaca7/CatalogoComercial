Categoria
- Id: int, PK
- Nome: string, obrigatório, mínimo 5 caracteres
- Descricao: string, opcional

Produto
- Id: int, PK
- Nome: string, obrigatório, mínimo 5 caracteres
- Descricao: string, opcional
- Preco: decimal
- CategoriaId: int, FK

Relacionamento:
Uma Categoria possui muitos Produtos.
Um Produto pertence a uma Categoria.

Categoria 1 ─── N Produto