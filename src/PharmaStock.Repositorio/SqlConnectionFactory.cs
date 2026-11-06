// Abre conexoes SQL Server a partir da connection string Padrao.
using Microsoft.Data.SqlClient;

namespace PharmaStock.Repositorio;

public interface ISqlConnectionFactory
{
    SqlConnection Create();
}

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString
            ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public SqlConnection Create() => new(_connectionString);
}
