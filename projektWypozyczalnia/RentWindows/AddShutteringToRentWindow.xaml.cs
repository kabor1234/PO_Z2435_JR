using System.Windows;
using System.Windows.Controls;
using projektWypozyczalnia.Classes;

namespace projektWypozyczalnia.RentWindows;

public partial class AddShutteringToRentWindow : Window
{
    private List<WidthForLength> _widths = new();
    private RentWindow _rentWindow;
    private WidthForLength _selectedWidthInfo;

    public AddShutteringToRentWindow(RentWindow rentWindow)
    {
        InitializeComponent();
        LoadShutteringSystems();
        _rentWindow = rentWindow;
    }

    private void RentShutterings_onClick(object sender, RoutedEventArgs e)
    {    
        string system = ShutteringComboBox.Text;
        string length = LengthComboBox.Text;
        string width = WidthComboBox.Text;
        
        if (!int.TryParse(AmountTextBox.Text, out int amount))
        {
            MessageBox.Show("Proszę wprowadzić poprawną ilość.");
            return;
        }
        
        double pricePerUnit = 0;
        double cost = 0;
        
        int selectedLength = int.Parse(length);
        int selectedWidth = int.Parse(width);
        
        var widths = DBUtility.GetWidthsForLength(selectedLength);
        var selectedWidthInfo = widths.FirstOrDefault(w => w.Width == selectedWidth);

        if (selectedWidthInfo != null)
        {
            pricePerUnit = selectedWidthInfo.Price;
        }

        
        if (amount > selectedWidthInfo.AmountInStock)
        {
            int remainingAmount = selectedWidthInfo.AmountInStock - amount;
            AvaibleAmountOfShutteringTextBlock.Text = $"Pozostała ilość: {remainingAmount}";
            MessageBox.Show("Ilość nie może przekroczyć dostępnej ilości w magazynie.");
            return;
        }
        
        cost = amount * pricePerUnit;
            
        PriceOfShutteringTextBlock.Text = $"Cena: {pricePerUnit:F2}";
        
        
        string name = $"Szalunek ścienny {system} - {length}x{width}";
        var item = new Item
        {
            Name = name,
            Amount = amount,
            PricePerUnit = pricePerUnit,
            TotalPrice = cost
        };
        
        _rentWindow.AddItemToList(item);
        Close();
    }

    private void Cancel_OnClick(object sender, RoutedEventArgs e)
    {
        if (_selectedWidthInfo != null)
        {
            if (int.TryParse(AmountTextBox.Text, out int amount))
            {
                _selectedWidthInfo.AmountInStock += amount;
                DBUtility.UpdateWidthStock(_selectedWidthInfo);
            }
            else
            {
                MessageBox.Show("Wprowadź poprawną ilość.");
            }
        }
        
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
        _widths = DBUtility.GetWidthsForLength(length);

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
                double pricePerUnit = selectedWidthInfo.Price;
                
                AvaibleAmountOfShutteringTextBlock.Text = $"{selectedWidthInfo.AmountInStock}";
                if (pricePerUnit == 0)
                {
                    PriceOfShutteringTextBlock.Text = "Brak ceny";
                }
                else
                {
                    PriceOfShutteringTextBlock.Text = $"Cena: {pricePerUnit:F2}";
                }
            }
        }
    }
}
