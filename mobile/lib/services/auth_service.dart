// Login (POST /api/auth/login) e sessão local (funcionarioId + nome).
import 'dart:convert';

import 'package:shared_preferences/shared_preferences.dart';

import '../models/login_response.dart';
import 'api_client.dart';

class AuthService {
  AuthService(this._api);

  final ApiClient _api;
  static const _chaveSessao = 'sessao_funcionario';

  Future<LoginResponse> login(String cpfOuEmail, String senha) async {
    late final dynamic corpo;
    try {
      corpo = await _api.postJson('/auth/login', {
        'cpfOuEmail': cpfOuEmail.trim(),
        'senha': senha,
      });
    } on ApiException catch (e) {
      if (e.statusCode == 401) {
        throw ApiException('CPF/e-mail ou senha inválidos.', statusCode: 401);
      }
      rethrow;
    }

    if (corpo is! Map<String, dynamic>) {
      throw ApiException('Resposta de login inesperada da API.');
    }
    final usuario = LoginResponse.fromJson(corpo);
    final prefs = await SharedPreferences.getInstance();
    await prefs.setString(_chaveSessao, jsonEncode(usuario.toJson()));
    return usuario;
  }

  /// Sessão salva no aparelho, ou null se ninguém logou.
  Future<LoginResponse?> sessaoSalva() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      final texto = prefs.getString(_chaveSessao);
      if (texto == null) return null;
      return LoginResponse.fromJson(jsonDecode(texto) as Map<String, dynamic>);
    } catch (_) {
      return null; // sessão corrompida = pede login de novo
    }
  }

  Future<void> sair() async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.remove(_chaveSessao);
  }
}
