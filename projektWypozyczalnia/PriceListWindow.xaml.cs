using System.Windows;

namespace projektWypozyczalnia;

public partial class PriceListWindow : Window
{
    
    private bool showEquipment = false;
    
    public PriceListWindow()
    {
        InitializeComponent();
    }

    private void EditPriceListButton_OnClick(object sender, RoutedEventArgs e)
    {
        EditPriceWindow editPriceWindow = new EditPriceWindow();
        editPriceWindow.Show();
    }

    private void CloseButton_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    
    private void ShowEquipmentCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        showEquipment = true;
        LoadData();
    }
    
    private void ShowEquipmentCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        showEquipment = false;
        LoadData();
    }
    
    private void LoadData()
    {
        if (showEquipment)
        {
            
        }
        else
        {
            LoadShutteringPriceData();
        }
    }
    
    private void LoadShutteringPriceData()
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
}