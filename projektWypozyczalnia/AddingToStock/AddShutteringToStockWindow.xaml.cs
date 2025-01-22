using System.Windows;
using System.Windows.Controls;
using projektWypozyczalnia.Classes;

namespace projektWypozyczalnia.AddingToStock;

public partial class AddShutteringToStockWindow : Window
{
    

    public AddShutteringToStockWindow()
    {
        InitializeComponent();
        LoadShutteringSystems();
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

    private void ShutteringComboBox_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
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
        List<WidthForLength> widths = DBUtility.GetWidthsForLength(length);

        foreach (var width in widths)
        {
            WidthComboBox.Items.Add(width.Width);
        }
    }

    private void AddShutteringsToStock_onClick(object sender, RoutedEventArgs e)
    {
        
        string selectedSystem = (string)ShutteringComboBox.SelectedItem;
        int selectedLength = (int)LengthComboBox.SelectedItem;
        int selectedWidth = (int)WidthComboBox.SelectedItem;
        
        if (int.TryParse(AmountTextBox.Text, out int amount) && amount > 0)
        {
            DBUtility.AddShutteringToStock(selectedSystem, selectedLength, selectedWidth, amount);
            Close();
            AddShutteringToStockWindow window = new AddShutteringToStockWindow();
            window.Show();
        }
        else
            MessageBox.Show("Wprowadź poprawną wartość liczbową");
    }

    private void AddNewShutteringSystem_OnClick(object sender, RoutedEventArgs e)
    {
        
        AddNewShutteringSystem addNewShutteringSystem = new AddNewShutteringSystem();
        addNewShutteringSystem.Show();
        Close();
    }

    private void Cancel_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
    
}

