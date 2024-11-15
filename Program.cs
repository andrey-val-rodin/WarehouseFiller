using ExcelDataReader;
using WarehouseFiller;

System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
var provider = new SqlProvider("../../../Components.db");

using (var stream = File.Open("../../../Components.xlsx", FileMode.Open, FileAccess.Read))
{
    using var reader = ExcelReaderFactory.CreateReader(stream);

    // Skip header
    reader.Read();

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
