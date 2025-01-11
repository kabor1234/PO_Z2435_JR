using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace projektWypozyczalnia;

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

    private void AddShutteringsToStock_onClick(object sender, RoutedEventArgs e)
    {
        
        string selectedSystem = (string)ShutteringComboBox.SelectedItem;
        int selectedLength = (int)LengthComboBox.SelectedItem;
        int selectedWidth = (int)WidthComboBox.SelectedItem;
        int amount = int.Parse(AmountTextBox.Text);
        
        DBUtility.AddShutteringToStock(selectedSystem, selectedLength, selectedWidth, amount);
        
        Close();

    }

    private void AddNewShutteringSystem_OnClick(object sender, RoutedEventArgs e)
    {
        
        AddNewShutteringSystem addNewShutteringSystem = new AddNewShutteringSystem();
        addNewShutteringSystem.Show();
        
        Close();
    }

    private void Cancel_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void ClosingWindow(object? sender, CancelEventArgs e)
    {
        StockConditionWindow newWindow = new StockConditionWindow();
        newWindow.Show();
    }
}

