// GET /api/produtos — usado só para trocar idProduto pelo nome nos alertas
// (o LoteDto da Api não traz o nome do produto).
import '../models/produto.dart';
import 'api_client.dart';

class ProdutoService {
  ProdutoService(this._api);

  final ApiClient _api;
  Map<int, String>? _cacheNomes;

  /// Mapa id -> nome. Se a chamada falhar, devolve mapa vazio:
  /// a tela continua funcionando mostrando "Produto #id".
  Future<Map<int, String>> mapaNomes({bool forcar = false}) async {
    if (!forcar && _cacheNomes != null) return _cacheNomes!;
    try {
      final corpo = await _api.getJson('/produtos');
      if (corpo is! List) return _cacheNomes ?? {};
      final produtos = corpo.cast<Map<String, dynamic>>().map(Produto.fromJson);
      _cacheNomes = {for (final p in produtos) p.id: p.nome};
      return _cacheNomes!;
    } catch (_) {
      return _cacheNomes ?? {};
    }
  }
}
