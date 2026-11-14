// Espelha LoteDto da Api: { id, idProduto, numeroLote, validade, quantidade }.
// Usado pelos dois alertas A7 (lotes a vencer e estoque baixo).
class LoteAlerta {
  const LoteAlerta({
    required this.id,
    required this.idProduto,
    required this.numeroLote,
    required this.validade,
    required this.quantidade,
  });

  final int id;
  final int idProduto;
  final String numeroLote;
  final DateTime validade;
  final num quantidade; // decimal no C#

  factory LoteAlerta.fromJson(Map<String, dynamic> json) {
    return LoteAlerta(
      id: (json['id'] as num).toInt(),
      idProduto: (json['idProduto'] as num).toInt(),
      numeroLote: json['numeroLote'] as String? ?? '',
      validade: DateTime.parse(json['validade'] as String),
      quantidade: json['quantidade'] as num? ?? 0,
    );
  }

  /// Dias de calendário até a validade (negativo = já venceu).
  int diasParaVencer([DateTime? hoje]) {
    final h = hoje ?? DateTime.now();
    final inicio = DateTime.utc(h.year, h.month, h.day);
    final fim = DateTime.utc(validade.year, validade.month, validade.day);
    return fim.difference(inicio).inDays;
  }
}
