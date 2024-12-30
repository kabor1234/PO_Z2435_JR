using System.Windows;
using System.Windows.Controls;

namespace projektWypozyczalnia;

public partial class AddShutteringToStockWindow : Window
{
    private DBUtility dbUtility = new DBUtility();

    public AddShutteringToStockWindow()
    {
        InitializeComponent();
        Loaded += OnWindowLoaded;
    }

    private void OnWindowLoaded(object sender, RoutedEventArgs e)
    {
        LoadShutteringSystems();
    }

    private void LoadShutteringSystems()
    {
        shutteringComboBox.Items.Clear();
        List<string> systems = dbUtility.GetShutteringSystems();

        foreach (var system in systems)
        {
            shutteringComboBox.Items.Add(system);
        }
    }

    private void shutteringComboBox_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
    {
        string selectedSystem = (string)shutteringComboBox.SelectedItem;
        LoadLengthsForSystem(selectedSystem);
    }

    private void LoadLengthsForSystem(string systemName)
    {
        lengthComboBox.Items.Clear();
        List<int> lengths = dbUtility.GetLengthsForSystem(systemName);

        foreach (var length in lengths)
        {
            lengthComboBox.Items.Add(length);
        }
    }

    private void lengthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        int selectedLength = (int)lengthComboBox.SelectedItem;
        LoadWidthsForLength(selectedLength);
    }

    private void LoadWidthsForLength(int length)
    {
        widthComboBox.Items.Clear();
        List<int> widths = dbUtility.GetWidthsForLength(length);

        foreach (var width in widths)
        {
            widthComboBox.Items.Add(width);
        }
    }

    private void AddShutteringsToStock_onClick(object sender, RoutedEventArgs e)
    {
        string selectedSystem = (string)shutteringComboBox.SelectedItem;
        int selectedLength = (int)lengthComboBox.SelectedItem;
        int selectedWidth = (int)widthComboBox.SelectedItem;
        int amount = int.Parse(amountTextBox.Text);

        dbUtility.AddToStock(selectedSystem, selectedLength, selectedWidth, amount);
    }

    private void AddNewShutteringSystem_OnClick(object sender, RoutedEventArgs e)
    {
        AddNewShutteringSystem addNewShutteringSystem = new AddNewShutteringSystem();
        addNewShutteringSystem.Show();
    }

    private void Cancel_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
    
}