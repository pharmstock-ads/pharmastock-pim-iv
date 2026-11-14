// PharmaStock Mobile — alertas A7 (lotes a vencer + estoque baixo) via PharmaStock.Api.
import 'package:flutter/material.dart';

import 'models/login_response.dart';
import 'screens/home_screen.dart';
import 'screens/login_screen.dart';
import 'services/servicos.dart';

Future<void> main() async {
  WidgetsFlutterBinding.ensureInitialized();
  final servicos = Servicos();
  final sessao = await servicos.auth.sessaoSalva();
  runApp(PharmaStockApp(servicos: servicos, sessao: sessao));
}

class PharmaStockApp extends StatelessWidget {
  const PharmaStockApp({super.key, required this.servicos, this.sessao});

  final Servicos servicos;
  final LoginResponse? sessao;

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'PharmaStock',
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(seedColor: const Color(0xFF1565C0)),
        useMaterial3: true,
        inputDecorationTheme: const InputDecorationTheme(
          border: OutlineInputBorder(),
        ),
      ),
      home: sessao == null
          ? LoginScreen(servicos: servicos)
          : HomeScreen(servicos: servicos, usuario: sessao!),
    );
  }
}
