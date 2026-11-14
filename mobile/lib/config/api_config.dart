// Endereço da PharmaStock.Api. Único lugar para trocar a URL.
//
// Padrão: emulador Android (10.0.2.2 = "localhost" do PC).
// Para outro alvo, rode com --dart-define, sem mexer no código:
//   Windows desktop : flutter run -d windows --dart-define=API_BASE_URL=http://localhost:5152/api
//   Celular físico  : flutter run --dart-define=API_BASE_URL=http://192.168.0.15:5152/api
class ApiConfig {
  static const String baseUrl = String.fromEnvironment(
    'API_BASE_URL',
    defaultValue: 'http://10.0.2.2:5152/api',
  );

  static const Duration timeout = Duration(seconds: 10);

  // Valores padrão dos alertas A7 (os mesmos que a Api usa quando omitidos).
  static const int diasPadrao = 30;
  static const int limitePadrao = 10;
}
