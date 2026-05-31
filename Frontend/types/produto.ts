import type { Categoria } from "./categoria";

export interface Produto {
    id: number
    nome: string
    descricao: string
    preco: number
    categoriaId: number
    categoria: Categoria
  }
  
  export interface ProdutoRequest {
    nome: string
    descricao: string
    preco: number
    categoriaId: number
  }