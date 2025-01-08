using System.Windows;
using Microsoft.Data.Sqlite;

namespace projektWypozyczalnia;

public partial class StockConditionWindow : Window
{
    private DBUtility dbUtility = new DBUtility();
    private bool showEquipment = false;
    public StockConditionWindow()
    {
        InitializeComponent();
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
    
    private void Close_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void ShowEquipmentCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        // Equipment Columns
        StockDataGrid.Columns[5].Visibility = Visibility.Visible;  // EquipmentName
        StockDataGrid.Columns[6].Visibility = Visibility.Visible;  // EquipmentAmount

        // Shuttering Columns
        StockDataGrid.Columns[0].Visibility = Visibility.Collapsed;  // Manufacturer
        StockDataGrid.Columns[1].Visibility = Visibility.Collapsed;  // Name
        StockDataGrid.Columns[2].Visibility = Visibility.Collapsed;  // Length
        StockDataGrid.Columns[3].Visibility = Visibility.Collapsed;  // Width
        StockDataGrid.Columns[4].Visibility = Visibility.Collapsed;  // Amount
    }

    private void ShowEquipmentCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        StockDataGrid.Columns[5].Visibility = Visibility.Collapsed;  // EquipmentName
        StockDataGrid.Columns[6].Visibility = Visibility.Collapsed;  // EquipmentAmount
        
        StockDataGrid.Columns[0].Visibility = Visibility.Visible;  // Manufacturer
        StockDataGrid.Columns[1].Visibility = Visibility.Visible;  // Name
        StockDataGrid.Columns[2].Visibility = Visibility.Visible;  // Length
        StockDataGrid.Columns[3].Visibility = Visibility.Visible;  // Width
        StockDataGrid.Columns[4].Visibility = Visibility.Visible;  // Amount
    }
}