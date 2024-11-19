using ExcelDataReader;
using WarehouseFiller;

System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
var provider = new SqlProvider("../../../Components.db");
bool useSingleInsertClauses = false;

var stopwatch = new System.Diagnostics.Stopwatch();
stopwatch.Start();

using (var stream = File.Open("../../../Components.xlsx", FileMode.Open, FileAccess.Read))
{
    using var reader = ExcelReaderFactory.CreateReader(stream);

    // Skip header
    reader.Read();

    if (useSingleInsertClauses)
    {
        while (reader.Read())
        {
            if (reader.GetValue(0) == null)
                continue;

            int componentId = (int)reader.GetDouble(0);
            var name = reader.GetString(1);
            int type = (int)reader.GetDouble(2);

            provider.InsertComponent(componentId, name, type);

            for (int i = 3, productId = 1; i <= 22; i++, productId++)
            {
                if (reader.GetValue(i) == null)
                    continue;

                int amount = (int)reader.GetDouble(i);
                provider.InsertProductComponent(productId, componentId, amount);
            }
        }
    }
    else
    {
        while (reader.Read())
        {
            if (reader.GetValue(0) == null)
                continue;

            int componentId = (int)reader.GetDouble(0);
            var name = reader.GetString(1);
            int type = (int)reader.GetDouble(2);

            provider.PrepareToInsertComponent(componentId, name, type);

            for (int i = 3, productId = 1; i <= 22; i++, productId++)
            {
                if (reader.GetValue(i) == null)
                    continue;

                int amount = (int)reader.GetDouble(i);
                provider.PrepareToInsertProductComponent(productId, componentId, amount);
            }
        }

        provider.InsertAll();
    }
}

stopwatch.Stop();
Console.WriteLine($"Sqlite time: {provider.Stopwatch.ElapsedMilliseconds} ms");
Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms");
