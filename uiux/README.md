# uiux — Interface e Experiência do Usuário

Pasta de artefatos de UI/UX do sistema **PharmaStock**. Reúne toda a parte visual do projeto: documentação de design, diagramas de casos de uso e protótipos de alta fidelidade desenvolvidos no Figma.

---

## Estrutura

```
uiux/
├── Documentacao/           ← Documento PDF de UI/UX
├── casos-de-uso/           ← Diagramas de casos de uso (PNG)
└── prototipo-final/        ← Protótipos de alta fidelidade (PNG)
```

---

## Documentacao/

Contém o documento `uiux.pim.pdf` com a fundamentação teórica e decisões de design do projeto:

- Identidade visual (paleta Blue-Pharma: azul escuro, azul comercial, branco, cinza)
- Princípios de UI aplicados (organização visual, padronização de telas)
- Princípios de UX aplicados (fluxo de trabalho do atendente, linguagem simples, avisos visuais)
- Wireframes de baixa fidelidade
- Protótipos de alta fidelidade

---

## casos-de-uso/

Diagramas modelados no **Lucidchart** mapeando as interações entre os atores e o sistema.

| Arquivo | Fluxo |
|---|---|
| `diagrama caso de uso geral.png` | Visão geral de todas as funcionalidades |
| `caso de uso login e cadastro.png` | Autenticação e criação de conta |
| `caso de uso dashboard.png` | Painel principal com indicadores |
| `caso de uso de controle de produtos.png` | CRUD de produtos |
| `caso de uso de controle de fornecedores.png` | CRUD de fornecedores |
| `caso de uso de controle de funcionarios.png` | Gestão de funcionários |
| `caso de uso movimentação de estoque.png` | Entradas e saídas de estoque |

**Ator principal:** Funcionário (Administrador / Atendente)

---

## prototipo-final/

Protótipos de alta fidelidade com a identidade visual Blue-Pharma aplicada. Desenvolvidos no **Figma** antes da implementação, servem como referência visual para o WinForms e o frontend HTML.

| Arquivo | Tela |
|---|---|
| `tela de login.png` | Autenticação com campos visuais |
| `tela de cadastro de login.png` | Criação de conta |
| `tela dashboard.png` | Painel com indicadores de estoque |
| `tela cadastro de produto.png` | Formulário de cadastro de produto |
| `produto - listagem.png` | Inventário em formato de tabela |
| `tela cadastro de fornecedor.png` | Formulário de cadastro de fornecedor |
| `tela funcionarios.png` | Listagem de funcionários |
| `cadastro de funcionario.png` | Formulário de cadastro de funcionário |
| `tela movimentaçao de estoque.png` | Entrada e saída de mercadorias |
