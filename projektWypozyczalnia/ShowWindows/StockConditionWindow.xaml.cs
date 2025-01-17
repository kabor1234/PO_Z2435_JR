using System.Windows;
using projektWypozyczalnia.AddingToStock;
using projektWypozyczalnia.Classes;

namespace projektWypozyczalnia.ShowWindows;

public partial class StockConditionWindow : Window
{
    
    private bool _showEquipment = false;

    public StockConditionWindow()
    {
        InitializeComponent();
        LoadShutteringData();
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
        Close();
    }
    
    private void ShowEquipmentCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        _showEquipment = true;
        LoadData();
    }
    
    private void ShowEquipmentCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        _showEquipment = false;
        LoadData();
    }
    
    private void LoadData()
    {
        if (_showEquipment)
        {
            LoadEquipmentData();
        }
        else
        {
            LoadShutteringData();
        }
    }
    
    
    private void LoadShutteringData()
    {
        try
        {
            var data = DBUtility.GetShutteringStockData();
            StockDataGrid.ItemsSource = data;
            
            SetColumnVisibilityForShuttering();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Błąd podczas ładowania danych szalunków: " + ex.Message);
        }
    }
    
    private void LoadEquipmentData()
    {
        try
        {
            var data = DBUtility.GetEquipmentStockData();
            StockDataGrid.ItemsSource = data;
            SetColumnVisibilityForEquipment();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Błąd podczas ładowania danych osprzętu: " + ex.Message);
        }
    }
    
    private void SetColumnVisibilityForShuttering()
    {
        StockDataGrid.Columns[0].Visibility = Visibility.Visible;  // Manufacturer
        StockDataGrid.Columns[1].Visibility = Visibility.Visible;  // Name
        StockDataGrid.Columns[2].Visibility = Visibility.Visible;  // Length
        StockDataGrid.Columns[3].Visibility = Visibility.Visible;  // Width
        StockDataGrid.Columns[4].Visibility = Visibility.Visible;  // Amount

        StockDataGrid.Columns[5].Visibility = Visibility.Collapsed;  // EquipmentName
        StockDataGrid.Columns[6].Visibility = Visibility.Collapsed;  // EquipmentAmount
    }
    
    private void SetColumnVisibilityForEquipment()
    {
        StockDataGrid.Columns[0].Visibility = Visibility.Collapsed;
        StockDataGrid.Columns[1].Visibility = Visibility.Collapsed;
        StockDataGrid.Columns[2].Visibility = Visibility.Collapsed;
        StockDataGrid.Columns[3].Visibility = Visibility.Collapsed;
        StockDataGrid.Columns[4].Visibility = Visibility.Collapsed;

        StockDataGrid.Columns[5].Visibility = Visibility.Visible;
        StockDataGrid.Columns[6].Visibility = Visibility.Visible;
    }

    private void RefreshWindow_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
        StockConditionWindow newWindow = new StockConditionWindow();
        newWindow.Show();
    }
}