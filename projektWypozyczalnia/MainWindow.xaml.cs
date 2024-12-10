using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace projektWypozyczalnia;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Rent_OnClick(object sender, RoutedEventArgs e)
    {
        RentWindow rentWindow = new RentWindow();
        rentWindow.Show();
    }

    private void Rental_OnClick(object sender, RoutedEventArgs e)
    {
        RentalWindow rentalWindow = new RentalWindow();
        rentalWindow.Show();
    }

    private void InventoryStatus_OnClick(object sender, RoutedEventArgs e)
    {
        InventoryStatusWindow inventoryStatusWindow = new InventoryStatusWindow();
        inventoryStatusWindow.Show();
    }

    private void AddToInventory_OnClick(object sender, RoutedEventArgs e)
    {
        AddToInventoryWindow addToInventoryWindow = new AddToInventoryWindow();
        addToInventoryWindow.Show();
    }

    private void Exit_OnClick(object sender, RoutedEventArgs e)
    {
        //Application.Current.Shutdown();
        DBUtility.GetClientsFromDatabase();
        List<Client> clients = DBUtility.GetClientsFromDatabase();
        foreach (var client in clients)
        {
            MessageBox.Show($"ID: {client.ClientID}, Name: {client.Name}, Surname: {client.Surname}");
        }
    }
}