using System.Windows;

namespace projektWypozyczalnia;

public partial class PriceListWindow : Window
{
    
    private bool showEquipment = false;
    
    public PriceListWindow()
    {
        InitializeComponent();
        LoadData();
    }

    private void EditShutteringPrice_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void EditEquipmentPrice_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void RefreshWindow_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
        DBUtility.GetShutteringPriceData();
        PriceListWindow priceListWindow = new PriceListWindow();
        priceListWindow.Show();
    }

    private void Close_OnClick(object sender, RoutedEventArgs e)
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
            var data = DBUtility.GetShutteringPriceData();
            if (data == null || !data.Any())
            {
                MessageBox.Show("Brak danych do wyświetlenia.");
            }
            else
            {
                PriceDataGrid.ItemsSource = data;
                SetColumnVisibilityForShuttering();
            }
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
            var data = DBUtility.GetEquipmentPriceData();
            if (data == null || !data.Any())
            {
                MessageBox.Show("Brak danych do wyświetlenia.");
            }
            else
            {
                PriceDataGrid.ItemsSource = data;
                SetColumnVisibilityForEquipment();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Błąd podczas ładowania danych osprzętu: " + ex.Message);
        }
    }
    
    private void SetColumnVisibilityForShuttering()
    {
        PriceDataGrid.Columns[0].Visibility = Visibility.Visible;  // Manufacturer
        PriceDataGrid.Columns[1].Visibility = Visibility.Visible;  // Name
        PriceDataGrid.Columns[2].Visibility = Visibility.Visible;  // Length
        PriceDataGrid.Columns[3].Visibility = Visibility.Visible;  // Width
        PriceDataGrid.Columns[4].Visibility = Visibility.Visible;  // Price

        PriceDataGrid.Columns[5].Visibility = Visibility.Collapsed;  // EquipmentName
        PriceDataGrid.Columns[6].Visibility = Visibility.Collapsed;  // Price
    }
    
    private void SetColumnVisibilityForEquipment()
    {
        PriceDataGrid.Columns[0].Visibility = Visibility.Collapsed;
        PriceDataGrid.Columns[1].Visibility = Visibility.Collapsed;
        PriceDataGrid.Columns[2].Visibility = Visibility.Collapsed;
        PriceDataGrid.Columns[3].Visibility = Visibility.Collapsed;
        PriceDataGrid.Columns[4].Visibility = Visibility.Collapsed;

        PriceDataGrid.Columns[5].Visibility = Visibility.Visible;
        PriceDataGrid.Columns[6].Visibility = Visibility.Visible;
    }
}