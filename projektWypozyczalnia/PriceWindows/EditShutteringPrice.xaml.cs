using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using projektWypozyczalnia.Classes;
using projektWypozyczalnia.ShowWindows;

namespace projektWypozyczalnia.Pricewindows;

public partial class EditShutteringPrice : Window
{
    public EditShutteringPrice()
    {
        InitializeComponent();
        LoadShutteringSystems();
    }
    
    private void LoadShutteringSystems()
    {
        ShutteringComboBox.Items.Clear();
        List<string> systems = DBUtility.GetShutteringSystems();

        foreach (var system in systems)
        {
            ShutteringComboBox.Items.Add(system);
        }
    }

    private void shutteringComboBox_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
    {
        string selectedSystem = (string)ShutteringComboBox.SelectedItem;
        LoadLengthsForSystem(selectedSystem);
    }

    private void LoadLengthsForSystem(string systemName)
    {
        LengthComboBox.Items.Clear();
        List<int> lengths = DBUtility.GetLengthsForSystem(systemName);

        foreach (var length in lengths)
        {
            LengthComboBox.Items.Add(length);
        }
    }

    private void lengthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        int selectedLength = (int)LengthComboBox.SelectedItem;
        LoadWidthsForLength(selectedLength);
    }

    private void LoadWidthsForLength(int length)
    {
        WidthComboBox.Items.Clear();
        List<int> widths = DBUtility.GetWidthsForLength(length);

        foreach (var width in widths)
        {
            WidthComboBox.Items.Add(width);
        }
    }

    private void EditPriceOfShutteringProduct_OnClick(object sender, RoutedEventArgs e)
    {
        if (!float.TryParse(NewPriceTextBox.Text, out float newPrice) || newPrice < 0)
        {
            MessageBox.Show("Cena musi być liczbą większą od 0.");
        }
        else
        {
            string selectedSystem = (string)ShutteringComboBox.SelectedItem;
            int selectedLength = (int)LengthComboBox.SelectedItem;
            int selectedWidth = (int)WidthComboBox.SelectedItem;
            
            DBUtility.UpdateShutteringPrice(selectedSystem, selectedLength, selectedWidth, newPrice);

            Close();
        }
    }
    

    private void Cancel_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void EditShutteringPriceWindow_onClosing(object? sender, CancelEventArgs e)
    {
        PriceListWindow priceListWindow = new PriceListWindow();    
        priceListWindow.ShowDialog();
    }
}