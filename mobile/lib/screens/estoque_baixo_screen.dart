// Estoque baixo: GET /api/alertas/estoque-baixo?limite=N
import 'package:flutter/material.dart';

import '../config/api_config.dart';
import '../services/servicos.dart';
import '../util/formatos.dart';
import '../widgets/lista_lotes.dart';

class EstoqueBaixoScreen extends StatefulWidget {
  const EstoqueBaixoScreen({super.key, required this.servicos});

  final Servicos servicos;

  @override
  State<EstoqueBaixoScreen> createState() => _EstoqueBaixoScreenState();
}

class _EstoqueBaixoScreenState extends State<EstoqueBaixoScreen> {
  static const _opcoes = [5, 10, 20, 50];
  int _limite = ApiConfig.limitePadrao;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Estoque baixo')),
      body: Column(
        children: [
          Padding(
            padding: const EdgeInsets.fromLTRB(12, 12, 12, 0),
            child: SingleChildScrollView(
              scrollDirection: Axis.horizontal,
              child: SegmentedButton<int>(
                segments: [
                  for (final l in _opcoes)
                    ButtonSegment(value: l, label: Text('até $l')),
                ],
                selected: {_limite},
                showSelectedIcon: false,
                onSelectionChanged: (s) => setState(() => _limite = s.first),
              ),
            ),
          ),
          Expanded(
            child: ListaLotes(
              key: ValueKey(_limite),
              carregar: () =>
                  widget.servicos.alertas.estoqueBaixo(limite: _limite),
              produtos: widget.servicos.produtos,
              mensagemVazia:
                  'Nenhum lote com $_limite unidades ou menos.',
              destaque: (lote) => Selo(
                texto: lote.quantidade <= 0
                    ? 'zerado'
                    : '${formatarQuantidade(lote.quantidade)} un.',
                cor: lote.quantidade <= 0
                    ? Colors.red.shade700
                    : Colors.orange.shade800,
              ),
            ),
          ),
        ],
      ),
    );
  }
}
