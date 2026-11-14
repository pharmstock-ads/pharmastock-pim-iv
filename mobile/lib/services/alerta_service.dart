// Alertas A7: GET /api/alertas/lotes-a-vencer e GET /api/alertas/estoque-baixo.
import '../config/api_config.dart';
import '../models/lote_alerta.dart';
import 'api_client.dart';

class AlertaService {
  AlertaService(this._api);

  final ApiClient _api;

  /// Lotes que vencem nos próximos [dias] dias, do mais próximo ao mais distante.
  Future<List<LoteAlerta>> lotesAVencer({int dias = ApiConfig.diasPadrao}) async {
    final lotes = await _buscar('/alertas/lotes-a-vencer', {'dias': '$dias'});
    return lotes..sort((a, b) => a.validade.compareTo(b.validade));
  }

  /// Lotes com quantidade até [limite], do menor estoque para o maior.
  Future<List<LoteAlerta>> estoqueBaixo({num limite = ApiConfig.limitePadrao}) async {
    final lotes = await _buscar('/alertas/estoque-baixo', {'limite': '$limite'});
    return lotes..sort((a, b) => a.quantidade.compareTo(b.quantidade));
  }

  Future<List<LoteAlerta>> _buscar(String path, Map<String, String> query) async {
    final corpo = await _api.getJson(path, query: query);
    if (corpo == null) return [];
    if (corpo is! List) {
      throw ApiException('A API devolveu um formato inesperado em $path.');
    }
    try {
      return corpo
          .cast<Map<String, dynamic>>()
          .map(LoteAlerta.fromJson)
          .toList();
    } catch (_) {
      throw ApiException('Não foi possível ler os lotes devolvidos pela API.');
    }
  }
}
