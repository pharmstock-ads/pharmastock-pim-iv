import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:pharmastock_mobile/main.dart';
import 'package:pharmastock_mobile/services/servicos.dart';

void main() {
  testWidgets('sem sessão salva, abre a tela de login', (tester) async {
    await tester.pumpWidget(PharmaStockApp(servicos: Servicos()));

    expect(find.text('PharmaStock'), findsOneWidget);
    expect(find.text('Entrar'), findsOneWidget);
    expect(find.byType(TextFormField), findsNWidgets(2));
  });

  testWidgets('valida campos vazios antes de chamar a Api', (tester) async {
    await tester.pumpWidget(PharmaStockApp(servicos: Servicos()));

    await tester.tap(find.text('Entrar'));
    await tester.pump();

    expect(find.text('Informe o CPF ou e-mail'), findsOneWidget);
    expect(find.text('Informe a senha'), findsOneWidget);
  });
}
