using System.Windows;
using Microsoft.Data.Sqlite;

namespace projektWypozyczalnia;

public class DBUtility
{
    public static List<Client> GetClientsFromDatabase(string command = "SELECT * FROM Clients",
        string dataBaseName = "Shutterings.db")
    {
        var columns = new List<string> {"ClientID", "NameOfCompany", "Name", "Surname", "Address", "PhoneNumber", "E-mail"};
        List<Client> Clients = new();
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
                        int ClientID = GetDBColumnValue<int>("ClientID", columns, reader);
                        string NameOfCompany = GetDBColumnValue<string>("NameOfCompany", columns, reader);
                        string Name = GetDBColumnValue<string>("Name", columns, reader);
                        string Surname = GetDBColumnValue<string>("Surname", columns, reader);
                        string Address = GetDBColumnValue<string>("Address", columns, reader);
                        string PhoneNumber = GetDBColumnValue<string>("PhoneNumber", columns, reader);
                        string Email = GetDBColumnValue<string>("Email", columns, reader);
                        Clients.Add(new Client(ClientID, NameOfCompany, Name, Surname, Address, PhoneNumber, Email));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.Close();
            }
        }
        
        return Clients;
    }

    public static List<Sale> GetSalesFromDatabase(string command = "SELECT * FROM Sales",
        string dataBaseName = "Shutterings.db")
    {
        var columns = new List<string>
            { "SalesID", "ClientID", "SaleDate", "Comments" };
        List<Sale> Sales = new();
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
                        int SalesID = GetDBColumnValue<int>("SalesID", columns, reader);
                        int ClientID = GetDBColumnValue<string>("ClientID", columns, reader);
                        string SaleDate = GetDBColumnValue<string>("SaleDate", columns, reader);
                        string Comments = GetDBColumnValue<string>("Comment", columns, reader);
                        Sales.Add(new Sale(SalesID, ClientID, SaleDate, Comments));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.Close();
            }
        }

        return Sales;
    }
    public static List<SaleDetails> GetSaleDetailsFromDatabase(string command = "SELECT * FROM SalesDetails",
        string dataBaseName = "Shutterings.db")
    {
        var columns = new List<string>
            { "SalesDetailsID", "SalesID", "ProductID", "EquipmentID", "Amount", "PricePerUnit" };
        List<SaleDetails> saleDetail = new();
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
                        int SalesDetailsID = GetDBColumnValue<int>("SalesDetailID", columns, reader);
                        int SailsID = GetDBColumnValue<string>("SailsID", columns, reader);
                        int ProductID = GetDBColumnValue<string>("ProductID", columns, reader);
                        int EquipmentID = GetDBColumnValue<string>("EquipmentID", columns, reader);
                        int Amount = GetDBColumnValue<string>("Amount", columns, reader);
                        float PricePerUnit = GetDBColumnValue<string>("PricePerUnit", columns, reader);
                        saleDetail.Add(new SaleDetails(SalesDetailsID, SailsID, ProductID, EquipmentID, Amount, PricePerUnit));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.Close();
            }
        }

        return saleDetail;
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
}