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

    private void AddToStock_OnClick(object sender, RoutedEventArgs e)
    {
        AddShutteringToStockWindow addShutteringToStockWindow = new AddShutteringToStockWindow();
        addShutteringToStockWindow.Show();
    }

    private void RemoveFromStock_OnClick(object sender, RoutedEventArgs e)
    {
        RemoveFromStockWindow removeFromStockWindow = new RemoveFromStockWindow();
        removeFromStockWindow.Show();
    }

    private void AddNewShutteringSystem_OnClick(object sender, RoutedEventArgs e)
    {
        AddNewShutteringSystem addNewShutteringSystem = new AddNewShutteringSystem();
        addNewShutteringSystem.Show();
    }
    
}