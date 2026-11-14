// Home: resumo dos dois alertas A7 com atalho para cada lista.
import 'package:flutter/material.dart';

import '../models/login_response.dart';
import '../models/lote_alerta.dart';
import '../services/servicos.dart';
import '../util/layout.dart';
import 'estoque_baixo_screen.dart';
import 'login_screen.dart';
import 'lotes_vencer_screen.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key, required this.servicos, required this.usuario});

  final Servicos servicos;
  final LoginResponse usuario;

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  late Future<List<LoteAlerta>> _aVencer;
  late Future<List<LoteAlerta>> _baixo;

  @override
  void initState() {
    super.initState();
    _carregarResumo();
  }

  void _carregarResumo() {
    _aVencer = widget.servicos.alertas.lotesAVencer();
    _baixo = widget.servicos.alertas.estoqueBaixo();
  }

  Future<void> _atualizar() async {
    setState(_carregarResumo);
    try {
      await Future.wait([_aVencer, _baixo]);
    } catch (_) {
      // o erro aparece no próprio card
    }
  }

  Future<void> _sair() async {
    await widget.servicos.auth.sair();
    if (!mounted) return;
    Navigator.of(context).pushReplacement(MaterialPageRoute(
      builder: (_) => LoginScreen(servicos: widget.servicos),
    ));
  }

  Future<void> _abrir(Widget tela) async {
    await Navigator.of(context).push(MaterialPageRoute(builder: (_) => tela));
    if (mounted) _atualizar(); // volta com números atualizados
  }

  @override
  Widget build(BuildContext context) {
    final tema = Theme.of(context);
    final usuario = widget.usuario;

    final cardVencer = _CardAlerta(
      titulo: 'Lotes a vencer',
      descricao: 'Vencem nos próximos 30 dias',
      icone: Icons.event_busy,
      cor: Colors.deepOrange.shade800,
      futuro: _aVencer,
      onTap: () => _abrir(LotesVencerScreen(servicos: widget.servicos)),
    );
    final cardBaixo = _CardAlerta(
      titulo: 'Estoque baixo',
      descricao: 'Lotes com 10 unidades ou menos',
      icone: Icons.inventory_2_outlined,
      cor: Colors.red.shade800,
      futuro: _baixo,
      onTap: () => _abrir(EstoqueBaixoScreen(servicos: widget.servicos)),
    );

    // Celular: cards empilhados. Tablet/desktop: lado a lado.
    final ladoALado = MediaQuery.sizeOf(context).width >= larguraTablet;

    return Scaffold(
      appBar: AppBar(
        title: const Text('PharmaStock'),
        actions: [
          IconButton(
            tooltip: 'Sair',
            icon: const Icon(Icons.logout),
            onPressed: _sair,
          ),
        ],
      ),
      body: RefreshIndicator(
        onRefresh: _atualizar,
        child: ListView(
          physics: const AlwaysScrollableScrollPhysics(),
          padding: paddingResponsivo(context, vertical: 16),
          children: [
            Semantics(
              header: true,
              child: Text('Olá, ${usuario.nome}',
                  style: tema.textTheme.titleLarge),
            ),
            if (usuario.cargo != null && usuario.cargo!.isNotEmpty)
              Text(usuario.cargo!, style: tema.textTheme.bodyMedium),
            const SizedBox(height: 24),
            if (ladoALado)
              IntrinsicHeight(
                child: Row(
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: [
                    Expanded(child: cardVencer),
                    const SizedBox(width: 12),
                    Expanded(child: cardBaixo),
                  ],
                ),
              )
            else ...[
              cardVencer,
              const SizedBox(height: 12),
              cardBaixo,
            ],
          ],
        ),
      ),
    );
  }
}

class _CardAlerta extends StatelessWidget {
  const _CardAlerta({
    required this.titulo,
    required this.descricao,
    required this.icone,
    required this.cor,
    required this.futuro,
    required this.onTap,
  });

  final String titulo;
  final String descricao;
  final IconData icone;
  final Color cor;
  final Future<List<LoteAlerta>> futuro;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) {
    final tema = Theme.of(context);
    // MergeSemantics: o leitor de tela lê o card inteiro como um botão só,
    // ex.: "Lotes a vencer, Vencem nos próximos 30 dias, 3 alertas".
    return MergeSemantics(
      child: Card(
        clipBehavior: Clip.antiAlias,
        child: InkWell(
          onTap: onTap,
          child: Padding(
            padding: const EdgeInsets.all(16),
            child: Row(
              children: [
                ExcludeSemantics(
                  child: CircleAvatar(
                    radius: 26,
                    backgroundColor: cor.withAlpha(30),
                    child: Icon(icone, color: cor, size: 28),
                  ),
                ),
                const SizedBox(width: 16),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Text(titulo, style: tema.textTheme.titleMedium),
                      Text(descricao, style: tema.textTheme.bodySmall),
                    ],
                  ),
                ),
                FutureBuilder<List<LoteAlerta>>(
                  future: futuro,
                  builder: (context, snap) {
                    if (snap.connectionState != ConnectionState.done) {
                      return const SizedBox(
                        width: 24,
                        height: 24,
                        child: CircularProgressIndicator(
                          strokeWidth: 2.5,
                          semanticsLabel: 'Carregando',
                        ),
                      );
                    }
                    if (snap.hasError) {
                      return Tooltip(
                        message: snap.error.toString(),
                        child: Icon(Icons.cloud_off,
                            color: tema.colorScheme.error,
                            semanticLabel: 'Erro ao carregar'),
                      );
                    }
                    final total = snap.data!.length;
                    return Text(
                      '$total',
                      semanticsLabel: total == 1 ? '1 alerta' : '$total alertas',
                      style: tema.textTheme.headlineMedium?.copyWith(
                        fontWeight: FontWeight.bold,
                        color: total == 0 ? tema.colorScheme.outline : cor,
                      ),
                    );
                  },
                ),
                const ExcludeSemantics(child: Icon(Icons.chevron_right)),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
