using System.Windows;
using System.Windows.Controls;
using projektWypozyczalnia.Classes;

namespace projektWypozyczalnia.RentWindows;

public partial class AddShutteringToRentWindow : Window
{
    private List<WidthForLength> _widths = new();
    private RentWindow rentWindow;

    public AddShutteringToRentWindow(RentWindow parentWindow)
    {
        InitializeComponent();
        rentWindow = parentWindow;
        LoadShutteringSystems();
    }

    private void RentShutterings_onClick(object sender, RoutedEventArgs e)
    {
        // Tworzenie nowego elementu (dane przykładowe)
        var newItem = new Item
        {
            Name = "Nowy sprzęt",
            Quantity = 2,
            Price = 200.0
        };

        // Wywołanie metody w RentWindow
        rentWindow.AddItemToDataGrid(newItem);

        // Zamknij okno po dodaniu elementu
        this.Close();
    }

    private void Cancel_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
    
    private void LoadShutteringSystems()
    {
        ShutteringComboBox.Items.Clear();
        List<ShutteringSystem> systems = DBUtility.GetShutteringSystems();

        foreach (var system in systems)
        {
            ShutteringComboBox.Items.Add(system.NameOfShuttering);
        }
    }

    private void ShutteringComboBox_SelectionChanged(object sender, SelectionChangedEventArgs changedEventArgs)
    {
        string selectedSystem = (string)ShutteringComboBox.SelectedItem;
        LoadLengthsForSystem(selectedSystem);
    }

    private void LoadLengthsForSystem(string systemName)
    {
        LengthComboBox.Items.Clear();
        List<LengthForSystem> lengths = DBUtility.GetLengthsForSystem(systemName);

        foreach (var length in lengths)
        {
            LengthComboBox.Items.Add(length.Length);
        }
    }

    private void LengthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        int selectedLength = (int)LengthComboBox.SelectedItem;
        LoadWidthsForLength(selectedLength);
    }

    private void LoadWidthsForLength(int length)
    {
        WidthComboBox.Items.Clear();
        _widths = DBUtility.GetWidthsForLength(length);  // Pobieramy dane i zapisujemy w zmiennej globalnej

        foreach (var width in _widths)
        {
            WidthComboBox.Items.Add(width.Width);
        }
    }

    private void WidthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (WidthComboBox.SelectedItem != null)
        {
            int selectedWidth = (int)WidthComboBox.SelectedItem;
            
            var selectedWidthInfo = _widths.FirstOrDefault(w => w.Width == selectedWidth);

            if (selectedWidthInfo != null)
            {
                AvaibleAmountOfShutteringTextBlock.Text = $"{selectedWidthInfo.AmountInStock}";
            }
        }
    }
}
