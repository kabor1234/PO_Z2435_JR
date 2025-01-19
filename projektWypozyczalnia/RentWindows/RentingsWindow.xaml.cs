using System.Windows;
using projektWypozyczalnia.Classes;

namespace projektWypozyczalnia.RentWindows;

public partial class RentingsWindow : Window
{
    public RentingsWindow()
    {
        InitializeComponent();
        LoadRentalItemsData();
    }

    private void LoadRentalItemsData()
    {
        var data = DBUtility.GetRentalItems();
        RentsDataGrid.ItemsSource = data;
    }

    private void Rent_OnClick(object sender, RoutedEventArgs e)
    {
        RentWindow rentWindow = new RentWindow();
        rentWindow.Show();
    }

    private void RefreshWindow_OnClick(object sender, RoutedEventArgs e)
    {
    }

    
    private void Close_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}