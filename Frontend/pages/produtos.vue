<script setup lang="ts">
import { ref, reactive, computed, onMounted } from 'vue'
import type { Produto, ProdutoRequest } from '../types/produto'
import type { Categoria } from '../types/categoria'


const produtosApiUrl = 'https://localhost:7207/api/produtos'
const categoriasApiUrl = 'https://localhost:7207/api/categorias'

const produtos = ref<Produto[]>([])
const categorias = ref<Categoria[]>([])
const erro = ref('')
const editando = ref(false)
const produtoEditandoId = ref<number | null>(null)

const form = reactive<ProdutoRequest>({
  nome: '',
  descricao: '',
  preco: 0,
  categoriaId: 0,
})

const nomeValido = computed(() => form.nome.trim().length >= 5)
const categoriaValida = computed(() => form.categoriaId > 0)
const precoValido = computed(() => form.preco > 0)

const formularioValido = computed(() =>
  nomeValido.value &&
  categoriaValida.value &&
  precoValido.value
)

const mensagem = ref('')
const tipoMensagem = ref<'sucesso' | 'erro'>('sucesso')
const exibirToast = ref(false)

function mostrarToast(texto: string, tipo: 'sucesso' | 'erro') {
  mensagem.value = texto
  tipoMensagem.value = tipo
  exibirToast.value = true

  setTimeout(() => {
    exibirToast.value = false
  }, 5000)
}

async function carregarProdutos() {
  produtos.value = await $fetch<Produto[]>(produtosApiUrl)
}

async function carregarCategorias() {
  categorias.value = await $fetch<Categoria[]>(categoriasApiUrl)
}

onMounted(async () => {
  await carregarCategorias()
  await carregarProdutos()
})

async function salvarProduto() {
    erro.value = ''
    if (!formularioValido.value) return

    try {
        if (editando.value && produtoEditandoId.value !== null) {
            await $fetch(`${produtosApiUrl}/${produtoEditandoId.value}`, {
                method: 'PUT',
                body: form
            })

            const index = produtos.value.findIndex(
            (p) => p.id === produtoEditandoId.value
            )

            const categoriaSelecionada = categorias.value.find(
            (c) => c.id === form.categoriaId
            )

            if (!categoriaSelecionada) {
            erro.value = 'Categoria selecionada não encontrada.'
            return
            }

            if (index !== -1) {
                produtos.value[index] = {
                    id: produtoEditandoId.value,
                    nome: form.nome,
                    descricao: form.descricao,
                    preco: form.preco,
                    categoriaId : form.categoriaId,
                    categoria: categoriaSelecionada
                }
            }
            mostrarToast('Produto atualizado com sucesso.', 'sucesso')
        } else {
            const novoProduto = await $fetch<Produto>(produtosApiUrl, {
                method: 'POST',
                body: form
            })
            produtos.value.push(novoProduto)
            mostrarToast('Produto cadastrado com sucesso.', 'sucesso')
        }

        limparFormulario()
    } catch (error: any) {
        mostrarToast('Erro ao cadastrar produto.', 'erro')
    }
}

function editarProduto(produto: Produto)
{
    editando.value = true
    produtoEditandoId.value = produto.id

    form.nome = produto.nome
    form.descricao = produto.descricao
    form.preco = produto.preco
    form.categoriaId = produto.categoriaId
}

async function excluirProduto(id: number) {
  erro.value = ''
  const confirmou = (globalThis as any).confirm('Deseja realmente excluir este produto?')

  if (!confirmou) return

  try {
    await $fetch(`${produtosApiUrl}/${id}`, {
      method: 'DELETE'
    })

    produtos.value = produtos.value.filter(p => p.id !== id)
    mostrarToast('Produto excluído com sucesso.', 'sucesso')
  } catch (error: any) {
    mostrarToast('Erro ao excluir produto.', 'erro')
  }
}

function limparFormulario() {
  form.nome = ''
  form.descricao = ''
  form.preco = 0
  form.categoriaId = 0
  editando.value = false
  produtoEditandoId.value = null
}

function formatarMoeda(valor: number) {
  return valor.toLocaleString('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  })
}
</script>

<template>
  <main>
    <h1>Produtos</h1>

    <NuxtLink to="/">Voltar</NuxtLink>

    <div
      v-if="exibirToast"
      class="toast"
      :class="tipoMensagem"
    >
      {{ mensagem }}
    </div>

    <section>
      <h2>{{ editando ? 'Editar Produto' : 'Novo Produto' }}</h2>

      <input
        v-model="form.nome"
        placeholder="Nome"
      />

      <input
        v-model="form.descricao"
        placeholder="Descrição"
      />

      <input
        v-model.number="form.preco"
        type="number"
        placeholder="Preço"
      />

      <select v-model.number="form.categoriaId">
        <option :value="0">
          Selecione uma categoria
        </option>

        <option
          v-for="categoria in categorias"
          :key="categoria.id"
          :value="categoria.id"
        >
          {{ categoria.nome }}
        </option>
      </select>

      <button 
        :disabled="!formularioValido"
        @click="salvarProduto">
        Salvar
      </button>
      <button
        v-if="editando"
        @click="limparFormulario"
        >
        Cancelar
      </button>
    </section>

    <table border="1" cellpadding="8">
      <thead>
        <tr>
          <th>Id</th>
          <th>Nome</th>
          <th>Descrição</th>
          <th>Preço</th>
          <th>Categoria</th>
          <th>Ações</th>
        </tr>
      </thead>

      <tbody>
        <tr v-for="produto in produtos" :key="produto.id">
          <td>{{ produto.id }}</td>
          <td>{{ produto.nome }}</td>
          <td>{{ produto.descricao }}</td>
          <td>{{ formatarMoeda(produto.preco)}}</td>
          <td>{{ produto.categoria?.nome }}</td>
          <td>
            <button @click="editarProduto(produto)">Editar</button>
            <button @click="excluirProduto(produto.id)">Excluir</button>
          </td>
        </tr>
      </tbody>
    </table>
  </main>
</template>