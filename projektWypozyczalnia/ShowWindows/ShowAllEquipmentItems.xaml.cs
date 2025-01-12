using System.Windows;
using projektWypozyczalnia.Classes;

namespace projektWypozyczalnia.ShowWindows;

public partial class ShowAllEquipmentItems : Window
{
    
    
    public ShowAllEquipmentItems()
    {
        InitializeComponent();
    }
    
    private void LoadEquipmentData()
    {
        var equipmentList = DBUtility.GetAllEquipment();
        StockDataGrid.ItemsSource = equipmentList
            .Select(name => new { EquipmentName = name })
            .ToList();
    }
    
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        LoadEquipmentData();
    }

    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}