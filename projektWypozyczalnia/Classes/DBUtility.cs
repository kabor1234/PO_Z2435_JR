using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
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
                catch (Exception e)
                {
                    MessageBox.Show("Błąd podczas dodawania produktu: " + e.Message);
                }
            }
        }
    }
    catch (Exception e)
    {
        MessageBox.Show("Błąd: " + e.Message);
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
            catch (Exception e)
            {
                MessageBox.Show("Błąd podczas dodawania osprzętu: " + e.Message);
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
            catch (Exception e)
            {
                MessageBox.Show("Błąd podczas łączenia się z bazą danych: " + e.Message);
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
        catch (Exception e)
        {
            MessageBox.Show("Błąd podczas ładowania systemów: " + e.Message);
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
            catch (Exception e)
            {
                MessageBox.Show("Błąd podczas ładowania długości: " + e.Message);
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
                SELECT DISTINCT p.ProductID, p.Width, p.AmountInStock, p.PriceOfShuttering 
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
                                ProductID = Convert.ToInt32(reader["ProductID"]),
                                Width = Convert.ToInt32(reader["Width"]),
                                AmountInStock = Convert.ToInt32(reader["AmountInStock"]),
                                Price = (double)reader["PriceOfShuttering"]
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd podczas ładowania szerokości i ilości: " + e.Message);
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
            catch (Exception e)
            {
                MessageBox.Show("Błąd podczas ładowania listy osprzętu: " + e.Message);
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
            catch (Exception e)
            {
                MessageBox.Show("Błąd podczas łączenia się z bazą danych: " + e.Message);
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
            catch (Exception e)
            {
                MessageBox.Show("Błąd podczas dodawania do magazynu: " + e.Message);
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
            catch (Exception e)
            {
                MessageBox.Show("Błąd podczas dodawania ilości: " + e.Message);
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
                catch (Exception e)
                {
                    MessageBox.Show($"Błąd podczas ładowania danych szalunków: {e.Message}");
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
                catch (Exception e)
                {
                    MessageBox.Show($"Błąd podczas ładowania danych osprzętu: {e.Message}");
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
                catch (Exception e)
                {
                    
                    MessageBox.Show($"Błąd podczas ładowania danych szalunków: {e.Message}");
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
            catch (Exception e)
            {
                MessageBox.Show("Błąd podczas aktualizacji ceny: " + e.Message);
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
            catch (Exception e)
            {
                MessageBox.Show($"Błąd podczas ładowania danych osprzętu: {e.Message}");
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
            catch (Exception e)
            {
                MessageBox.Show("Błąd podczas aktualizacji ceny: " + e.Message);
            }
        }
    }
    public static void AddClientToDatabase(string nameOfCompany, string nip, string address, string postNumber, string nameOfPostEstablishment)
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
                    var eists = Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;
            
                    if (eists)
                    {
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
    public static Client GetClientByNumber(string numberOfCompany)
    {
        Client client = null;

        using var connection = new SqliteConnection($"Data Source={DataBaseName}");
        try
        {
            connection.Open();

            string query = @"SELECT ClientID, NameOfCompany, NIP, Address, PostNumber, NameOfPostEstablishment 
                         FROM Clients WHERE NIP = @NIP;";

            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@NIP", numberOfCompany);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                
                client = new Client(
                    reader.GetInt32(0),  
                    reader.GetString(1),  
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5)
                );
            }
        }
        catch (Exception e)
        {
            MessageBox.Show("Błąd podczas pobierania klienta: " + e.Message);
        }

        return client;
    }
    public static List<Client> GetAllClients()
    {
        var results = new List<Client>();

        using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
        {
            try
            {
                connection.Open();

                string query = @"
            SELECT 
                ClientID, 
                NameOfCompany, 
                NIP, 
                Address, 
                PostNumber, 
                NameOfPostEstablishment 
            FROM 
                Clients;";

                using (var command = new SqliteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                            var client = new Client(
                                reader.GetInt32(reader.GetOrdinal("ClientID")),
                                reader["NameOfCompany"]?.ToString(),
                                reader["NIP"]?.ToString(),
                                reader["Address"]?.ToString(),
                                reader["PostNumber"]?.ToString(),
                                reader["NameOfPostEstablishment"]?.ToString()
                            );

                            
                            results.Add(client);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd podczas łączenia się z bazą danych: " + e.Message);
            }
        }

        return results;
    }
    public static void AddNewLending(int clientId, string startOfLendDate, string endOfLendDate, string addressOfBuilding, string? comment)
    {
        try
        {
            using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
            {
                connection.Open();
                
                string currentMonth = DateTime.Now.ToString("MM");
                string currentYear = DateTime.Now.ToString("yyyy");
                string getMaxNumberQuery = @"
                    SELECT NumberOfLend
                    FROM Lending
                    WHERE substr(NumberOfLend, instr(NumberOfLend, '/') + 1) = @MonthYear
                    ORDER BY CAST(substr(NumberOfLend, 1, instr(NumberOfLend, '/') - 1) AS INTEGER) DESC
                    LIMIT 1;";

                string monthYear = $"{currentMonth}/{currentYear}";
                string newNumber = "001";

                using (var command = new SqliteCommand(getMaxNumberQuery, connection))
                {
                    command.Parameters.AddWithValue("@MonthYear", monthYear);

                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        string lastNumber = result.ToString()?.Split('/')[0];
                        if (int.TryParse(lastNumber, out int lastNumberValue))
                        {
                            newNumber = (lastNumberValue + 1).ToString("D3");
                        }
                    }
                }
                
                string numberOfLend = $"{newNumber}/{monthYear}";
                
                string insertQuery = @"
                    INSERT INTO Lending (NumberOfLend, ClientID, StartLendDate, EndLendDate, Comment, AddressOfBuilding)
                    VALUES (@NumberOfLend, @ClientID, @StartLendDate, @EndLendDate, @Comments, @AddressOfBuilding);";

                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@NumberOfLend", numberOfLend);
                    command.Parameters.AddWithValue("@ClientID", clientId);
                    command.Parameters.AddWithValue("@StartLendDate", startOfLendDate);
                    command.Parameters.AddWithValue("@EndLendDate", endOfLendDate);
                    command.Parameters.AddWithValue("@Comments", comment ?? string.Empty);
                    command.Parameters.AddWithValue("@AddressOfBuilding", addressOfBuilding);

                    command.ExecuteNonQuery();
                }

                MessageBox.Show($"Wypożyczenie o numerze {numberOfLend} zostało dodane do bazy danych.");
            }
        }
        catch (Exception e)
        {
            MessageBox.Show("Błąd podczas dodawania wypożyczenia: " + e.Message);
        }
    }
    public static List<RentalItems> GetAllLendings()
{
    var results = new List<RentalItems>();

    using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
    {
        try
        {
            connection.Open();

            string query = @"
            SELECT 
                Lending.LendID, 
                Lending.NumberOfLend, 
                Lending.ClientID, 
                Clients.NameOfCompany, 
                Lending.StartLendDate, 
                Lending.EndLendDate, 
                Lending.AddressOfBuilding
            FROM 
                Lending
            INNER JOIN 
                Clients ON Lending.ClientID = Clients.ClientID
            WHERE 
                strftime('%Y-%m-%d', substr(Lending.EndLendDate, 7, 4) || '-' || substr(Lending.EndLendDate, 4, 2) || '-' || substr(Lending.EndLendDate, 1, 2)) >= DATE('now');";

            using (var command = new SqliteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var rentalItem = new RentalItems
                        {
                            LendID = reader.GetInt32(reader.GetOrdinal("LendID")),
                            NumberOfLend = reader["NumberOfLend"]?.ToString(),
                            ClientID = reader.GetInt32(reader.GetOrdinal("ClientID")),
                            NameOfCompany = reader["NameOfCompany"]?.ToString(),
                            StartLendDate = reader["StartLendDate"]?.ToString(),
                            EndLendDate = reader["EndLendDate"]?.ToString(),
                            AddressOfBuilding = reader["AddressOfBuilding"]?.ToString()
                        };

                        results.Add(rentalItem);
                    }
                }
            }
        }
        catch (Exception e)
        {
            MessageBox.Show("Błąd podczas pobierania danych z bazy: " + e.Message);
        }
    }

    return results;
}
    public static void AddShutteringLendingDetails(int lendingID, ObservableCollection<ItemShutteringRent> items, 
        string ContactPersonName, string ContactPersonSurname, string ContactPersonEmail, 
        string ContactPersonPhoneNumber)
    {
        using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
        {
            connection.Open();

            foreach (var item in items)
            {
                try
                {
                    double totalCost = item.AmountOfShuttering * item.PriceOfShuttering;
                    
                    var checkProductQuery = @"
                        SELECT COUNT(1) 
                        FROM ShutteringProduct 
                        WHERE ProductID = @ProductID;
                    ";
                    
                    using (var command = new SqliteCommand(checkProductQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", item.ProductID);
                        var productExists = Convert.ToInt32(command.ExecuteScalar()) > 0;
                        if (!productExists)
                        {
                            MessageBox.Show($"Produkt o ID {item.ProductID} nie istnieje w tabeli ShutteringProduct.");
                            return;
                        }
                    }
                    
                    var insertQuery = @"
                        INSERT INTO LendingShutteringDetails 
                        (LendingID, ProductID, Amount, TotalCost, ContactPersonName, 
                         ContactPersonSurname, ContactPersonPhoneNumber, ContactPersonEmail)
                        VALUES (@LendingID, @ProductID, @Amount, @TotalCost, @ContactPersonName, 
                                @ContactPersonSurname, @ContactPersonPhoneNumber, @ContactPersonEmail);
                    ";

                    using (var command = new SqliteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@LendingID", lendingID);
                        command.Parameters.AddWithValue("@ProductID", item.ProductID);
                        command.Parameters.AddWithValue("@Amount", item.AmountOfShuttering);
                        command.Parameters.AddWithValue("@TotalCost", totalCost);
                        command.Parameters.AddWithValue("@ContactPersonName", ContactPersonName);
                        command.Parameters.AddWithValue("@ContactPersonSurname", ContactPersonSurname);
                        command.Parameters.AddWithValue("@ContactPersonPhoneNumber", ContactPersonPhoneNumber);
                        command.Parameters.AddWithValue("@ContactPersonEmail", ContactPersonEmail);

                        int affectedRows = command.ExecuteNonQuery();
                        
                        if (affectedRows > 0)
                        {
                            MessageBox.Show($"Dane zostały dodane do LendingShutteringDetails.");
                        }
                        else
                        {
                            MessageBox.Show("Nie udało się dodać danych do LendingShutteringDetails.");
                        }
                    }
                    
                    var updateQuery = @"
                        UPDATE ShutteringProduct
                        SET AmountInStock = AmountInStock - @Amount
                        WHERE ProductID = @ProductID;
                    ";

                    using (var command = new SqliteCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Amount", item.AmountOfShuttering);
                        command.Parameters.AddWithValue("@ProductID", item.ProductID);

                        int affectedRows = command.ExecuteNonQuery();
                        
                        if (affectedRows > 0)
                        {
                            Console.WriteLine("Stan magazynowy został zaktualizowany.");
                        }
                        else
                        {
                            Console.WriteLine("Nie udało się zaktualizować stanu magazynowego.");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Błąd przy dodawaniu szczegółów wypożyczenia: {e.Message}");
                }
            }
        }
    }
    public static void ReturnShutteringItemsToStock()
{
    try
    {
        using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
        {
            connection.Open();

            // Zapytanie, które pobiera wynajmy, które już się zakończyły
            string query = @"SELECT 
                                LendingShutteringDetails.ProductID,
                                LendingShutteringDetails.Amount
                            FROM 
                                LendingShutteringDetails
                            INNER JOIN 
                                Lending ON LendingShutteringDetails.LendingID = Lending.LendID
                            WHERE 
                                strftime('%Y-%m-%d', substr(Lending.EndLendDate, 7, 4) || '-' || substr(Lending.EndLendDate, 4, 2) || '-' || substr(Lending.EndLendDate, 1, 2)) < DATE('now');";

            using (var command = new SqliteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int productID = reader.GetInt32(reader.GetOrdinal("ProductID"));
                        int amount = reader.GetInt32(reader.GetOrdinal("Amount"));

                        // Zaktualizowanie stanu magazynowego
                        var updateStockQuery = @"
                            UPDATE ShutteringProduct
                            SET AmountInStock = AmountInStock + @Amount
                            WHERE ProductID = @ProductID;
                        ";

                        using (var updateStockCommand = new SqliteCommand(updateStockQuery, connection))
                        {
                            updateStockCommand.Parameters.AddWithValue("@Amount", amount);
                            updateStockCommand.Parameters.AddWithValue("@ProductID", productID);
                            updateStockCommand.ExecuteNonQuery();
                        }
                    }
                }
            }

            // Komunikat o pomyślnym zakończeniu operacji
            MessageBox.Show("Przedmioty zostały zwrócone do magazynu.");
        }
    }
    catch (Exception ex)
    {
        // Obsługa błędów
        MessageBox.Show("Błąd podczas zwracania przedmiotów do magazynu: " + ex.Message);
    }
}
    public static void SetShutteringAmountToZeroInLendingDetails()
{
    try
    {
        using (var connection = new SqliteConnection($"Data Source={DataBaseName}"))
        {
            connection.Open();

            // Zapytanie, które pobiera wynajmy, które już się zakończyły
            string query = @"SELECT 
                                LendingShutteringDetails.ProductID,
                                LendingShutteringDetails.Amount
                            FROM 
                                LendingShutteringDetails
                            INNER JOIN 
                                Lending ON LendingShutteringDetails.LendingID = Lending.LendID
                            WHERE 
                                strftime('%Y-%m-%d', substr(Lending.EndLendDate, 7, 4) || '-' || substr(Lending.EndLendDate, 4, 2) || '-' || substr(Lending.EndLendDate, 1, 2)) < DATE('now');";

            using (var command = new SqliteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int productID = reader.GetInt32(reader.GetOrdinal("ProductID"));

                        // Ustawienie ilości na 0 w LendingShutteringDetails po zakończeniu wypożyczenia
                        var updateLendingShutteringQuery = @"
                            UPDATE LendingShutteringDetails
                            SET Amount = 0
                            WHERE ProductID = @ProductID;
                        ";

                        using (var updateCommand = new SqliteCommand(updateLendingShutteringQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@ProductID", productID);
                            updateCommand.ExecuteNonQuery();
                        }
                    }
                }
            }

            MessageBox.Show("Ilość w LendingShutteringDetails została ustawiona na 0.");
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Błąd podczas ustawiania ilości na 0 w LendingShutteringDetails: " + ex.Message);
    }
}

}


    