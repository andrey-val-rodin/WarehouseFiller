using System.ComponentModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

namespace WarehouseFiller
{
    internal class SqlProvider
    {
        private readonly SQLiteConnection _connection;
        private readonly Stopwatch _stopwatch = new();
        private readonly StringBuilder _insertComponents = new();
        private readonly StringBuilder _insertProductComponents = new();

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

        public void PrepareToInsertComponent(int id, string name, int type)
        {
            if (_insertComponents.Length == 0)
            {
                _insertComponents.AppendLine("INSERT INTO Component (Id, Name, Type) VALUES");
                _insertComponents.AppendLine($"({id}, '{name}', {type})");
            }
            else
            {
                _insertComponents.Append(',');
                _insertComponents.AppendLine($"({id}, '{name}', {type})");
            }
        }

        public void PrepareToInsertProductComponent(int productId, int componentId, int amount)
        {
            if (_insertProductComponents.Length == 0)
            {
                _insertProductComponents.AppendLine("INSERT INTO ProductComponent (ProductId, ComponentId, Amount) VALUES");
                _insertProductComponents.AppendLine($"({productId}, {componentId}, {amount})");
            }
            else
            {
                _insertProductComponents.Append(',');
                _insertProductComponents.AppendLine($"({productId}, {componentId}, {amount})");
            }
        }

        public void InsertAll()
        {
            if (_insertComponents.Length == 0 || _insertProductComponents.Length == 0)
                throw new InvalidOperationException("You mast call PrepareToInsertComponent and PrepareToInsertProductComponent before");

            Stopwatch.Start();

            var query = _insertComponents.ToString();
            using var command1 = new SQLiteCommand(query, _connection);
            command1.ExecuteNonQuery();

            query = _insertProductComponents.ToString();
            using var command2 = new SQLiteCommand(query, _connection);
            command2.ExecuteNonQuery();

            Stopwatch.Stop();
        }
    }
}
