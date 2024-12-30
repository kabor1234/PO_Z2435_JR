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
    

    private void AddShutteringToStock_OnClick(object sender, RoutedEventArgs e)
    {
        AddShutteringToStockWindow addShutteringToStock = new AddShutteringToStockWindow();
        addShutteringToStock.Show();
    }

    private void AddEquipmentToStock_OnClick(object sender, RoutedEventArgs e)
    {
        AddEquipmentToStockWindow addEquipmentToStockWindow = new AddEquipmentToStockWindow();
        addEquipmentToStockWindow.Show();
    }
}