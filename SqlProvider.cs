using System.Data.SQLite;
using System.Diagnostics;

namespace WarehouseFiller
{
    internal class SqlProvider
    {
        private SQLiteConnection _connection;
        private Stopwatch _stopwatch = new Stopwatch();

        public Stopwatch Stopwatch => _stopwatch;

        public SqlProvider(string path)
        {
            _connection = new SQLiteConnection($"DataSource={path};Mode=Write");
            _connection.Open();
        }

        public void InsertComponent(int id, string name, int type)
        {
            Stopwatch.Start();

            var query = @"
INSERT INTO Component (Id, Name, Type)
VALUES
(@id, @name, @type)";
            using var command = new SQLiteCommand(query, _connection);
            command.Parameters.Add(new SQLiteParameter("@id", id));
            command.Parameters.Add(new SQLiteParameter("@name", name));
            command.Parameters.Add(new SQLiteParameter("@type", type));
            command.ExecuteNonQuery();

            Stopwatch.Stop();
        }

        public void InsertProductComponent(int productId, int componentId, int amount)
        {
            Stopwatch.Start();

            var query = @"
INSERT INTO ProductComponent (ProductId, ComponentId, Amount)
VALUES
(@productId, @componentId, @amount)";
            using var command = new SQLiteCommand(query, _connection);
            command.Parameters.Add(new SQLiteParameter("@productId", productId));
            command.Parameters.Add(new SQLiteParameter("@componentId", componentId));
            command.Parameters.Add(new SQLiteParameter("@amount", amount));
            command.ExecuteNonQuery();

            Stopwatch.Stop();
        }
    }
}
