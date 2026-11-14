// Agrupa os serviços do app (um ApiClient compartilhado) para passar às telas.
import 'alerta_service.dart';
import 'api_client.dart';
import 'auth_service.dart';
import 'produto_service.dart';

class Servicos {
  Servicos._(this.api)
      : auth = AuthService(api),
        alertas = AlertaService(api),
        produtos = ProdutoService(api);

  factory Servicos({ApiClient? api}) => Servicos._(api ?? ApiClient());

  final ApiClient api;
  final AuthService auth;
  final AlertaService alertas;
  final ProdutoService produtos;
}
