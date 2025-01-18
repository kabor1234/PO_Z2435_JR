using System.Windows;
using projektWypozyczalnia.AddingToStock;

namespace projektWypozyczalnia.RentWindows;

public partial class ChoosingAddingItemWindow : Window
{
    private RentWindow _rentWindow;
    public ChoosingAddingItemWindow(RentWindow rentWindow)
    {
        InitializeComponent();
        _rentWindow = rentWindow;
    }
    private void ChooseShutterings_OnClick(object sender, RoutedEventArgs e)
    {
        AddShutteringToRentWindow addShutterings = new AddShutteringToRentWindow(_rentWindow);
        addShutterings.ShowDialog();
    }
    
    private void ChooseEquipment_OnClick(object sender, RoutedEventArgs e)
    {
        AddEquipmentToRentWindow addEquipment = new AddEquipmentToRentWindow(_rentWindow);
        addEquipment.Show();
    }


    private void Exit_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }


}