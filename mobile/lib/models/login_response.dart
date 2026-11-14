// Espelha LoginResponseDto da Api: { funcionarioId, nome, cargo }.
class LoginResponse {
  const LoginResponse({
    required this.funcionarioId,
    required this.nome,
    this.cargo,
  });

  final int funcionarioId;
  final String nome;
  final String? cargo;

  factory LoginResponse.fromJson(Map<String, dynamic> json) {
    return LoginResponse(
      funcionarioId: (json['funcionarioId'] as num).toInt(),
      nome: json['nome'] as String? ?? '',
      cargo: json['cargo'] as String?,
    );
  }

  Map<String, dynamic> toJson() => {
        'funcionarioId': funcionarioId,
        'nome': nome,
        'cargo': cargo,
      };
}
