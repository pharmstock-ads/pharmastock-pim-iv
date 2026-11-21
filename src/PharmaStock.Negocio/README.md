# PharmaStock.Negocio

Regras de negócio. Classes `*Negocio` (ex.: `AuthNegocio`, `EntradaNegocio`, `VendaNegocio`, `AlertaNegocio`).

- FEFO na venda (escolhe lote; a UI não escolhe)
- Validações → `DominioException`
- Usa só interfaces do Repositório

Não roda sozinha. Consumida pela **Web** e pela **Api** (não pelo Flutter direto).
