<script setup lang="ts">
import { ref, computed, onMounted, reactive } from 'vue';
import type { Categoria, CategoriaRequest } from '../types/categoria';

const apiUrl = 'https://localhost:7207/api/categorias'

const categorias = ref<Categoria[]>([])
const erro = ref('')
const editando = ref(false)
const categoriaEditandoId = ref<number | null>(null)

const form = reactive<CategoriaRequest>({
    nome: '',
    descricao: '',
})

const nomeValido = computed(() => form.nome.trim().length >= 5)

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

async function carregarCategorias() {
    categorias.value = await $fetch<Categoria[]>(apiUrl)
}

async function salvarCategoria() {
    erro.value = ''
    if (!nomeValido.value) return

    if (editando.value && categoriaEditandoId.value !== null) {
        await $fetch(`${apiUrl}/${categoriaEditandoId.value}`, {
            method: 'PUT',
            body: form
        })

        const index = categorias.value.findIndex(c => c.id === categoriaEditandoId.value)

        if (index !== -1) {
            categorias.value[index] = {
                id: categoriaEditandoId.value,
                nome: form.nome,
                descricao: form.descricao
            }
        }
        mostrarToast('Produto atualizado com sucesso!', 'sucesso')
    } else {
        const novaCategoria = await $fetch<Categoria>(apiUrl, {
            method: 'POST',
            body: form
        })

        categorias.value.push(novaCategoria)
        mostrarToast('Produto cadastrado com sucesso!', 'sucesso')
    }

    limparFormulario()
}

function editarCategoria(categoria: Categoria) {
  erro.value = ''
  editando.value = true
  categoriaEditandoId.value = categoria.id
  form.nome = categoria.nome
  form.descricao = categoria.descricao
}

async function excluirCategoria(id: number) {
  erro.value = ''
  const confirmou = confirm('Deseja realmente excluir esta categoria?')

  if (!confirmou) return

  try {
    await $fetch(`${apiUrl}/${id}`, {
      method: 'DELETE'
    })

    mostrarToast('Produto excluído com sucesso!', 'sucesso')
    categorias.value = categorias.value.filter(c => c.id !== id)
  } catch (error: any) {
    mostrarToast('Erro ao excluir categoria.', 'erro')
  }
}

function limparFormulario() {
  erro.value = ''
  form.nome = ''
  form.descricao = ''
  editando.value = false
  categoriaEditandoId.value = null
}

onMounted(carregarCategorias)
</script>

<template>
    <main>
    <h1>Categorias</h1>

    <NuxtLink to="/">Voltar</NuxtLink>

    <div
      v-if="exibirToast"
      class="toast"
      :class="tipoMensagem"
    >
      {{ mensagem }}
    </div>

    <section>
      <h2>{{ editando ? 'Editar Categoria' : 'Nova Categoria' }}</h2>

      <input
        v-model="form.nome"
        placeholder="Nome"
      />

      <input
        v-model="form.descricao"
        placeholder="Descrição"
      />

      <button
        :disabled="!nomeValido"
        @click="salvarCategoria"
      >
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
          <th>Ações</th>
        </tr>
      </thead>

      <tbody>
        <tr v-for="categoria in categorias" :key="categoria.id">
          <td>{{ categoria.id }}</td>
          <td>{{ categoria.nome }}</td>
          <td>{{ categoria.descricao }}</td>
          <td>
            <button @click="editarCategoria(categoria)">
              Editar
            </button>

            <button @click="excluirCategoria(categoria.id)">
              Excluir
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </main>
</template>