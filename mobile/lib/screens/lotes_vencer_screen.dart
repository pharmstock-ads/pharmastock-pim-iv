// Lotes a vencer: GET /api/alertas/lotes-a-vencer?dias=N
import 'package:flutter/material.dart';

import '../config/api_config.dart';
import '../services/servicos.dart';
import '../util/formatos.dart';
import '../widgets/lista_lotes.dart';

class LotesVencerScreen extends StatefulWidget {
  const LotesVencerScreen({super.key, required this.servicos});

  final Servicos servicos;

  @override
  State<LotesVencerScreen> createState() => _LotesVencerScreenState();
}

class _LotesVencerScreenState extends State<LotesVencerScreen> {
  static const _opcoes = [7, 15, 30, 60, 90];
  int _dias = ApiConfig.diasPadrao;

  Color _corPrazo(int dias) {
    if (dias <= 7) return Colors.red.shade700;
    if (dias <= 15) return Colors.orange.shade800;
    return Colors.amber.shade800;
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Lotes a vencer')),
      body: Column(
        children: [
          Padding(
            padding: const EdgeInsets.fromLTRB(12, 12, 12, 0),
            child: SingleChildScrollView(
              scrollDirection: Axis.horizontal,
              child: SegmentedButton<int>(
                segments: [
                  for (final d in _opcoes)
                    ButtonSegment(value: d, label: Text('$d dias')),
                ],
                selected: {_dias},
                showSelectedIcon: false,
                onSelectionChanged: (s) => setState(() => _dias = s.first),
              ),
            ),
          ),
          Expanded(
            child: ListaLotes(
              key: ValueKey(_dias), // trocar o filtro recarrega a lista
              carregar: () =>
                  widget.servicos.alertas.lotesAVencer(dias: _dias),
              produtos: widget.servicos.produtos,
              mensagemVazia:
                  'Nenhum lote vence nos próximos $_dias dias.',
              destaque: (lote) {
                final dias = lote.diasParaVencer();
                return Selo(texto: descreverPrazo(dias), cor: _corPrazo(dias));
              },
            ),
          ),
        ],
      ),
    );
  }
}
