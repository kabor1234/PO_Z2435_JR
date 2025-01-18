using System.Windows;

namespace projektWypozyczalnia.RentWindows;

public partial class RentingsWindow : Window
{
    public RentingsWindow()
    {
        InitializeComponent();
    }

    private void ShowExpiredRentalCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        
    }

    private void ShowExpiredRentalCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        
    }

    private void Rent_OnClick(object sender, RoutedEventArgs e)
    {
        RentWindow rentWindow = new();
        rentWindow.Show();
    }

    private void RefreshWindow_OnClick(object sender, RoutedEventArgs e)
    {
    }

    private void ShowEquipmentCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        
    }
    
    private void Close_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}