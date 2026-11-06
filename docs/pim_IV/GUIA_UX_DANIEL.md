# Guia — UX / UI Design (Daniel)

**Para:** Daniel  
**De:** Anderson (Líder · Núcleo/API)  
**Atualizado:** 06/11/2026  
**Pasta de entregas:** `uiux/`  
**Status:** Web e Mobile já podem codar em paralelo; seu material **orienta** as telas e entra no **ABNT** (Etapa 6 do manual)

---

## Documentos de referência (leia nesta ordem)

| Doc | Por quê |
|-----|---------|
| [RELATORIO_ENG_SOFTWARE_AGIL.md](./RELATORIO_ENG_SOFTWARE_AGIL.md) | Negócio fictício, RF/RNF, MVP, A7 |
| [CONTRATO_TECNICO.md](./CONTRATO_TECNICO.md) | O que o sistema faz / não faz (FEFO, sem POST lotes, auth sem JWT) |
| [GUIA_WEB_VINICIUS.md](./GUIA_WEB_VINICIUS.md) | Telas que a Web precisa cobrir |
| [GUIA_MOBILE_ROBERTO.md](./GUIA_MOBILE_ROBERTO.md) | Telas do app (login + alertas A7) |
| [ORDEM_DESENVOLVIMENTO.md](./ORDEM_DESENVOLVIMENTO.md) | Seu bloco = **6C UX** |
| [`../../uiux/README.md`](../../uiux/README.md) | O que já existe do PIM III |

Manual PIM IV — **Etapa 6 (UX e UI Design)** exige evidências de: personas, fluxos, wireframes/protótipos, análise básica de usabilidade.

---

## 1. Seu papel (em uma frase)

Você desenha **como a pessoa usa** o PharmaStock (farmácia / estoque / validade).  
Vinicius e Roberto **implementam** as telas; você entrega o material de design + texto para o documento acadêmico.

Você **não** precisa programar C# nem Flutter.  
Você **não** inventa regra de negócio (ex.: “usuário escolhe o lote na venda”) — isso já está fechado no contrato.

---

## 2. Contexto do produto (para as personas)

| Tema | O que o time já fechou |
|------|------------------------|
| Sistema | PharmaStock — estoque de farmácia com **lotes** e **validade** |
| Web | Login, fornecedor, produto, **entrada**, **venda** (FEFO sem escolher lote), consultas |
| Mobile | Login + **lotes a vencer** + **estoque baixo** (A7) |
| Auth MVP | Login com funcionário; sem JWT |
| Fora do MVP | Desconto complexo, cadastro de lote isolado, PDV mobile completo |

Problema central do usuário: **não perder produto vencido** e **não faltar estoque**.

---

## 3. O que o manual pede (checklist de entrega)

| # | Entrega | Onde versionar | Status sugerido |
|---|---------|----------------|-----------------|
| 1 | **Personas** (2–3 usuários tipicos) | `uiux/personas/` (criar) ou PDF/MD no `uiux/` | ❌ → ✅ |
| 2 | **Fluxos de navegação** (Web + Mobile) | `uiux/fluxos/` ou diagramas em `docs/pim_IV/imgs/` | ❌ → ✅ |
| 3 | **Wireframes** (baixa fidelidade) das telas MVP | `uiux/prototipos/` | ⚠️ (há material do III) |
| 4 | **Protótipos** (média/alta fidelidade) atualizados ao IV | `uiux/prototipos/alta-fidelidade/` | ⚠️ |
| 5 | **Análise básica de usabilidade** (problemas + melhorias) | `uiux/usabilidade.md` (criar) | ❌ → ✅ |
| 6 | Capturas / export para o **ABNT** | Passar para Docs quando houver dono | ❌ |

Legenda mental: ❌ falta · ⚠️ parcial (reaproveitar III) · ✅ pronto para o documento.

---

## 4. Personas (o que escrever)

Crie **2 ou 3** personas curtas (meia página cada). Sugestão alinhada ao seed/domínio:

| Persona | Papel | Objetivo principal |
|---------|--------|-------------------|
| Ana | Farmacêutica / responsável | Entrar no sistema, registrar entrada, acompanhar validade |
| Carlos | Estoquista | Conferir lotes, estoque baixo, apoiar entrada |
| Mariana | Atendente | Registrar venda rápida sem escolher lote |

Para cada persona, documente:

1. Nome, cargo, contexto (farmácia de bairro / estoque)  
2. Objetivo principal  
3. Dores (ex.: “não sei o que vence primeiro”, “tela confusa na venda”)  
4. Tarefas no sistema (ligar às telas Web/Mobile)  

---

## 5. Fluxos de navegação (obrigatório)

Desenhe fluxos **simples** (caixas e setas). No mínimo:

### Web (Vinicius)

1. Login → Home  
2. Home → Fornecedores (listar / criar)  
3. Home → Produtos (listar / criar / detalhe)  
4. Home → Entrada (form com itens: produto, nº lote, validade, qtd, preço) → sucesso  
5. Home → Venda (produto, qtd, preço; **sem** lote) → sucesso ou erro “estoque insuficiente”  
6. Logout  

