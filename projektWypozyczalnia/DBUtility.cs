using System.Windows;
using Microsoft.Data.Sqlite;

namespace projektWypozyczalnia;

public class DBUtility
{
    private static readonly string dataBaseName = "Shutterings.db";
    public static List<Client> GetClientsFromDatabase(string command = "SELECT * FROM Clients")
    {
        var columns = new List<string> {"ClientID", "NameOfCompany", "Name", "Surname", "Address", "PhoneNumber", "E-mail"};
        List<Client> clients = new();
        SQLitePCL.Batteries.Init();

        using (var connection = new SqliteConnection($"Data Source={dataBaseName}"))
        {
            try
            {
                connection.Open();
                var sqliteCommand = connection.CreateCommand();
                sqliteCommand.CommandText = command;

                using (var reader = sqliteCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int clientId = GetDBColumnValue<int>("ClientID", columns, reader);
                        string nameOfCompany = GetDBColumnValue<string>("NameOfCompany", columns, reader);
                        string name = GetDBColumnValue<string>("Name", columns, reader);
                        string surname = GetDBColumnValue<string>("Surname", columns, reader);
                        string address = GetDBColumnValue<string>("Address", columns, reader);
                        string phoneNumber = GetDBColumnValue<string>("PhoneNumber", columns, reader);
                        string email = GetDBColumnValue<string>("Email", columns, reader);
                        clients.Add(new Client(clientId, nameOfCompany, name, surname, address, phoneNumber, email));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.Close();
            }
        }
        
        return clients;
    }

    public static List<Sale> GetSalesFromDatabase(string command = "SELECT * FROM Sales")
    {
        var columns = new List<string> { "SalesID", "ClientID", "SaleDate", "Comments" };
        List<Sale> sales = new();
        SQLitePCL.Batteries.Init();

        using (var connection = new SqliteConnection($"Data Source={dataBaseName}"))
        {
            try
            {
                connection.Open();
                var sqliteCommand = connection.CreateCommand();
                sqliteCommand.CommandText = command;

                using (var reader = sqliteCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int salesId = GetDBColumnValue<int>("SalesID", columns, reader);
                        int clientId = GetDBColumnValue<string>("ClientID", columns, reader);
                        string saleDate = GetDBColumnValue<string>("SaleDate", columns, reader);
                        string comments = GetDBColumnValue<string>("Comment", columns, reader);
                        sales.Add(new Sale(salesId, clientId, saleDate, comments));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.Close();
            }
        }

        return sales;
    }
    public static List<SaleDetails> GetSaleDetailsFromDatabase(string command = "SELECT * FROM SalesDetails")
    {
        var columns = new List<string>
            { "SalesDetailsID", "SalesID", "ProductID", "EquipmentID", "Amount", "PricePerUnit" };
        List<SaleDetails> salesDetails = new();
        SQLitePCL.Batteries.Init();

        using (var connection = new SqliteConnection($"Data Source={dataBaseName}"))
        {
            try
            {
                connection.Open();
                var sqliteCommand = connection.CreateCommand();
                sqliteCommand.CommandText = command;

                using (var reader = sqliteCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int salesDetailsId = GetDBColumnValue<int>("SalesDetailID", columns, reader);
                        int sailsId = GetDBColumnValue<string>("SailsID", columns, reader);
                        int productId = GetDBColumnValue<string>("ProductID", columns, reader);
                        int equipmentId = GetDBColumnValue<string>("EquipmentID", columns, reader);
                        int amount = GetDBColumnValue<string>("Amount", columns, reader);
                        float pricePerUnit = GetDBColumnValue<string>("PricePerUnit", columns, reader);
                        salesDetails.Add(new SaleDetails(salesDetailsId, sailsId, productId, equipmentId, amount, pricePerUnit));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.Close();
            }
        }

        return salesDetails;
    }
    
    public static List<Lending> GetLendingFromDatabase(string command = "SELECT * FROM Lending")
    {
        var columns = new List<string> {"LendID", "ClientID", "StartLendDate", "EndLaneDate", "Comments"};
        List<Lending> lending = new();
        SQLitePCL.Batteries.Init();

        using (var connection = new SqliteConnection($"Data Source={dataBaseName}"))
        {
            try
            {
                connection.Open();
                var sqliteCommand = connection.CreateCommand();
                sqliteCommand.CommandText = command;

                using (var reader = sqliteCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int lendId = GetDBColumnValue<int>("LendID", columns, reader);
                        int clientId = GetDBColumnValue<string>("ClientID", columns, reader);
                        string startLendDate = GetDBColumnValue<string>("StartLendDate", columns, reader);
                        string endLendDate = GetDBColumnValue<string>("EndLendDate", columns, reader);
                        string comments = GetDBColumnValue<string>("Comments", columns, reader);
                        lending.Add(new Lending(lendId, clientId, startLendDate, endLendDate, comments));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.Close();
            }
        }
        
        return lending;
    }
    
    public static List<LendingDetails> GetLendingDetailsFromDatabase(string command = "SELECT * FROM LendingDetails")
    {
        var columns = new List<string>
            { "detailsID", "SalesID", "ProductID", "EquipmentID", "Amount"};
        List<LendingDetails> lendingDetails = new();
        SQLitePCL.Batteries.Init();

        using (var connection = new SqliteConnection($"Data Source={dataBaseName}"))
        {
            try
            {
                connection.Open();
                var sqliteCommand = connection.CreateCommand();
                sqliteCommand.CommandText = command;

                using (var reader = sqliteCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int detailsId = GetDBColumnValue<int>("DetailID", columns, reader);
                        int sailsId = GetDBColumnValue<string>("SailsID", columns, reader);
                        int productId = GetDBColumnValue<string>("ProductID", columns, reader);
                        int equipmentId = GetDBColumnValue<string>("EquipmentID", columns, reader);
                        int amount = GetDBColumnValue<string>("Amount", columns, reader);
                        lendingDetails.Add(new LendingDetails(detailsId, sailsId, productId, equipmentId, amount));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.Close();
            }
        }

        return lendingDetails;
    }
    
    public static List<SystemOfShuttering> GetSystemOfShutteringFromDatabase(string command = "SELECT * FROM SystemOfShuttering")
    {
        var columns = new List<string>
            { "detailsID", "SalesID", "ProductID", "EquipmentID", "Amount"};
        List<SystemOfShuttering> systemOfShutterings = new();
        SQLitePCL.Batteries.Init();

        using (var connection = new SqliteConnection($"Data Source={dataBaseName}"))
        {
            try
            {
                connection.Open();
                var sqliteCommand = connection.CreateCommand();
                sqliteCommand.CommandText = command;

                using (var reader = sqliteCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int systemID = GetDBColumnValue<int>("SystemID", columns, reader);
                        string nameOfShuttering = GetDBColumnValue<string>("NameOfShuttering", columns, reader);
                        string manufacturer = GetDBColumnValue<string>("Manufacture", columns, reader);
                        string summary = GetDBColumnValue<string>("Summary", columns, reader);
                        systemOfShutterings.Add(new SystemOfShuttering(systemID, nameOfShuttering, manufacturer, summary));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.Close();
            }
        }

        return systemOfShutterings;
    }
    
    public static List<LengthCategory> GetLengthCategoryFromDatabase(string command = "SELECT * FROM LenghtCategory")
    {
        var columns = new List<string>
            { "LengthID", "SystemID", "Length"};
        List<LengthCategory> lengthCategory = new();
        SQLitePCL.Batteries.Init();

        using (var connection = new SqliteConnection($"Data Source={dataBaseName}"))
        {
            try
            {
                connection.Open();
                var sqliteCommand = connection.CreateCommand();
                sqliteCommand.CommandText = command;

                using (var reader = sqliteCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int lengthId = GetDBColumnValue<int>("SystemID", columns, reader);
                        int systemId = GetDBColumnValue<string>("NameOfShuttering", columns, reader);
                        int length = GetDBColumnValue<string>("Manufacture", columns, reader);
                        lengthCategory.Add(new LengthCategory(length, systemId, lengthId));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.Close();
            }
        }

        return lengthCategory;
    }
    
    public static List<ShutteringProduct> GetShutteringProductsFromDatabase(string command = "SELECT * FROM ShutteringCategory")
    {
        var columns = new List<string>
            { "ProductID", "CategoryID", "Width", "AmountInStock"};
        List<ShutteringProduct> shutteringProduct = new();
        SQLitePCL.Batteries.Init();

        using (var connection = new SqliteConnection($"Data Source={dataBaseName}"))
        {
            try
            {
                connection.Open();
                var sqliteCommand = connection.CreateCommand();
                sqliteCommand.CommandText = command;

                using (var reader = sqliteCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int productId = GetDBColumnValue<int>("ProductID", columns, reader);
                        int categoryId = GetDBColumnValue<string>("CategoryID", columns, reader);
                        int width = GetDBColumnValue<string>("Width", columns, reader);
                        int amountInStock = GetDBColumnValue<int>("AmountInStock", columns, reader);
                        shutteringProduct.Add(new ShutteringProduct(productId, categoryId, width, amountInStock));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.Close();
            }
        }

        return shutteringProduct;
    }
    
    public static List<Equipment> GetEquipmentFromDatabase(string command = "SELECT * FROM Equipment")
    {
        var columns = new List<string>
            { "EquipmentID", "NameOfEquipment", "Summary", "AmountInStock"};
        List<Equipment> equipment = new();
        SQLitePCL.Batteries.Init();

        using (var connection = new SqliteConnection($"Data Source={dataBaseName}"))
        {
            try
            {
                connection.Open();
                var sqliteCommand = connection.CreateCommand();
                sqliteCommand.CommandText = command;

                using (var reader = sqliteCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int equipmentId = GetDBColumnValue<int>("EquipmentID", columns, reader);
                        string nameOfEquipment = GetDBColumnValue<string>("NameOfEquipment", columns, reader);
                        string summary = GetDBColumnValue<string>("Summary", columns, reader);
                        int amountInStock = GetDBColumnValue<int>("AmountInStock", columns, reader);
                        equipment.Add(new Equipment(equipmentId, nameOfEquipment, summary, amountInStock));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.Close();
            }
        }

        return equipment;
    }

    private static dynamic GetDBColumnValue<T>(string condition, List<string> columns, SqliteDataReader? reader)
    {
        if (reader == null) throw new NullReferenceException();
        if (typeof(T) == typeof(double))
        {
            var temp = columns.Contains(condition) && !reader.IsDBNull(columns.IndexOf(condition))
                ? reader.GetDouble(columns.IndexOf(condition))
                : 0;
            return temp;
        }

        if (typeof(T) == typeof(string))
        {
            var temp = (columns.Contains(condition) && !reader.IsDBNull(columns.IndexOf(condition))
                ? reader.GetString(columns.IndexOf(condition))
                : null) ?? string.Empty;
            return temp;
        }

        if(typeof(T) == typeof(int))
        {
            var temp = columns.Contains(condition) && !reader.IsDBNull(columns.IndexOf(condition))
                ? reader.GetInt32(columns.IndexOf(condition))
                : 0;
            return temp;
        }

        throw new NotSupportedException($"The type {typeof(T).Name} is not supported.");
    }
    
    public void AddShutteringSystemToDatabase(string nameOfShuttering, string manufacturer, int length, List<int> widths)
    {
        string dataBaseName = "Shutterings.db";

        using (var connection = new SqliteConnection($"Data Source={dataBaseName}"))
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
    
    public List<AvailableSystemsOfShuttering> GetAllShutteringSystems()
    {
        string databaseName = "Shutterings.db";
        var results = new List<AvailableSystemsOfShuttering>();

        using (var connection = new SqliteConnection($"Data Source={databaseName}"))
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
                            Console.WriteLine($"NameOfShuttering: {reader["NameOfShuttering"]}, Manufacturer: {reader["Manufacturer"]}, Length: {reader["Length"]}, Width: {reader["Width"]}");
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
    
        public List<string> GetShutteringSystems()
    {
        var results = new List<string>();

        using (var connection = new SqliteConnection($"Data Source={dataBaseName}"))
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

    public List<int> GetLengthsForSystem(string systemName)
    {
        var results = new List<int>();

        using (var connection = new SqliteConnection($"Data Source={dataBaseName}"))
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

    public List<int> GetWidthsForLength(int length)
    {
        var results = new List<int>();

        using (var connection = new SqliteConnection($"Data Source={dataBaseName}"))
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

    public void AddToStock(string systemName, int length, int width, int amount)
    {
        using (var connection = new SqliteConnection($"Data Source={dataBaseName}"))
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
    
    
        // Dodanie nowego osprzętu
        public void AddEquipmentToDatabase(string nameOfEquipment)
        {
            using (var connection = new SqliteConnection($"Data Source={dataBaseName}"))
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

        // Pobranie listy osprzętu (ComboBox)
        public Dictionary<int, string> GetEquipmentList()
        {
            var equipmentList = new Dictionary<int, string>();

            using (var connection = new SqliteConnection($"Data Source={dataBaseName}"))
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

        // Dodanie ilości do wybranego osprzętu
        public void AddAmountToEquipment(int equipmentId, int amount)
        {
            using (var connection = new SqliteConnection($"Data Source={dataBaseName}"))
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
}


    