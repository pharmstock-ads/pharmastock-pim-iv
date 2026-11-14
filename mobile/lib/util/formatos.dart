// Formatação simples pt-BR sem depender do pacote intl.

/// 2026-11-20 -> "20/11/2026"
String formatarData(DateTime data) {
  String dois(int n) => n.toString().padLeft(2, '0');
  return '${dois(data.day)}/${dois(data.month)}/${data.year}';
}

/// 15 -> "15" · 15.0 -> "15" · 2.5 -> "2,5"
String formatarQuantidade(num quantidade) {
  if (quantidade == quantidade.roundToDouble()) {
    return quantidade.toInt().toString();
  }
  var texto = quantidade.toStringAsFixed(2);
  texto = texto.replaceFirst(RegExp(r'0+$'), '');
  return texto.replaceAll('.', ',');
}

/// Texto curto sobre a validade: "vence hoje", "vence em 3 dias", "vencido há 2 dias".
String descreverPrazo(int dias) {
  if (dias == 0) return 'vence hoje';
  if (dias == 1) return 'vence amanhã';
  if (dias > 1) return 'vence em $dias dias';
  if (dias == -1) return 'vencido há 1 dia';
  return 'vencido há ${-dias} dias';
}
