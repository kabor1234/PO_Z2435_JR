using System.Windows;
using projektWypozyczalnia.AddingToStock;

namespace projektWypozyczalnia.RentWindows;

public partial class ChoosingAddingItemWindow : Window
{
    private RentWindow rentWindow;
    public ChoosingAddingItemWindow(RentWindow parentWindow)
    {
        InitializeComponent();
        rentWindow = parentWindow;
    }
    private void ChooseShutterings_OnClick(object sender, RoutedEventArgs e)
    {
        AddShutteringToRentWindow addShutterings = new AddShutteringToRentWindow(rentWindow);
        addShutterings.ShowDialog();
    }
    
    private void ChooseEquipment_OnClick(object sender, RoutedEventArgs e)
    {
        AddEquipmentToRentWindow addEquipment = new AddEquipmentToRentWindow();
        addEquipment.Show();
    }


    private void Exit_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }


}