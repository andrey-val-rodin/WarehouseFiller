using System.ComponentModel;
using System.Data.SQLite;
using System.Xml.Linq;

namespace WarehouseFiller
{
    internal class SqlProvider
    {
        private SQLiteConnection _connection;

        public SqlProvider(string path)
        {
            _connection = new SQLiteConnection($"DataSource={path};Mode=ReadWrite");
            _connection.Open();
        }

        public void InsertComponent(int id, string name, int type)
        {
            var query = @"
INSERT INTO Component (Id, Name, Type)
VALUES
(@id, @name, @type)";
            using var command = new SQLiteCommand(query, _connection);
            command.Parameters.Add(new SQLiteParameter("@id", id));
            command.Parameters.Add(new SQLiteParameter("@name", name));
            command.Parameters.Add(new SQLiteParameter("@type", type));
            command.ExecuteNonQuery();
        }

        public void InsertProductComponent(int productId, int componentId, int amount)
        {
            var query = @"
INSERT INTO ProductComponent (ProductId, ComponentId, Amount)
VALUES
(@productId, @componentId, @amount)";
            using var command = new SQLiteCommand(query, _connection);
            command.Parameters.Add(new SQLiteParameter("@productId", productId));
            command.Parameters.Add(new SQLiteParameter("@componentId", componentId));
            command.Parameters.Add(new SQLiteParameter("@amount", amount));
            command.ExecuteNonQuery();
        }
    }
}
