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
        //var data = DBUtility.GetRentalItems();
       //RentsDataGrid.ItemsSource = data;
    }

    private void RentShutterings_OnClick(object sender, RoutedEventArgs e)
    {
        RentShutteringsWindow rentWindow = new RentShutteringsWindow();
        rentWindow.Show();
    }
    
    private void RentEquipment_OnClick(object sender, RoutedEventArgs e)
    {
        RentEquipmentWindow rentEquipmentWindow = new RentEquipmentWindow(); 
        rentEquipmentWindow.Show();
    }

    private void RefreshWindow_OnClick(object sender, RoutedEventArgs e)
    {
    }

    
    private void Close_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }


}