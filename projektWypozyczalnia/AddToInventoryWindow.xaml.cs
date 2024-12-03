using System.Transactions;
using System.Windows;
using Microsoft.Data.Sqlite;

namespace projektWypozyczalnia;

public partial class AddToInventoryWindow : Window
{
    public AddToInventoryWindow()
    {
        InitializeComponent();
    }
    
    public static List<Transaction> DeleteFromDatabase(int index,string dataBaseName = $"FinanseDataBase.db",string tableName = $"ListaTranzakcji")
    {
        string command = $"DELETE FROM {tableName} WHERE ID = {index}";
        List<Transaction> transactions = new();
        SQLitePCL.Batteries.Init();

        using (var connection = new SqliteConnection($"Data Source={dataBaseName}"))
        {
            try
            {
                connection.Open(); 
                var sqliteCommand = connection.CreateCommand();
                sqliteCommand.CommandText = command;
                sqliteCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                connection.Close();
            }
        }

        return transactions;
    }
}