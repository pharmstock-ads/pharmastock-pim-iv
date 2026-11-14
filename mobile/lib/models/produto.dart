// Espelha ProdutoDto da Api: { id, codigo, codBarra, nome, idCategoria, idFornecedor }.
// No app só é usado para mostrar o nome do produto nos alertas.
class Produto {
  const Produto({
    required this.id,
    required this.codigo,
    required this.nome,
    this.codBarra,
  });

  final int id;
  final int codigo;
  final String nome;
  final String? codBarra;

  factory Produto.fromJson(Map<String, dynamic> json) {
    return Produto(
      id: (json['id'] as num).toInt(),
      codigo: (json['codigo'] as num?)?.toInt() ?? 0,
      nome: json['nome'] as String? ?? '',
      codBarra: json['codBarra'] as String?,
    );
  }
}
