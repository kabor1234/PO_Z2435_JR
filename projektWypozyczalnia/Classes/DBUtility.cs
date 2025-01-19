using System.Windows;
using Microsoft.Data.Sqlite;

namespace projektWypozyczalnia.Classes;

public static class DBUtility
{
    private static readonly string DataBaseName = "Shutterings.db";
    public static void AddShutteringSystemToDatabase(string nameOfShuttering, string manufacturer, int length, List<int> widths)
{
    try
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
                string insertSystemQuery = "INSERT INTO SystemOfShuttering (NameOfShuttering, Manufacturer) VALUES (@NameOfShuttering, @Manufacturer);";
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
            
            string checkLengthQuery = "SELECT LengthID FROM LengthCategory WHERE SystemID = @SystemID AND Length = @Length;";
            int lengthId = -1;

            using (var checkLengthCommand = new SqliteCommand(checkLengthQuery, connection))
            {
                checkLengthCommand.Parameters.AddWithValue("@SystemID", systemId);
                checkLengthCommand.Parameters.AddWithValue("@Length", length);

                var lengthResult = checkLengthCommand.ExecuteScalar();
                if (lengthResult != null)
                {
                    lengthId = Convert.ToInt32(lengthResult);
                }
            }
            
            if (lengthId == -1)
            {
                string insertLengthQuery = "INSERT INTO LengthCategory (SystemID, Length) VALUES (@SystemID, @Length);";
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
            }
            
            string checkProductQuery = "SELECT COUNT(*) FROM ShutteringProduct WHERE LengthID = @LengthID AND Width = @Width;";
            string insertProductQuery = "INSERT INTO ShutteringProduct (LengthID, Width, AmountInStock, PriceOfShuttering) VALUES (@LengthID, @Width, @AmountInStock, @PriceOfShuttering);";

            foreach (var width in widths)
            {
                try
                {
                    using (var checkProductCommand = new SqliteCommand(checkProductQuery, connection))
                    {
                        checkProductCommand.Parameters.AddWithValue("@LengthID", lengthId);
                        checkProductCommand.Parameters.AddWithValue("@Width", width);

                        var productExists = Convert.ToInt32(checkProductCommand.ExecuteScalar()) > 0;

                        if (productExists)
                        {
                            MessageBox.Show($"Produkt o szerokości {width} już istnieje.");
                        }
                        else 
                        {
                            using (var productCommand = new SqliteCommand(insertProductQuery, connection))
                            {
                                productCommand.Parameters.AddWithValue("@LengthID", lengthId);
                                productCommand.Parameters.AddWithValue("@Width", width);
                                productCommand.Parameters.AddWithValue("@AmountInStock", 0);
                                productCommand.Parameters.AddWithValue("@PriceOfShuttering", 0);
                                productCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas dodawania produktu: " + ex.Message);
                }
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Błąd: " + ex.Message);
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
                        INSERT INTO Equipment (NameOfEquipment, AmountInStock, PriceOfEquipment) 
                        VALUES (@Name, @AmountInStock, @PriceOfEquipment);";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", nameOfEquipment);
                    command.Parameters.AddWithValue("@AmountInStock", 0);
                    command.Parameters.AddWithValue("@PriceOfEquipment", 0);
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
                                reader["NameOfShuttering"].ToString(),
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
    public static List<ShutteringSystem> GetShutteringSystems()
    {
        var results = new List<ShutteringSystem>();

        using var connection = new SqliteConnection($"Data Source={DataBaseName}");
        try
        {
            connection.Open();

            string query = @"SELECT NameOfShuttering FROM SystemOfShuttering;";

            using var command = new SqliteCommand(query, connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new ShutteringSystem
                {
                    NameOfShuttering = reader["NameOfShuttering"]?.ToString()
                });
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Błąd podczas ładowania systemów: " + ex.Message);
        }

        return results;
    }
    public static List<LengthForSystem> GetLengthsForSystem(string systemName)
    {
        var results = new List<LengthForSystem>();

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
                            results.Add(new LengthForSystem
                            {
                                Length = Convert.ToInt32(reader["Length"])
                            });
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
    public static List<WidthForLength> GetWidthsForLength(int length)
    {
        var results = new List<WidthForLength>();

        using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
        {
            try
            {
                connection.Open();

                string query = @"
                                SELECT DISTINCT p.Width, p.AmountInStock, p.PriceOfShuttering 
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
                            results.Add(new WidthForLength
                            {
                                Width = Convert.ToInt32(reader["Width"]),
                                AmountInStock = Convert.ToInt32(reader["AmountInStock"]),
                                Price = (double)reader["PriceOfShuttering"]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("To tutaj");
                MessageBox.Show("Błąd podczas ładowania szerokości i ilości: " + ex.Message);
            }
        }

        return results;
    }
    public static Dictionary<int, (string Name, int AmountInStock)> GetEquipmentList()
    {
        var equipmentList = new Dictionary<int, (string Name, int AmountInStock)>();

        using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
        {
            try
            {
                connection.Open();

                string query = @"
            SELECT EquipmentID, NameOfEquipment, AmountInStock 
            FROM Equipment;";

                using (var command = new SqliteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["EquipmentID"]);
                            string name = reader["NameOfEquipment"].ToString();
                            int amountInStock = Convert.ToInt32(reader["AmountInStock"]);
                            equipmentList.Add(id, (name, amountInStock));
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
    public static List<PriceShuttering> GetShutteringPriceData()
        {
            var results = new List<PriceShuttering>();
            
            using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
            {
                try
                {
                    connection.Open();

                    string query = @"SELECT 
                                     p.PriceOfShuttering AS Price,
                                     p.Width,
                                     l.Length,
                                     s.NameOfShuttering AS ShutteringName,
                                     s.Manufacturer 
                                     FROM                                      
                                     ShutteringProduct p
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
                                    Price = (double)reader["Price"] == 0 
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
                UPDATE ShutteringProduct
                SET PriceOfShuttering = @NewPrice
                WHERE LengthID IN (
                    SELECT l.LengthID 
                    FROM LengthCategory l
                    INNER JOIN SystemOfShuttering s ON l.SystemID = s.SystemID
                    WHERE s.NameOfShuttering = @SystemName AND l.Length = @Length
                )
                AND Width = @Width;";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SystemName", systemName);
                    command.Parameters.AddWithValue("@Length", length);
                    command.Parameters.AddWithValue("@Width", width);
                    command.Parameters.AddWithValue("@NewPrice", newPrice);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cena została zaktualizowana.");
                    }
                    else
                    {
                        MessageBox.Show("Nie znaleziono produktu do aktualizacji.");
                    }
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
                                    NameOfEquipment AS Name,
                                    PriceOfEquipment AS Price
                                FROM 
                                    Equipment;";
                

                using (var command = new SqliteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new PriceEquipment
                            {
                                NameOfEquipment = reader["Name"].ToString(),
                                Price = (double)reader["Price"] == 0
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
                                UPDATE Equipment
                                SET PriceOfEquipment = @NewPrice
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
    public static WidthForLength GetWidthInfo(string systemName, int length, int width)
    {
        using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
        {
            try
            {
                connection.Open();

                string query = @"
                SELECT 
                    p.Width,
                    p.AmountInStock,
                    p.Price
                FROM 
                    SystemOfShuttering s
                INNER JOIN 
                    LengthCategory l ON s.SystemID = l.SystemID
                INNER JOIN 
                    ShutteringProduct p ON l.LengthID = p.LengthID
                WHERE 
                    s.NameOfShuttering = @SystemName 
                    AND l.Length = @Length 
                    AND p.Width = @Width;";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SystemName", systemName);
                    command.Parameters.AddWithValue("@Length", length);
                    command.Parameters.AddWithValue("@Width", width);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new WidthForLength
                            {
                                Width = reader["Width"] != DBNull.Value ? Convert.ToInt32(reader["Width"]) : 0,
                                AmountInStock = reader["AmountInStock"] != DBNull.Value ? Convert.ToInt32(reader["AmountInStock"]) : 0,
                                Price = reader["Price"] != DBNull.Value ? Convert.ToDouble(reader["Price"]) : 0.0
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas pobierania informacji o szerokości: " + ex.Message);
            }
        }

        return null;
    }
    public static bool UpdateWidthStock(string systemName, int length, int width, int newAmountInStock)
    {
        using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
        {
            try
            {
                connection.Open();

                string query = @"
                UPDATE ShutteringProduct
                SET AmountInStock = @NewAmountInStock
                WHERE Width = @Width AND LengthID IN (
                    SELECT LengthID
                    FROM LengthCategory
                    WHERE SystemID = (
                        SELECT SystemID
                        FROM SystemOfShuttering
                        WHERE NameOfShuttering = @SystemName
                    )
                );";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NewAmountInStock", newAmountInStock);
                    command.Parameters.AddWithValue("@Width", width);
                    command.Parameters.AddWithValue("@SystemName", systemName);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;  // Jeśli aktualizacja przebiegła pomyślnie, zwróci true
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas aktualizacji stanu magazynowego: " + ex.Message);
                return false;
            }
        }
    }
    public static AvailableSystemsOfShuttering FetchShutteringInfo(string systemName)
    {
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
                    ShutteringProduct p ON l.LengthID = p.LengthID
                WHERE 
                    s.NameOfShuttering = @systemName;";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@systemName", systemName);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new AvailableSystemsOfShuttering(
                                reader["NameOfShuttering"].ToString(),
                                reader["Manufacturer"]?.ToString(),
                                reader["Length"] != DBNull.Value ? Convert.ToInt32(reader["Length"]) : 0,
                                reader["Width"] != DBNull.Value ? Convert.ToInt32(reader["Width"]) : 0
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas łączenia się z bazą danych: " + ex.Message);
            }
        }

        return null;
    }
    public static void AddClientToDatabase(string nameOfCompany, int nip, string address, string postNumber, string nameOfPostEstablishment)
    {
        using (var connection = new SqliteConnection($"DataSource={DataBaseName}"))
        {
            try
            {
                connection.Open();
                
                string checkQuery = "SELECT COUNT(*) FROM Clients WHERE NIP = @NIP";
        
                using (var checkCommand = new SqliteCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@NIP", nip);
                    var exists = Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;
            
                    if (exists)
                    {
                        MessageBox.Show("Klient o tym NIPie już istnieje.");
                        return;
                    }
                }
                
                string insertQuery = @"INSERT INTO Clients (NameOfCompany, NIP, Address, PostNumber, NameOfPostEstablishment)
                                       VALUES (@NameOfCompany, @NIP, @Address, @PostNumber, @NameOfPostEstablishment)";
        
                using (var command = new SqliteCommand(insertQuery, connection))
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
            catch (Exception e)
            {
                MessageBox.Show("Błąd podczas dodawania klienta: " + e.Message);
            }
        }
    }

    public static void AddContactPerson(string name, string surname, string phoneNumber ,string email)
    {
        using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
        {
            try
            {
                connection.Open();
                string insertQuery = @"INSERT INTO ContactPersons (Name, Surname, PhoneNumber, Email)
                                      VALUES (@Name, @Surname, @PhoneNumber, @Email)";

                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Surname", surname);
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    command.Parameters.AddWithValue("@Email", email);
                    
                    command.ExecuteNonQuery();
                    MessageBox.Show("Osoba do kontaktu została dodana.");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd podczas dodawania klienta: " + e.Message);
            }
        }
        return;
    }

    public static void AddNewLending(string numberOfLend, int clientId, string startOfLendDate, string endOfLendDate, string? comments, string addressOfBuilding)
    {
        try
        {
            using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
            {
                connection.Open();

                string query = @"
                INSERT INTO Lending (NumberOfLend, IsFinished, ClientID, StartLendDate, EndLendDate, Comments, AddressOfBuilding)
                VALUES (@NumberOfLend, 0, @ClientID, @StartLendDate, @EndLendDate, @Comments, @AddressOfBuilding);";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NumberOfLend", numberOfLend);
                    command.Parameters.AddWithValue("@ClientID", clientId);
                    command.Parameters.AddWithValue("@StartLendDate", startOfLendDate);
                    command.Parameters.AddWithValue("@EndLendDate", endOfLendDate);
                    command.Parameters.AddWithValue("@Comments", comments ?? string.Empty);
                    command.Parameters.AddWithValue("@AddressOfBuilding", addressOfBuilding);

                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Wypożyczenie zostało dodane do bazy danych.");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Błąd podczas dodawania wypożyczenia: " + ex.Message);
        }
    }
    
}


    