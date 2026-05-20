using Microsoft.Data.Sqlite;

namespace TodoMVC.Data;

public static class Database
{
    private const string ConnectionString = "Data Source=productos.db";

    public static void Inicializar()
    {
        using var connection = AbrirConexion();
        var sql = @"
            CREATE TABLE IF NOT EXISTS Productos (
                Id         INTEGER PRIMARY KEY AUTOINCREMENT,
                nombre     TEXT    NOT NULL,
                comprado   INTEGER NOT NULL DEFAULT 0,
                cantidad   INTEGER NOT NULL
            )";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.ExecuteNonQuery();
    }

    public static SqliteConnection AbrirConexion()
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        return connection;
    }
}
