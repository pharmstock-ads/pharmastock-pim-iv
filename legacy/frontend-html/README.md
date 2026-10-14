# PharmStock — Documentação

Front-end estático do sistema de gestão de estoque farmacêutico.  
Projeto acadêmico (ADS) com foco em **HTML semântico**, **acessibilidade** e **layout responsivo**.

**Atalhos:** [Abrir o sistema](index.html) · [Login](login/index.html) · [Produtos](produtos-lista/index.html)

---

## Visão geral

O repositório é um protótipo de interface **sem backend**: as telas simulam cadastros, listagens e navegação entre módulos. Toda a aparência visual vem de um único arquivo CSS na raiz (`style.css`).

**Tecnologias:** HTML5 · CSS3 · Acessibilidade · Layout responsivo

### Como o sistema se comporta

- **Entrada:** o usuário abre `login/index.html`, preenche e-mail e senha e clica em Entrar → redireciona para o dashboard (`index.html`).
- **Navegação interna:** o menu lateral leva a cada módulo (produtos, fornecedor, funcionários, movimentação).
- **Sair:** o link “Sair” volta para a tela de login.
- **Formulários:** ao salvar, exibem alerta ou redirecionam — não há persistência em banco de dados.

---

## Estrutura de pastas

Na raiz ficam apenas o dashboard e o CSS global. Cada tela tem sua própria pasta com `index.html`.

```
FRONTWORKSPACE/
├── index.html              ← Dashboard (página inicial do app)
├── style.css               ← Estilos globais compartilhados
├── README.md               ← Esta documentação
├── login/index.html        ← Autenticação
├── criar-conta/index.html  ← Cadastro público
├── produtos-lista/index.html
├── produtos-cadastro/index.html
├── fornecedor-cadastro/index.html
├── funcionarios-lista/index.html
├── funcionario-cadastro/index.html
├── movimentacao/index.html
└── docs/                   ← Materiais extras (PDF, etc.)
```

---

## HTML semântico

As páginas internas usam landmarks e hierarquia adequada para leitores de tela e SEO básico:

| Elemento | Uso |
|----------|-----|
| `<aside>` | Barra lateral com identidade do sistema |
| `<nav aria-label="Menu principal">` | Links do menu |
| `<main id="conteudo-principal">` | Conteúdo principal da página |
| `<header>` | Título da tela (`<h1>`) e contexto |
| `<section>` | Blocos de formulário, tabelas e filtros |
| `<table>` | Com `<caption class="sr-only">` e `<th scope="col">` |
| `<form>` | Com `<label for="...">` associado aos campos |

O item ativo do menu usa `aria-current="page"` para indicar a página atual.

---

## Acessibilidade

- **Skip link** — “Ir para o conteúdo” no início de cada página (visível ao focar com Tab).
- **Labels** — campos de busca e formulários com `label` visível ou `.sr-only` (somente leitor de tela).
- **Foco visível** — `:focus-visible` em links, botões e inputs.
- **Paginação** — itens desabilitados usam `aria-disabled="true"` em vez de `href="#"`.
- **Ícones decorativos** — emojis do menu podem usar `aria-hidden="true"` onde aplicável.
- **Redução de movimento** — `@media (prefers-reduced-motion: reduce)` reduz animações.

**Limitação atual:** login não valida credenciais; qualquer e-mail/senha preenchidos permitem entrar (protótipo).

---

## Layout responsivo

O `style.css` adapta o layout em três faixas principais:

| Largura máxima | Comportamento |
|----------------|---------------|
| 900px | Cards do dashboard em 2 colunas; login em coluna; painéis empilhados |
| 768px | Sidebar no topo; menu em linha; filtros e botões em coluna; formulários em 1 coluna |
| 480px | Métricas em 1 coluna; tabelas com fonte menor |

Tabelas largas usam `overflow-x: auto` no container para scroll horizontal em telas pequenas.

---

## CSS e organização visual

- **Variáveis CSS** (`:root`) — cores primárias, status, bordas e raios.
- **Paleta sóbria** — azul escuro no menu (`#1f3b63` → `#162b49`).
- **Sem estilos inline** — espaçamentos e alinhamentos via classes utilitárias (`.mb-20`, `.text-center`, etc.).
- **Componentes** — sidebar, cards, tabelas, formulários, botões e telas de autenticação compartilham o mesmo design system.

---

## JavaScript (uso mínimo)

O projeto prioriza HTML/CSS. O JS aparece apenas onde a interação exige:

- Mostrar/ocultar senha no login
- Atualizar texto de toggles (ex.: “Sim/Não”, “Ativo/Inativo”)
- Redirecionamento simulado após submit de alguns formulários

**Busca de produtos** (filtrar tabela por nome/ID) ainda não implementada — requer JavaScript ou backend.

---

## Mapa de telas

| Pasta / arquivo | Função |
|-----------------|--------|
| `login/` | Acesso ao sistema |
| `criar-conta/` | Cadastro de novo usuário (simulado) |
| `index.html` | Dashboard com métricas e resumos |
| `produtos-lista/` | Listagem e busca de produtos |
| `produtos-cadastro/` | Cadastro/edição de produto |
| `fornecedor-cadastro/` | Cadastro de fornecedor |
| `funcionarios-lista/` | Listagem de funcionários |
| `funcionario-cadastro/` | Cadastro de funcionário |
| `movimentacao/` | Entrada/saída de estoque |

---

*PharmStock — Projeto acadêmico ADS · Front-end estático · Repositório FRONTWORKSPACE*
