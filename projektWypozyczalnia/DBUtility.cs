using System.Windows;
using Microsoft.Data.Sqlite;

namespace projektWypozyczalnia;

public class DBUtility
{
    public static List<Client> aGetFromDatabase(string command = "SELECT * FROM Clients",
        string dataBaseName = $"Shutterings.db")
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
                        MessageBox.Show("while");
                        int ClientID = columns.Contains("ClientID") ? reader.GetInt32(columns.IndexOf("ClientID")) : -1;
                        var NameOfCompany = (columns.Contains("NameOfCompany") && !reader.IsDBNull(columns.IndexOf("NameOfCompany"))
                            ? reader.GetString(columns.IndexOf("NameOfCompany"))
                            : null) ?? string.Empty;
                        string Name = columns.Contains("Name") ? reader.GetString(columns.IndexOf("Name")) : String.Empty;
                        string Surname = columns.Contains("Surname") ? reader.GetString(columns.IndexOf("Surname")) : String.Empty;
                        string Address = columns.Contains("Address") ? reader.GetString(columns.IndexOf("Address")) : String.Empty;
                        string PhoneNumber = columns.Contains("PhoneNumber") ? reader.GetString(columns.IndexOf("PhoneNumber")) : String.Empty;
                        string Email = columns.Contains("Email") ? reader.GetString(columns.IndexOf("Email")) : String.Empty;
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
    private static dynamic ListOfSales<T>(string condition, List<string> columns, SqliteDataReader? reader)
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