// Ajuda a adaptar o layout a celular, tablet e desktop.
import 'dart:math' as math;

import 'package:flutter/widgets.dart';

/// Largura máxima do conteúdo em telas grandes (tablet / Windows).
const double larguraMaximaConteudo = 720;

/// A partir desta largura a Home mostra os dois cards lado a lado.
const double larguraTablet = 600;

/// Padding que centraliza o conteúdo em telas largas, mas mantém a
/// rolagem (e o puxar para atualizar) na largura toda da tela.
EdgeInsets paddingResponsivo(BuildContext context, {double vertical = 12}) {
  final largura = MediaQuery.sizeOf(context).width;
  final lateral = math.max(12.0, (largura - larguraMaximaConteudo) / 2);
  return EdgeInsets.symmetric(horizontal: lateral, vertical: vertical);
}