### Mobile (Roberto)

1. Login → Home de alertas  
2. Home → Lotes a vencer (lista / vazio)  
3. Home → Estoque baixo (lista / vazio)  
4. Logout  

Ferramentas ok: Figma, Penpot, Excalidraw, draw.io, Miro — o que o time conseguir abrir depois.

---

## 6. Telas para wireframe / protótipo (mapa MVP)

### Web

| Tela | Observação de UX |
|------|------------------|
| Login | E-mail/CPF + senha; erro claro se falhar |
| Home / menu | Acesso rápido às áreas do MVP |
| Fornecedores — lista e formulário | Campos: nome, CNPJ, telefone |
| Produtos — lista, detalhe, formulário | Código, nome, código de barras, fornecedor/categoria |
| Entrada | Pode ser 1+ itens; destacar validade e preço unitário |
| Venda | **Não** mostrar seletor de lote; mensagem se faltar estoque |
| Lotes (consulta) | Opcional: validade e quantidade visíveis |

### Mobile

| Tela | Observação de UX |
|------|------------------|
| Login | Mesma ideia da Web (credencial de funcionário) |
| Home | Dois cards grandes: “A vencer” / “Estoque baixo” |
| Lista a vencer | Lote, validade, quantidade (+ produto se possível) |
| Lista estoque baixo | Idem; empty state amigável |
| Estados | Loading, erro de rede, lista vazia |

Reaproveite o que já estiver em `uiux/` do PIM III, mas **atualize** nomes/fluxos para o IV (entrada com `preco_unitario`, venda FEFO, A7 no mobile).

---

## 7. Passo a passo recomendado

1. [ ] Ler o relatório Ágil (MVP + A7) e o contrato (o que não pode na UI)  
2. [ ] Inventariar o que já existe em `uiux/` (casos de uso / alta fidelidade)  
3. [ ] Escrever 2–3 personas  
4. [ ] Desenhar fluxos Web + Mobile  
5. [ ] Wireframes das telas da tabela da §6  
6. [ ] Alinhar 15–30 min com **Vinicius** e **Roberto** (bater tela × implementação)  
7. [ ] Subir fidelidade (cores, tipografia, componentes) — tema farmácia/estoque, sem exagero  
8. [ ] Teste rápido de usabilidade (mesmo informal): 1–2 colegas tentam “registrar uma venda” / “ver o que vence” no protótipo  
9. [ ] Anotar 3–5 problemas + melhorias em `uiux/usabilidade.md`  
10. [ ] Exportar imagens/PDF para o capítulo UX do ABNT  

---

## 8. Regras de produto que o design deve respeitar

| Pode na UI | Não pode na UI |
|------------|----------------|
| Mostrar validade e quantidade de lote em **consulta** | Pedir escolha de lote na **venda** |
| Destacar alertas A7 no mobile | Inventar cadastro de lote solto (sem entrada) |
| Mensagens claras de erro (login, estoque) | Jogar stack SQL / código técnico na cara do usuário |
| Empty states (“nenhum alerta”) | Assumir que o seed sempre tem estoque cheio |

Defaults A7 (só para você saber o que o app mostra): `dias=30`, `limite=10`.

---

## 9. Critérios de aceite (Geovanna / Docs)

- [ ] Personas documentadas e coerentes com o domínio  
- [ ] Fluxos Web + Mobile cobrindo o MVP  
- [ ] Wireframes/protótipos versionados em `uiux/`  
- [ ] Notas de usabilidade com melhorias concretas  
- [ ] Material utilizável no documento ABNT (imagens + texto curto)  
- [ ] Alinhado com o que Vinicius/Roberto vão (ou já estão) implementando  

---

## 10. Fora do seu escopo

- Programar MVC ou Flutter  
- Modelar banco / SPs  
- JWT, ML, LIBRAS (só se o grupo pedir seção curta depois)  
- Redigir o ABNT inteiro (você entrega o **capítulo/insumos** de UX)

---

## 11. Dependências / quem ajuda

| Precisa | Quem |
|---------|------|
| Prioridade de telas / RF | Geovanna (Ágil) |
| Dúvida “pode ter X na tela?” | Anderson (contrato) |
| Como a Web/Mobile vão montar a tela | Vinicius / Roberto |
| Inserir no documento final | Docs (quando houver dono) |

Branch sugerida (se versionar artefatos no Git): `feature/ux-daniel`  
Ferramenta: preferir link Figma/Penpot **e** export PNG/PDF no repo para quem não tiver conta.

---

## 12. Estrutura sugerida em `uiux/`

```
uiux/
├── README.md                 ← índice do que você entregou
├── personas/                 ← 1 arquivo ou PDF por persona
├── fluxos/                   ← web.md / mobile.md ou imagens
├── prototipos/
│   ├── baixa-fidelidade/
│   └── alta-fidelidade/
├── usabilidade.md            ← problemas + melhorias
└── casos-de-uso/             ← o que já existia no III (atualizar se fizer sentido)
```

Atualize o `uiux/README.md` com links diretos para cada entrega quando estiver pronto.
