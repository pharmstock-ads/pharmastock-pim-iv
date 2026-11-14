import 'package:flutter_test/flutter_test.dart';
import 'package:pharmastock_mobile/models/login_response.dart';
import 'package:pharmastock_mobile/models/lote_alerta.dart';
import 'package:pharmastock_mobile/util/formatos.dart';

void main() {
  test('LoginResponse lê o JSON da Api', () {
    final r = LoginResponse.fromJson({
      'funcionarioId': 1,
      'nome': 'Ana Paula Souza',
      'cargo': 'Farmaceutica',
    });
    expect(r.funcionarioId, 1);
    expect(r.nome, 'Ana Paula Souza');
    expect(r.cargo, 'Farmaceutica');
  });

  test('LoteAlerta lê o JSON da Api (quantidade decimal)', () {
    final l = LoteAlerta.fromJson({
      'id': 3,
      'idProduto': 2,
      'numeroLote': 'L-001',
      'validade': '2026-11-20T00:00:00',
      'quantidade': 15.0,
    });
    expect(l.idProduto, 2);
    expect(l.numeroLote, 'L-001');
    expect(l.validade, DateTime(2026, 11, 20));
    expect(formatarQuantidade(l.quantidade), '15');
  });

  test('diasParaVencer conta dias de calendário', () {
    final l = LoteAlerta(
      id: 1,
      idProduto: 1,
      numeroLote: 'X',
      validade: DateTime(2026, 11, 20),
      quantidade: 1,
    );
    expect(l.diasParaVencer(DateTime(2026, 11, 14, 23, 59)), 6);
    expect(l.diasParaVencer(DateTime(2026, 11, 20, 8)), 0);
    expect(l.diasParaVencer(DateTime(2026, 11, 22)), -2);
  });

  test('formatos pt-BR', () {
    expect(formatarData(DateTime(2026, 1, 5)), '05/01/2026');
    expect(formatarQuantidade(2.5), '2,5');
    expect(descreverPrazo(0), 'vence hoje');
    expect(descreverPrazo(5), 'vence em 5 dias');
    expect(descreverPrazo(-3), 'vencido há 3 dias');
  });
}
