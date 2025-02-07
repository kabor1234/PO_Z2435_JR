using System.Windows;
using System.Windows.Controls;
using projektWypozyczalnia.Classes;

namespace projektWypozyczalnia.RentWindows;

public partial class AddShutteringToRentWindow : Window
{
    private List<WidthForLength> _widths = new();
    private RentShutteringsWindow _rentWindow;
    private WidthForLength _availableAmountItemInfo;

    public AddShutteringToRentWindow(RentShutteringsWindow rentWindow, WidthForLength availableAmountItemInfo)
    {
        InitializeComponent();
        _rentWindow = rentWindow; // Przypisanie RentWindow
        _availableAmountItemInfo = availableAmountItemInfo;
        LoadShutteringSystems();
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
        double totalCost = 0;

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
            MessageBox.Show("Ilość nie może przekroczyć dostępnej ilości w magazynie.");
            return;
        }

        totalCost = amount * pricePerUnit;

        PriceOfShutteringTextBlock.Text = $"Cena: {pricePerUnit:F2}";
        

        string nameOfShuttering = system;
        string lengthOfShuttering = length;
        string widthOfShuttering = width;

        var existingItem = _rentWindow.Items.FirstOrDefault(i => i.NameOfShuttering == nameOfShuttering && i.LengthOfShuttering == int.Parse(lengthOfShuttering) && i.WidthOfShuttering == int.Parse(widthOfShuttering));

        if (existingItem != null)
        {
            existingItem.AmountOfShuttering += amount;
            existingItem.TotalPriceOfShuttering = existingItem.AmountOfShuttering * existingItem.PriceOfShuttering;
            _rentWindow.AddedObjectDataGrid.Items.Refresh();
            _rentWindow.UpdateTotalValue();
        }
        else
        {
            var item = new ItemShutteringRent()
            {
                NameOfShuttering = nameOfShuttering,
                LengthOfShuttering = int.Parse(lengthOfShuttering),
                WidthOfShuttering = int.Parse(widthOfShuttering),
                ProductID = _availableAmountItemInfo.ProductID,
                AmountOfShuttering = amount,
                PriceOfShuttering = pricePerUnit,
                TotalPriceOfShuttering = totalCost
            };

            _rentWindow.AddItemToList(item);
        }
        AvaibleAmountOfShutteringTextBlock.Text = $"Pozostała ilość: {_availableAmountItemInfo.AmountInStock}";
        
        Close();
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

            // Znajdujemy informacje o szerokości
            _availableAmountItemInfo = _widths.FirstOrDefault(w => w.Width == selectedWidth);

            // Jeśli znaleźliśmy dane dla wybranej szerokości
            if (_availableAmountItemInfo != null)
            {
                double pricePerUnit = _availableAmountItemInfo.Price;

                // Wyświetlamy dostępne ilości i cenę
                AvaibleAmountOfShutteringTextBlock.Text = $"{_availableAmountItemInfo.AmountInStock}";
                if (pricePerUnit == 0)
                {
                    PriceOfShutteringTextBlock.Text = "Brak ceny";
                }
                else
                {
                    PriceOfShutteringTextBlock.Text = $"{pricePerUnit:F2}";
                }
            }
        }
    }
}
