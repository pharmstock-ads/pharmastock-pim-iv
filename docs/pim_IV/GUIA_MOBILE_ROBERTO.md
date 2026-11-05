# Guia — Mobile Flutter (Roberto)

**Para:** Roberto  
**De:** Anderson (Líder · Núcleo/API)  
**Atualizado:** 05/11/2026  
**Pasta do app:** `mobile/`  
**Contexto:** [RELATORIO_ENG_SOFTWARE_AGIL.md](./RELATORIO_ENG_SOFTWARE_AGIL.md) · [CONTRATO_TECNICO.md](./CONTRATO_TECNICO.md)

---

## 1. O que você entrega

App **Flutter** que consome a **API REST** (`PharmaStock.Api`) — requisito do manual (Etapa 9).

### Funcionalidade complementar fechada (A7)

> **Lotes a vencer** e **estoque baixo** — telas de consulta/alerta.

Isso é o diferencial mobile no MVP. Cadastros pesados e PDV completo ficam na **Web** (Vinicius); no app, foque em:

1. Login (mesmo usuário/funcionário da API)  
2. Dashboard simples de alertas (A7)  
3. Listagens de apoio (produtos / lotes) se der tempo  

---

## 2. Arquitetura

```
Flutter (mobile/)  ──HTTP/JSON──►  PharmaStock.Api  ──►  Negocio  ──►  SQL Server
```

| Pode | Não pode |
|------|----------|
| `http` / `dio` chamando a Api | Acessar SQL Server direto do app |
| Guardar `funcionarioId` (SharedPreferences / secure storage) | Inventar regras FIFO no app |
| Telas só de leitura para alertas | Duplicar stored procedures |

Base URL (dev): algo como `http://10.0.2.2:5xxx/api` no emulador Android, ou IP da máquina na rede para device físico. Anderson publica a URL quando a Api estiver estável + Swagger.

---

## 3. Contexto do negócio (para as telas)

- PharmaStock = farmácia com controle de **lote** e **validade**.  
- Problema real: perder produto vencido ou faltar estoque.  
- Seu app responde: “o que está perto de vencer?” e “o que está com pouco estoque?”

Parâmetros sugeridos ( Negócio/API ):

| Alerta | Parâmetro | Exemplo |
|--------|-----------|---------|
| Lotes a vencer | `dias` | validade ≤ hoje + 30 |
| Estoque baixo | `limite` | quantidade ≤ 10 (constante no MVP) |

---

## 4. O que esperar antes de começar o desenvolvimento “de verdade”

### Já pode fazer agora (preparação)

- [ ] Instalar Flutter SDK + Android Studio / VS Code + emulador (ou Chrome para web debug)  
- [ ] Na pasta `mobile/`: `flutter create .` (se ainda estiver vazia) ou abrir o projeto existente  
- [ ] Estruturar pastas, por exemplo:

```
mobile/lib/
  main.dart
  config/api_config.dart      ← baseUrl
  services/api_client.dart
  services/auth_service.dart
  services/alerta_service.dart
  models/...
  screens/login_screen.dart
  screens/home_alerts_screen.dart
  screens/lotes_vencer_screen.dart
  screens/estoque_baixo_screen.dart
```

- [ ] Tela de login + home com dois cards (“A vencer”, “Estoque baixo”) usando **dados fake**  
- [ ] Dependências: `http` ou `dio`; opcional `provider` / `riverpod`  

### Dados de demo já no repo

`database/inserts/inserts.txt` traz funcionários, produtos e lotes — mas **quantidade inicial = 0** e validades em 2027–2028. Para as telas A7 não ficarem vazias na apresentação, o time precisa rodar **entrada** (ou Geovanni acrescentar seed de movimentação / lote perto de vencer).

### Esperar a API (Anderson)

Bloqueio real de integração:

| Endpoint | Uso |
|----------|-----|
| `POST /api/auth/login` | Entrar e guardar `funcionarioId` |
| `GET /api/alertas/lotes-a-vencer?dias=30` | Lista A7 |
| `GET /api/alertas/estoque-baixo?limite=10` | Lista A7 |

Enquanto não existirem: manter mocks e a UI pronta. Quando Swagger estiver no ar, trocar o client.

Opcional depois: `GET /api/produtos`, `GET /api/lotes`.

---

## 5. Passo a passo

### Passo 1 — Projeto Flutter na pasta `mobile/`

```bash
cd mobile
flutter create . --project-name pharmastock_mobile
flutter pub add http
# ou: flutter pub add dio
```

Garantir `.gitignore` Flutter padrão (não versionar `build/`).

### Passo 2 — Config da API

`api_config.dart` com `baseUrl` fácil de trocar. Documentar no README do `mobile/` como apontar para o PC do Anderson.

### Passo 3 — Login

1. Form: CPF/e-mail + senha  
2. `POST /api/auth/login` com JSON  
3. Sucesso → salvar `funcionarioId`, `nome`, `cargo` → ir para Home  
4. Erro 401 → mensagem “credenciais inválidas”

### Passo 4 — Home de alertas (coração do A7)

Dois atalhos / cards:

- **Lotes a vencer** → lista (produto, número do lote, validade, qtd)  
- **Estoque baixo** → lista (produto, lote, quantidade)

Pull-to-refresh. Empty state: “Nenhum alerta no momento”.

### Passo 5 — Tratamento de rede

- Loading indicator  
- Timeout / Api offline → mensagem clara  
- Não crashar com JSON inesperado (parse defensivo)

### Passo 6 — Polimento para nota

- Tema simples e legível (farmácia / estoque — sem exagero)  
- Capturas de tela para o ABNT e para a Geovanna  
- README em `mobile/` com: como rodar, URL da API, emulador vs device  

---

## 6. Contrato JSON esperado (rascunho)

Anderson pode ajustar nomes — acompanhar Swagger. Formato sugerido:

**Login request**

```json
{ "cpfOuEmail": "12345678901", "senha": "demo" }
```

**Login response**

```json
{ "funcionarioId": 1, "nome": "Ana", "cargo": "Atendente" }
```

**Lote a vencer (item)**

```json
{
  "loteId": 3,
  "produtoId": 1,
  "produtoNome": "Dipirona 500mg",
  "numeroLote": "L2026A",
  "validade": "2026-12-01",
  "quantidade": 40
}
```

**Estoque baixo (item)** — mesmos campos; filtro por `quantidade`.

---

## 7. Critérios de aceite (Geovanna)

- App abre no emulador/device  
- Login contra a API (ou mock documentado na demo se API cair)  
- Tela de lotes a vencer mostra dados coerentes com o seed  
- Tela de estoque baixo idem  
- Não há acesso direto a banco no código Dart  

---

## 8. Fora do escopo do MVP mobile

- Cadastrar produto/fornecedor no app (Web)  
- Registrar venda completa no app (Web primeiro)  
- JWT, biometria, offline sync  
- Notificações push  

Se sobrar tempo após A7: atalho “última vendas” via `GET /api/vendas`.

---

## 9. Dependências

| Precisa de | Quem |
|------------|------|
| API + Swagger + URL de dev | Anderson |
| Seed + **entrada demo** (hoje `inserts.txt` deixa lote com qtd 0 e validade longe) | Geovanni |
| História A7 no board + aceite | Geovanna |
| Visual / cores (opcional) | UX |

Quando a API subir, peça ao Anderson um **exemplo curl** de cada endpoint de alerta para colar no README do mobile.
