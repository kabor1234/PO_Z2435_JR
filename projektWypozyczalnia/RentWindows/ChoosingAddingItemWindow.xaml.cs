using System.Windows;

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
        throw new NotImplementedException();
    }
    private void Exit_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}