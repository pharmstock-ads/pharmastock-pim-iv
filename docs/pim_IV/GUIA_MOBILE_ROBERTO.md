# Guia — Mobile Flutter (Roberto)

**Para:** Roberto  
**De:** Anderson (Líder · Núcleo/API)  
**Atualizado:** 06/11/2026  
**Pasta do app:** `mobile/` (hoje só tem README — o app Flutter ainda precisa ser criado)  
**Status:** a **API já está pronta** — você pode integrar de verdade (Scalar + endpoints A7)

---

## Documentos de referência (leia nesta ordem)

| Doc | Por quê |
|-----|---------|
| [README.md](../../README.md) | Como o time sobe o banco e a Api |
| [CONTRATO_TECNICO.md](./CONTRATO_TECNICO.md) §7 | Contrato HTTP oficial (login + alertas) |
| [ORDEM_DESENVOLVIMENTO.md](./ORDEM_DESENVOLVIMENTO.md) | Seu bloco = **6B Mobile** |
| [GUIA_BANCO_GEOVANNI.md](./GUIA_BANCO_GEOVANNI.md) | Seed / por que alerta pode vir vazio |
| Scalar da Api | Testar as mesmas rotas antes de codar no Flutter |

Abrir a Api na máquina de quem tem o banco:

```bash
cd pharmastock-pim-iv/src
dotnet run --project PharmaStock.Api
```

Documentação interativa: **http://localhost:5152/scalar** (perfil http).

---

## 1. O que você entrega (MVP mobile)

App **Flutter** que consome a **API REST** — exigência do manual (app mobile).

### Funcionalidade complementar fechada (A7)

> Telas de **lotes a vencer** e **estoque baixo**.

| # | Tela / fluxo | Prioridade |
|---|--------------|------------|
| 1 | Login (mesmo funcionário da Api) | Obrigatório |
| 2 | Home com atalhos para os dois alertas | Obrigatório |
| 3 | Lista **lotes a vencer** (`dias`, default 30) | Obrigatório |
| 4 | Lista **estoque baixo** (`limite`, default 10) | Obrigatório |
| 5 | Listar produtos (apoio) | Se der tempo |
| 6 | Histórico de vendas | Só se sobrar tempo |

Cadastros pesados e PDV completo ficam na **Web** (Vinicius).

---

## 2. Arquitetura

```
Flutter (mobile/)  ──HTTP/JSON──►  PharmaStock.Api  ──►  Negocio  ──►  SQL Server
```

| Pode | Não pode |
|------|----------|
| `http` ou `dio` chamando a Api | Abrir SQL Server / connection string no Dart |
| Guardar `funcionarioId` (ex. `shared_preferences`) | Inventar FEFO / regra de estoque no app |
| Telas de consulta de alertas | Duplicar stored procedures |
| Tratar loading / erro de rede | Assumir que sempre há lote com estoque no seed |

O app **só fala com a Api**. Quem fala com o banco é o C#.

---

## 3. Endpoints que você usa (já existem)

Base: `http://<host>:5152/api`  
(Em Development a Api também pode estar em `https://localhost:7275` — no emulador prefira **http** na porta **5152**.)

### 3.1 Login

`POST /api/auth/login`

**Request:**

```json
{
  "cpfOuEmail": "ana.souza@pharmastock.local",
  "senha": "hash_demo_ana"
}
```

**Response 200:**

```json
{
  "funcionarioId": 1,
  "nome": "Ana Paula Souza",
  "cargo": "Farmaceutica"
}
```

**401** → credenciais inválidas (mostrar mensagem amigável).

Guarde no dispositivo: `funcionarioId`, `nome` (e `cargo` se quiser).

### 3.2 Lotes a vencer (A7)

`GET /api/alertas/lotes-a-vencer?dias=30`

- Se omitir `dias`, a Api usa **30**.  
- Retorna lista de lotes (JSON camelCase), por exemplo:

```json
[
  {
    "id": 1,
    "idProduto": 2,
    "numeroLote": "L-...",
    "validade": "2026-11-20T00:00:00",
    "quantidade": 15
  }
]
```

**Importante:** o DTO atual **não** traz o nome do produto — só `idProduto`. Na UI você pode:

1. Mostrar `idProduto` + lote + validade + qtd, **ou**  
2. Chamar `GET /api/produtos` uma vez, guardar um mapa `id → nome`, e exibir o nome.

### 3.3 Estoque baixo (A7)

`GET /api/alertas/estoque-baixo?limite=10`

- Se omitir `limite`, a Api usa **10**.  
- Mesmo formato de item de lote da seção 3.2.

### 3.4 Apoio (opcional)

| Método | Rota |
|--------|------|
| GET | `/api/produtos` |
| GET | `/api/lotes?produtoId=1` |

Não use `POST /api/lotes` — **não existe** no MVP (lote nasce na entrada, na Web/Api).

---

## 4. Por que a tela de alerta pode vir vazia

O seed (`database/inserts/inserts.txt`) cria lotes com **quantidade 0** e validades longas.

Para a demo A7 mostrar dados:

1. Alguém do time sobe a Api e faz **entrada** no Scalar (`POST /api/entradas`) com validade próxima e quantidade > 0, **ou**  
2. Geovanni acrescenta script de entrada demo no banco.

Enquanto isso: trate **lista vazia** com mensagem do tipo “Nenhum alerta no momento” (não é bug do app).

Teste os endpoints no **Scalar** antes de culpar o Flutter.

---

## 5. URL da Api: emulador vs celular físico

