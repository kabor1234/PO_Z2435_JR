using System.Windows;
using System.Windows.Controls;
using projektWypozyczalnia.Classes;

namespace projektWypozyczalnia.RentWindows;

public partial class ChooseClientForShutteringsWindow : Window
{
    private Client _clientFromCombobox;

    public ChooseClientForShutteringsWindow()
    {
        InitializeComponent();
        Loaded += Window_Loaded;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        var clients = DBUtility.GetAllClients();
        ClientComboBox.ItemsSource = clients;
        ClientComboBox.DisplayMemberPath = "DisplayName";
    }
    private void AddClientForShutterings_onClick(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }
    private void ChooseClientForShutterings_selectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ClientComboBox.SelectedItem != null)
        {
            _clientFromCombobox = (Client)ClientComboBox.SelectedItem;
        }
    }
    public Client GetSelectedClient()
    {
        return _clientFromCombobox;
    }
    private void CancelClientForShutteringsButton_OnClick(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}