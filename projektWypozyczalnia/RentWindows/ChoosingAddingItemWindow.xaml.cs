using System.Windows;
using projektWypozyczalnia.AddingToStock;

namespace projektWypozyczalnia.RentWindows;

public partial class ChoosingAddingItemWindow : Window
{
    public ChoosingAddingItemWindow()
    {
        InitializeComponent();
    }
    private void ChooseShutterings_OnClick(object sender, RoutedEventArgs e)
    {
        AddShutteringToRentingWindow addShutterings = new AddShutteringToRentingWindow();
        addShutterings.Show();
    }
    
    private void ChooseEquipment_OnClick(object sender, RoutedEventArgs e)
    {
        AddEquipmentToRentingWindow addEquipment = new AddEquipmentToRentingWindow();
        addEquipment.Show();
    }


    private void Exit_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }


}