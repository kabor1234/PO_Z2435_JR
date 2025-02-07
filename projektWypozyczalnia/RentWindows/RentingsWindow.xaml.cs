using System.Windows;
using projektWypozyczalnia.Classes;

namespace projektWypozyczalnia.RentWindows;

public partial class RentingsWindow : Window
{
    public RentingsWindow()
    {
        InitializeComponent();
        DBUtility.ReturnShutteringItemsToStock();
        Loaded += RentingsWindow_OnLoaded;

    }
    private void RentingsWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        var lendings = DBUtility.GetAllLendings()
            .Where(lending => !string.IsNullOrWhiteSpace(lending.NumberOfLend))
            .ToList();

        if (lendings.Any())
            RentsDataGrid.ItemsSource = lendings;
        

        
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
        Close();
        
        RentingsWindow rentingsWindow = new RentingsWindow();
        rentingsWindow.Show();
    }

    
    private void Close_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }


    private void DetailsMenuItem_Click(object sender, RoutedEventArgs e)
    {
    }
}