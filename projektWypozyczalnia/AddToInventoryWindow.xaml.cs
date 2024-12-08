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

    private void Close_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}