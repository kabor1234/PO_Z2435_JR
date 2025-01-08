using System.Windows;

namespace projektWypozyczalnia;

public partial class ShowAllEquipmentItems : Window
{
    DBUtility dbUtility = new DBUtility();
    
    public ShowAllEquipmentItems()
    {
        InitializeComponent();
    }
    
    private void LoadEquipmentData()
    {
        var equipmentList = dbUtility.GetAllEquipment();
        StockDataGrid.ItemsSource = equipmentList
            .Select(name => new { EquipmentName = name })  // Mapowanie na obiekt anonimowy
            .ToList();
    }
    
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        LoadEquipmentData();
    }

    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}