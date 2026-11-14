// Lista de lotes reaproveitada pelas duas telas de alerta.
// Trata loading, erro de rede, lista vazia e pull-to-refresh.
import 'package:flutter/material.dart';

import '../models/lote_alerta.dart';
import '../services/produto_service.dart';
import '../util/formatos.dart';

class ListaLotes extends StatefulWidget {
  const ListaLotes({
    super.key,
    required this.carregar,
    required this.produtos,
    required this.destaque,
    required this.mensagemVazia,
  });

  final Future<List<LoteAlerta>> Function() carregar;
  final ProdutoService produtos;
  final Widget Function(LoteAlerta lote) destaque;
  final String mensagemVazia;

  @override
  State<ListaLotes> createState() => _ListaLotesState();
}

class _Dados {
  const _Dados(this.lotes, this.nomes);
  final List<LoteAlerta> lotes;
  final Map<int, String> nomes;
}

class _ListaLotesState extends State<ListaLotes> {
  late Future<_Dados> _futuro;

  @override
  void initState() {
    super.initState();
    _futuro = _buscar();
  }

  Future<_Dados> _buscar({bool forcarProdutos = false}) async {
    final lotes = widget.carregar();
    final nomes = widget.produtos.mapaNomes(forcar: forcarProdutos);
    return _Dados(await lotes, await nomes);
  }

  Future<void> _recarregar() async {
    setState(() => _futuro = _buscar(forcarProdutos: true));
    try {
      await _futuro;
    } catch (_) {
      // o FutureBuilder mostra o erro
    }
  }

  @override
  Widget build(BuildContext context) {
    return RefreshIndicator(
      onRefresh: _recarregar,
      child: FutureBuilder<_Dados>(
        future: _futuro,
        builder: (context, snap) {
          if (snap.connectionState != ConnectionState.done) {
            return const Center(child: CircularProgressIndicator());
          }
          if (snap.hasError) {
            return _Mensagem(
              icone: Icons.cloud_off,
              texto: snap.error.toString(),
              acao: FilledButton.tonal(
                onPressed: _recarregar,
                child: const Text('Tentar novamente'),
              ),
            );
          }
          final dados = snap.data!;
          if (dados.lotes.isEmpty) {
            return _Mensagem(
              icone: Icons.check_circle_outline,
              texto: widget.mensagemVazia,
            );
          }
          return ListView.separated(
            physics: const AlwaysScrollableScrollPhysics(),
            padding: const EdgeInsets.all(12),
            itemCount: dados.lotes.length,
            separatorBuilder: (_, __) => const SizedBox(height: 8),
            itemBuilder: (context, i) {
              final lote = dados.lotes[i];
              final nome = dados.nomes[lote.idProduto] ?? 'Produto #${lote.idProduto}';
              return Card(
                child: ListTile(
                  leading: const CircleAvatar(child: Icon(Icons.medication)),
                  title: Text(nome),
                  subtitle: Text(
                    'Lote ${lote.numeroLote}\n'
                    'Validade ${formatarData(lote.validade)} · '
                    'Qtd ${formatarQuantidade(lote.quantidade)}',
                  ),
                  isThreeLine: true,
                  trailing: widget.destaque(lote),
                ),
              );
            },
          );
        },
      ),
    );
  }
}

class _Mensagem extends StatelessWidget {
  const _Mensagem({required this.icone, required this.texto, this.acao});

  final IconData icone;
  final String texto;
  final Widget? acao;

  @override
  Widget build(BuildContext context) {
    final tema = Theme.of(context);
    // ListView para o pull-to-refresh funcionar também nesse estado.
    return ListView(
      physics: const AlwaysScrollableScrollPhysics(),
      padding: const EdgeInsets.all(32),
      children: [
        const SizedBox(height: 48),
        Icon(icone, size: 64, color: tema.colorScheme.outline),
        const SizedBox(height: 16),
        Text(texto, textAlign: TextAlign.center, style: tema.textTheme.bodyLarge),
        if (acao != null) ...[
          const SizedBox(height: 16),
          Center(child: acao),
        ],
      ],
    );
  }
}

/// Selo colorido usado no canto direito de cada item.
class Selo extends StatelessWidget {
  const Selo({super.key, required this.texto, required this.cor});

  final String texto;
  final Color cor;

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 6),
      decoration: BoxDecoration(
        color: cor.withAlpha(35),
        borderRadius: BorderRadius.circular(16),
      ),
      child: Text(
        texto,
        style: TextStyle(color: cor, fontWeight: FontWeight.w600, fontSize: 12),
      ),
    );
  }
}
