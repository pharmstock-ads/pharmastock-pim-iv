// Cliente HTTP único do app. Só fala com a PharmaStock.Api (nunca com o banco).
import 'dart:async';
import 'dart:convert';

import 'package:http/http.dart' as http;

import '../config/api_config.dart';

/// Erro já traduzido para uma mensagem que pode ir direto para a tela.
class ApiException implements Exception {
  ApiException(this.mensagem, {this.statusCode});

  final String mensagem;
  final int? statusCode;

  @override
  String toString() => mensagem;
}

class ApiClient {
  ApiClient({http.Client? client, String? baseUrl})
      : _client = client ?? http.Client(),
        baseUrl = baseUrl ?? ApiConfig.baseUrl;

  final http.Client _client;
  final String baseUrl;

  static const Map<String, String> _headers = {
    'Content-Type': 'application/json',
    'Accept': 'application/json',
  };

  Future<dynamic> getJson(String path, {Map<String, String>? query}) {
    return _enviar(() => _client.get(_uri(path, query), headers: _headers));
  }

  Future<dynamic> postJson(String path, Object body) {
    return _enviar(() => _client.post(
          _uri(path),
          headers: _headers,
          body: jsonEncode(body),
        ));
  }

  Uri _uri(String path, [Map<String, String>? query]) {
    final uri = Uri.parse('$baseUrl$path');
    return query == null ? uri : uri.replace(queryParameters: query);
  }

  Future<dynamic> _enviar(Future<http.Response> Function() requisicao) async {
    late final http.Response resposta;
    try {
      resposta = await requisicao().timeout(ApiConfig.timeout);
    } on TimeoutException {
      throw ApiException(
        'A API demorou para responder. Confira se ela está rodando.',
      );
    } on http.ClientException {
      throw ApiException('Não foi possível conectar à API em $baseUrl.');
    } catch (_) {
      throw ApiException('Falha de rede ao chamar a API em $baseUrl.');
    }

    if (resposta.statusCode < 200 || resposta.statusCode >= 300) {
      throw ApiException(
        _mensagemDoCorpo(resposta) ??
            'A API respondeu com erro ${resposta.statusCode}.',
        statusCode: resposta.statusCode,
      );
    }

    if (resposta.bodyBytes.isEmpty) return null;
    try {
      return jsonDecode(utf8.decode(resposta.bodyBytes));
    } on FormatException {
      throw ApiException('A API devolveu uma resposta inesperada.');
    }
  }

  /// A Api devolve erros como { "mensagem": "..." } (ou ProblemDetails com "title").
  String? _mensagemDoCorpo(http.Response resposta) {
    try {
      final corpo = jsonDecode(utf8.decode(resposta.bodyBytes));
      if (corpo is Map) {
        final msg = corpo['mensagem'] ?? corpo['title'];
        if (msg is String && msg.trim().isNotEmpty) return msg;
      }
    } catch (_) {
      // corpo vazio ou não-JSON: usa a mensagem genérica
    }
    return null;
  }
}
