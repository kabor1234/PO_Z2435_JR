using System.Windows;
using Microsoft.Data.Sqlite;

namespace projektWypozyczalnia.Classes;

public static class DBUtility
{
    private static readonly string DataBaseName = "Shutterings.db";
    public static void AddShutteringSystemToDatabase(string nameOfShuttering, string manufacturer, int length, List<int> widths)
    {

        using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
        {
             connection.Open();
            
            string checkSystemQuery = "SELECT SystemID FROM SystemOfShuttering WHERE NameOfShuttering = @NameOfShuttering;";
            int systemId = -1;

            using (var checkCommand = new SqliteCommand(checkSystemQuery, connection))
            {
                checkCommand.Parameters.AddWithValue("@NameOfShuttering", nameOfShuttering);

                var result = checkCommand.ExecuteScalar();
                if (result != null)
                {
                    systemId = Convert.ToInt32(result);
                }
            }

            if (systemId == -1)
            {
                string insertSystemQuery = "INSERT INTO SystemOfShuttering (NameOfShuttering, Manufacturer, Summary) VALUES (@NameOfShuttering, @Manufacturer, @Summary);";
                using (var insertCommand = new SqliteCommand(insertSystemQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@NameOfShuttering", nameOfShuttering);
                    insertCommand.Parameters.AddWithValue("@Manufacturer", manufacturer);
                    insertCommand.Parameters.AddWithValue("@Summary", "Auto-generated entry");
                    insertCommand.ExecuteNonQuery();
                }
                
                using (var command = new SqliteCommand("SELECT last_insert_rowid();", connection))
                {
                    systemId = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            
            string insertLengthQuery = "INSERT INTO LengthCategory (SystemID, Length) VALUES (@SystemID, @Length);";
            int lengthId;

            using (var lengthCommand = new SqliteCommand(insertLengthQuery, connection))
            {
                lengthCommand.Parameters.AddWithValue("@SystemID", systemId);
                lengthCommand.Parameters.AddWithValue("@Length", length);
                lengthCommand.ExecuteNonQuery();

                using (var command = new SqliteCommand("SELECT last_insert_rowid();", connection))
                {
                    lengthId = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            
            string insertProductQuery = "INSERT INTO ShutteringProduct (LengthID, Width, AmountInStock) VALUES (@LengthID, @Width, @AmountInStock);";
            foreach (var width in widths)
            {
                using (var productCommand = new SqliteCommand(insertProductQuery, connection))
                {
                    productCommand.Parameters.AddWithValue("@LengthID", lengthId);
                    productCommand.Parameters.AddWithValue("@Width", width);
                    productCommand.Parameters.AddWithValue("@AmountInStock", 0);
                    productCommand.ExecuteNonQuery();
                }
            }
        }
    }
    public static List<AvailableSystemsOfShuttering> GetAllShutteringSystems()
    {
        var results = new List<AvailableSystemsOfShuttering>();

        using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
        {
            try
            {
                connection.Open();

                string query = @"
            SELECT 
                s.NameOfShuttering,
                s.Manufacturer,
                l.Length,
                p.Width
            FROM 
                SystemOfShuttering s
            INNER JOIN 
                LengthCategory l ON s.SystemID = l.SystemID
            INNER JOIN 
                ShutteringProduct p ON l.LengthID = p.LengthID;";

                using (var command = new SqliteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var system = new AvailableSystemsOfShuttering(
                                reader["NameOfShuttering"]?.ToString(),
                                reader["Manufacturer"]?.ToString(),
                                reader["Length"] != DBNull.Value ? Convert.ToInt32(reader["Length"]) : 0,
                                reader["Width"] != DBNull.Value ? Convert.ToInt32(reader["Width"]) : 0
                            );

                            results.Add(system);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas łączenia się z bazą danych: " + ex.Message);
            }
        }

        return results;
    }
    public static List<string> GetShutteringSystems()
    {
        var results = new List<string>();

        using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
        {
            try
            {
                connection.Open();

                string query = @"SELECT NameOfShuttering FROM SystemOfShuttering;";

                using (var command = new SqliteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(reader["NameOfShuttering"]?.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas ładowania systemów: " + ex.Message);
            }
        }

        return results;
    }
    public static List<int> GetLengthsForSystem(string systemName)
    {
        var results = new List<int>();

        using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
        {
            try
            {
                connection.Open();

                string query = @"
                SELECT DISTINCT l.Length 
                FROM LengthCategory l
                INNER JOIN SystemOfShuttering s ON l.SystemID = s.SystemID
                WHERE s.NameOfShuttering = @SystemName;";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SystemName", systemName);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(Convert.ToInt32(reader["Length"]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas ładowania długości: " + ex.Message);
            }
        }

        return results;
    }
    public static List<int> GetWidthsForLength(int length)
    {
        var results = new List<int>();

        using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
        {
            try
            {
                connection.Open();

                string query = @"
                SELECT DISTINCT p.Width 
                FROM ShutteringProduct p
                INNER JOIN LengthCategory l ON p.LengthID = l.LengthID
                WHERE l.Length = @Length;";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Length", length);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(Convert.ToInt32(reader["Width"]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas ładowania szerokości: " + ex.Message);
            }
        }

        return results;
    }
    public static Dictionary<int, string> GetEquipmentList()
        {
            var equipmentList = new Dictionary<int, string>();

            using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
            {
                try
                {
                    connection.Open();

                    string query = @"
                    SELECT EquipmentID, NameOfEquipment 
                    FROM Equipment;";

                    using (var command = new SqliteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = Convert.ToInt32(reader["EquipmentID"]);
                                string name = reader["NameOfEquipment"].ToString();
                                equipmentList.Add(id, name);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas ładowania listy osprzętu: " + ex.Message);
                }
            }
            return equipmentList;
        }
    public static List<string> GetAllEquipment()
    {
        var results = new List<string>();

        using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
        {
            try
            {
                connection.Open();

                string query = @"
            SELECT 
                NameOfEquipment
            FROM 
                Equipment;";

                using (var command = new SqliteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var equipmentName = reader["NameOfEquipment"]?.ToString();
                            
                            if (!string.IsNullOrEmpty(equipmentName))
                            {
                                results.Add(equipmentName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas łączenia się z bazą danych: " + ex.Message);
            }
        }

        return results;
    }
    public static void AddShutteringToStock(string systemName, int length, int width, int amount)
    {
        using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
        {
            try
            {
                connection.Open();

                string query = @"
                UPDATE ShutteringProduct
                SET AmountInStock = AmountInStock + @Amount
                WHERE ProductID IN (
                    SELECT p.ProductID 
                    FROM ShutteringProduct p
                    INNER JOIN LengthCategory l ON p.LengthID = l.LengthID
                    INNER JOIN SystemOfShuttering s ON l.SystemID = s.SystemID
                    WHERE s.NameOfShuttering = @SystemName AND l.Length = @Length AND p.Width = @Width
                );";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SystemName", systemName);
                    command.Parameters.AddWithValue("@Length", length);
                    command.Parameters.AddWithValue("@Width", width);
                    command.Parameters.AddWithValue("@Amount", amount);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Ilość została zaktualizowana.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas dodawania do magazynu: " + ex.Message);
            }
        }
    }
    public static void AddEquipmentToStock(int equipmentId, int amount)
    {
        using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
        {
            try
            {
                connection.Open();

                string query = @"
                                UPDATE Equipment
                                SET AmountInStock = AmountInStock + @Amount
                                WHERE EquipmentID = @EquipmentId;";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Amount", amount);
                    command.Parameters.AddWithValue("@EquipmentId", equipmentId);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Ilość została zaktualizowana.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas dodawania ilości: " + ex.Message);
            }
        }
    }
    public static void AddEquipmentToDatabase(string nameOfEquipment)
        {
            using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
            {
                try
                {
                    connection.Open();

                    string query = @"
                    INSERT INTO Equipment (NameOfEquipment, AmountInStock) 
                    VALUES (@Name, 0);";

                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", nameOfEquipment);
                        command.ExecuteNonQuery();
                        
                        MessageBox.Show("Osprzęt został dodany.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas dodawania osprzętu: " + ex.Message);
                }
            }
        }
    public static List<ShutteringStockItem> GetShutteringStockData()
        {
            var results = new List<ShutteringStockItem>();

            using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
            {
                try
                {
                    connection.Open();
                    string query = @"
                        SELECT s.NameOfShuttering AS Name, s.Manufacturer, l.Length, p.Width, p.AmountInStock AS Amount
                        FROM ShutteringProduct p
                        INNER JOIN LengthCategory l ON p.LengthID = l.LengthID
                        INNER JOIN SystemOfShuttering s ON l.SystemID = s.SystemID
                        WHERE p.AmountInStock > 0;
                    ";

                    using (var command = new SqliteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new ShutteringStockItem
                                {
                                    Manufacturer = reader["Manufacturer"].ToString(),
                                    Name = reader["Name"].ToString(),
                                    Length = Convert.ToInt32(reader["Length"]),
                                    Width = Convert.ToInt32(reader["Width"]),
                                    Amount = Convert.ToInt32(reader["Amount"])
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd podczas ładowania danych szalunków: {ex.Message}");
                }
            }
            return results;
        }
    public static List<EquipmentStockItem> GetEquipmentStockData()
        {
            var results = new List<EquipmentStockItem>();

            using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
            {
                try
                {
                    connection.Open();
                    string query = @"
                        SELECT e.NameOfEquipment AS EquipmentName, e.AmountInStock AS EquipmentAmount
                        FROM Equipment e
                        WHERE e.AmountInStock > 0;
                    ";

                    using (var command = new SqliteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new EquipmentStockItem
                                {
                                    EquipmentName = reader["EquipmentName"].ToString(),
                                    EquipmentAmount = Convert.ToInt32(reader["EquipmentAmount"])
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd podczas ładowania danych osprzętu: {ex.Message}");
                }
            }
            return results;
        }
    public static void AddPriceListRecordsForExistingProductsAndEquipment()
{
    string getProductIdsQuery = "SELECT ProductID FROM ShutteringProduct";
    string getEquipmentIdsQuery = "SELECT EquipmentID FROM Equipment";
    string checkIfProductRecordExistsQuery = "SELECT COUNT(*) FROM PriceList WHERE ProductID = @ProductID";
    string checkIfEquipmentRecordExistsQuery = "SELECT COUNT(*) FROM PriceList WHERE EquipmentID = @EquipmentID";

    List<int> productIds = new();
    List<int> equipmentIds = new();

    using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
    {
        try
        {
            connection.Open();
            
            using (var productCommand = connection.CreateCommand())
            {
                productCommand.CommandText = getProductIdsQuery;
                using (var reader = productCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productIds.Add(reader.GetInt32(0));
                    }
                }
            }
            
            using (var equipmentCommand = connection.CreateCommand())
            {
                equipmentCommand.CommandText = getEquipmentIdsQuery;
                using (var reader = equipmentCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        equipmentIds.Add(reader.GetInt32(0));
                    }
                }
            }
            
            foreach (var productId in productIds)
            {
                using (var checkProductCommand = connection.CreateCommand())
                {
                    checkProductCommand.CommandText = checkIfProductRecordExistsQuery;
                    checkProductCommand.Parameters.AddWithValue("@ProductID", productId);

                    int productCount = Convert.ToInt32(checkProductCommand.ExecuteScalar());
                    if (productCount > 0)
                    {
                        continue;  
                    }
                }
                
                string insertProductQuery = "INSERT INTO PriceList (ProductID, EquipmentID, Price) VALUES (@ProductID, NULL, NULL)";
                using (var insertProductCommand = connection.CreateCommand())
                {
                    insertProductCommand.CommandText = insertProductQuery;
                    insertProductCommand.Parameters.AddWithValue("@ProductID", productId);

                    try
                    {
                        insertProductCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Błąd wstawiania danych ProductID: {ex.Message}");
                    }
                }
            }
            
            foreach (var equipmentId in equipmentIds)
            {
                using (var checkEquipmentCommand = connection.CreateCommand())
                {
                    checkEquipmentCommand.CommandText = checkIfEquipmentRecordExistsQuery;
                    checkEquipmentCommand.Parameters.AddWithValue("@EquipmentID", equipmentId);

                    int equipmentCount = Convert.ToInt32(checkEquipmentCommand.ExecuteScalar());
                    if (equipmentCount > 0)
                    {
                        continue;
                    }
                }
                
                string insertEquipmentQuery = "INSERT INTO PriceList (ProductID, EquipmentID, Price) VALUES (NULL, @EquipmentID, NULL)";
                using (var insertEquipmentCommand = connection.CreateCommand())
                {
                    insertEquipmentCommand.CommandText = insertEquipmentQuery;
                    insertEquipmentCommand.Parameters.AddWithValue("@EquipmentID", equipmentId);

                    try
                    {
                        insertEquipmentCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Błąd wstawiania danych EquipmentID: {ex.Message}");
                    }
                }
            }
            
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Wystąpił błąd: {ex.Message}");
        }
    }
}
    public static List<PriceShuttering> GetShutteringPriceData()
        {
            AddPriceListRecordsForExistingProductsAndEquipment();
            
            var results = new List<PriceShuttering>();
            
            using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
            {
                try
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            pl.Price AS Price,
                            p.Width,
                            l.Length,
                            s.NameOfShuttering AS ShutteringName,
                            s.Manufacturer 
                        FROM
                            PriceList pl
                        INNER JOIN
                            ShutteringProduct p ON pl.ProductID = p.ProductID
                        INNER JOIN 
                            LengthCategory l ON p.LengthID = l.LengthID
                        INNER JOIN 
                            SystemOfShuttering s ON l.SystemID = s.SystemID;
                    ";

                    using (var command = new SqliteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new PriceShuttering
                                {
                                    Manufacturer = reader["Manufacturer"].ToString(),
                                    System = reader["ShutteringName"].ToString(),
                                    Length = Convert.ToInt32(reader["Length"]),
                                    Width = Convert.ToInt32(reader["Width"]),
                                    Price = reader["Price"] == DBNull.Value 
                                        ? "brak ceny" 
                                        : $"{(double)reader["Price"]:N2} zł"
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                    MessageBox.Show($"Błąd podczas ładowania danych szalunków: {ex.Message}");
                }
            }
            
            return results;
        }
    public static void UpdateShutteringPrice(string systemName, int length, int width, float newPrice)
    {
        using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
        {
            try
            {
                connection.Open();

                string query = @"
            UPDATE PriceList
            SET Price = @NewPrice
            WHERE ProductID IN (
                SELECT p.ProductID 
                FROM ShutteringProduct p
                INNER JOIN LengthCategory l ON p.LengthID = l.LengthID
                INNER JOIN SystemOfShuttering s ON l.SystemID = s.SystemID
                WHERE s.NameOfShuttering = @SystemName AND l.Length = @Length AND p.Width = @Width
            );";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SystemName", systemName);
                    command.Parameters.AddWithValue("@Length", length);
                    command.Parameters.AddWithValue("@Width", width);
                    command.Parameters.AddWithValue("@NewPrice", newPrice);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Cena została zaktualizowana.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas aktualizacji ceny: " + ex.Message);
            }
        }
    }
    public static List<PriceEquipment> GetEquipmentPriceData()
    {
        var results = new List<PriceEquipment>();

        using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
        {
            try
            {
                connection.Open();

                string query = @"
                SELECT 
                    pl.Price AS Price,
                    e.NameOfEquipment
                FROM
                    PriceList pl
                INNER JOIN
                    Equipment e ON pl.EquipmentID = e.EquipmentID
            ";

                using (var command = new SqliteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new PriceEquipment
                            {
                                NameOfEquipment = reader["NameOfEquipment"].ToString(),
                                Price = reader["Price"] == DBNull.Value 
                                    ? "brak ceny" 
                                    : $"{(double)reader["Price"]:N2} zł"
                            });

                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania danych osprzętu: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        return results;
    }
    public static void UpdateEquipmentPrice(int equipmentId, float newPrice)
    {
        using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
        {
            try
            {
                connection.Open();

                string query = @"
                                UPDATE PriceList
                                SET Price = @NewPrice
                                WHERE EquipmentID = @EquipmentId;";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NewPrice", newPrice);
                    command.Parameters.AddWithValue("@EquipmentId", equipmentId);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Cena została zaktualizowana.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas aktualizacji ceny: " + ex.Message);
            }
        }
    }
    public static void AddClientToDatabase(string nameOfCompany, int nip, string address, string postNumber, string nameOfPostEstablishment)
    {
        using (var connection = new SqliteConnection($"DataSource={DataBaseName}"))
        {
            try
            {
                connection.Open();
                
                string query = @"
                                INSERT Clients
                                VALUES (@NameOfCompany, @NIP, @Address, @PostNumber, @NameOfPostEstablishment)";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NameOfCompany", nameOfCompany);
                    command.Parameters.AddWithValue("@NIP", nip);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@PostNumber", postNumber);
                    command.Parameters.AddWithValue("@NameOfPostEstablishment", nameOfPostEstablishment);
                    
                    command.ExecuteNonQuery();
                    MessageBox.Show("Klient został dodany.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas dodawania klienta: " + ex.Message);
            }
        }
    }
}


    