| Onde o app roda | `baseUrl` típica |
|-----------------|------------------|
| Emulador Android | `http://10.0.2.2:5152/api` (= “localhost” da sua máquina) |
| iOS Simulator | `http://127.0.0.1:5152/api` |
| Chrome (`flutter run -d chrome`) | `http://localhost:5152/api` |
| Celular físico (mesma Wi‑Fi) | `http://IP-DO-SEU-PC:5152/api` (ex. `http://192.168.0.15:5152/api`) |

Coloque a URL em **um único arquivo** (`lib/config/api_config.dart`) para trocar fácil.

No Android, HTTP claro (não HTTPS) pode exigir permissão em `AndroidManifest.xml` / network security config em Dev — se a chamada falhar só no Android, pesquise “cleartext HTTP Android Flutter”.

A Api precisa estar **rodando** no PC enquanto você testa o app.

---

## 6. Criar o projeto Flutter (passo a passo)

A pasta `mobile/` ainda não tem o app completo. No PC:

```bash
cd pharmastock-pim-iv/mobile
flutter create . --project-name pharmastock_mobile
flutter pub add http
# opcional: flutter pub add shared_preferences
# opcional: flutter pub add provider   # ou riverpod, se preferir
```

Garantir que `build/` está no `.gitignore`.

### Estrutura sugerida de pastas

```
mobile/lib/
  main.dart
  config/api_config.dart
  services/api_client.dart
  services/auth_service.dart
  services/alerta_service.dart
  models/login_response.dart
  models/lote_alerta.dart
  models/produto.dart
  screens/login_screen.dart
  screens/home_screen.dart
  screens/lotes_vencer_screen.dart
  screens/estoque_baixo_screen.dart
```

---

## 7. Implementação guiada

### Passo 1 — `api_config.dart`

```dart
class ApiConfig {
  // Troque conforme emulador / device (ver seção 5)
  static const String baseUrl = 'http://10.0.2.2:5152/api';
}
```

### Passo 2 — Client HTTP

Funções claras:

- `postJson(path, body)` → login  
- `getJson(path)` → alertas / produtos  

Headers: `Content-Type: application/json` e `Accept: application/json`.

Trate:

- timeout  
- status != 2xx → mensagem  
- corpo vazio / JSON inesperado → não deixar o app crashar  

### Passo 3 — Login

1. Form: CPF ou e-mail + senha  
2. `POST $baseUrl/auth/login` com o JSON da seção 3.1  
3. Sucesso → salvar `funcionarioId` e `nome` → navegar para Home  
4. 401 → “Credenciais inválidas”  

Credencial de teste: `ana.souza@pharmastock.local` / `hash_demo_ana`.

### Passo 4 — Home (coração do A7)

Dois cards / botões:

- **Lotes a vencer** → abre a lista (chama endpoint com `dias=30`)  
- **Estoque baixo** → abre a lista (chama com `limite=10`)  

Mostre o nome do funcionário logado. Botão sair (limpa storage).

### Passo 5 — Telas de lista

Para cada item mostre no mínimo:

- número do lote  
- validade (formatar data)  
- quantidade  
- produto (`idProduto` ou nome via mapa de produtos)

Pull-to-refresh.  
Empty state se a lista vier `[]`.  
Loading (CircularProgressIndicator) enquanto busca.

### Passo 6 — Polimento

- Tema simples e legível  
- Capturas de tela para ABNT / Geovanna  
- README em `mobile/` com: como rodar, qual `baseUrl` usar, que a Api precisa estar no ar  

---

## 8. Como validar sem chute

1. Suba a Api → abra Scalar → teste login e os dois GETs de alerta.  
2. Se o Scalar retornar dados e o app não, o problema é URL/emulador/parse.  
3. Se o Scalar já vier `[]`, falta **entrada/estoque** no banco — não é falha do Flutter.

Exemplo de teste manual no Scalar (mesmos paths do app).

---

## 9. Critérios de aceite

- [ ] App abre no emulador ou device  
- [ ] Login contra a Api real (Ana do seed)  
- [ ] Tela de lotes a vencer consome `GET /api/alertas/lotes-a-vencer`  
- [ ] Tela de estoque baixo consome `GET /api/alertas/estoque-baixo`  
- [ ] Lista vazia tratada com mensagem (não crash)  
- [ ] Nenhum acesso a banco no código Dart  

---

## 10. Fora do escopo do MVP mobile

- Cadastrar produto / fornecedor / entrada / venda no app (Web)  
- JWT, biometria, modo offline, push  
- Inventar campos que a Api não devolve (use o JSON real do Scalar)

Se sobrar tempo: atalho para `GET /api/vendas`.

---

## 11. Dependências / quem ajuda

| Precisa | Quem |
|---------|------|
| Api no ar + Scalar + dúvida de contrato JSON | Anderson |
| Seed / entrada demo para alertas não vazios | Geovanni (ou entrada via Scalar) |
| História A7 no board + aceite | Geovanna |
| Visual / cores | Daniel — [GUIA_UX_DANIEL.md](./GUIA_UX_DANIEL.md) |

Branch sugerida: `feature/mobile-a7-roberto`.

---

## 12. Checklist rápido “antes de pedir ajuda”

1. A Api está rodando? Scalar abre?  
2. O `baseUrl` está certo para emulador/device?  
3. O JSON do login usa `cpfOuEmail` e `senha` (nomes exatos)?  
4. Testei o mesmo GET de alerta no Scalar?  
5. Tratei lista vazia?
