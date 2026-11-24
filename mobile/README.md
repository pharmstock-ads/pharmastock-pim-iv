# mobile — App Flutter (PIM IV)

Aplicativo Flutter que consome a **PharmaStock.Api** (ASP.NET Core Web API).
Funcionalidade complementar ao sistema web (**A7**): **lotes a vencer** e **estoque baixo**.

O app só fala com a Api via HTTP/JSON — nenhum acesso a banco no código Dart.

## Telas

| Tela | O que faz | Endpoint |
|------|-----------|----------|
| Login | CPF/e-mail + senha, guarda `funcionarioId` e `nome` no aparelho | `POST /api/auth/login` |
| Home | Saudação + cards com a contagem de cada alerta (puxar para atualizar) | os dois GET de alerta |
| Lotes a vencer | Filtro 7/15/30/60/90 dias, ordenado pela validade, selo "vence em N dias" | `GET /api/alertas/lotes-a-vencer?dias=N` |
| Estoque baixo | Filtro até 5/10/20/50 unidades, ordenado pela quantidade | `GET /api/alertas/estoque-baixo?limite=N` |
| (apoio) | Troca `idProduto` pelo nome do produto nas listas | `GET /api/produtos` |

Todas as listas tratam **loading**, **erro de rede** (com "Tentar novamente"), **lista vazia** e **pull-to-refresh**.

**Diferentes dispositivos:** no celular os cards ficam empilhados; em tablet/desktop (≥ 600 px) ficam lado a lado e o conteúdo é centralizado (máx. 720 px) — ver `lib/util/layout.dart`.

**Acessibilidade:** rótulos para leitor de tela (TalkBack / Narrador) nos cards, contagens, itens das listas e indicadores de carregamento; cores dos selos com contraste reforçado; o texto respeita o tamanho de fonte do sistema.

## Estrutura

```
lib/
  main.dart                      app + decide login/home pela sessão salva
  config/api_config.dart         baseUrl da Api (único lugar para trocar)
  services/api_client.dart       GET/POST JSON, timeout, erros -> ApiException
  services/auth_service.dart     login + sessão (shared_preferences)
  services/alerta_service.dart   alertas A7
  services/produto_service.dart  mapa id -> nome do produto
  services/servicos.dart         agrupa os serviços
  models/                        LoginResponse, LoteAlerta, Produto (espelham os DTOs da Api)
  screens/                       login, home, lotes a vencer, estoque baixo
  widgets/lista_lotes.dart       lista reaproveitada pelas duas telas de alerta
  util/formatos.dart             data/quantidade em pt-BR
test/                            testes de modelos e da tela de login
```

## Como rodar

Pré-requisitos: Flutter SDK (`flutter doctor`), e a **Api rodando** (ver README da raiz).

```bash
cd src
dotnet run --project PharmaStock.Api      # http://localhost:5152/scalar
```

Primeira vez (gera `android/`, `windows/` etc. e libera HTTP no Android):

```bash
cd mobile
powershell -ExecutionPolicy Bypass -File .\configurar.ps1
```

Rodar:

| Onde | Comando |
|------|---------|
| Emulador Android | `flutter run` (padrão `http://10.0.2.2:5152/api`) |
| Windows desktop | `flutter run -d windows --dart-define=API_BASE_URL=http://localhost:5152/api` |
| Celular físico (mesma Wi-Fi) | `flutter run --dart-define=API_BASE_URL=http://IP-DO-PC:5152/api` |

> **Chrome (`-d chrome`) não funciona hoje:** a Api não tem CORS habilitado, então o navegador bloqueia as chamadas. Use emulador ou Windows.
> Para celular físico, a Api precisa escutar na rede: `dotnet run --project PharmaStock.Api --urls http://0.0.0.0:5152`.

Testes:

```bash
flutter test
```

Login de teste (seed): `ana.souza@pharmastock.local` / `@Ana123`.

## Alertas vazios?

O seed cria lotes com quantidade 0 e validade longa. Se o **Scalar** também devolver `[]`, falta estoque no banco — não é bug do app.
Faça um `POST /api/entradas` no Scalar com validade próxima e quantidade > 0 (ou rode o script de entrada demo do banco, quando existir).

## Checklist

- [x] Código de login, lotes a vencer, estoque baixo, vazio e erro de rede
- [x] Nenhum acesso a banco no Dart
- [ ] Login e listas provados contra a Api real
- [ ] Capturas de tela para o documento ABNT
