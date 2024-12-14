using System.Windows;

namespace projektWypozyczalnia;

public partial class AddShutteringToStockWindow : Window
{
    public AddShutteringToStockWindow()
    {
        InitializeComponent();
    }

    private void StartupAddShutteringToStockWindow(object sender, RoutedEventArgs e)
    {
        if (AddShutteringToStockComboBox.Items.Count > 0)
        {
            AddShutteringToStockComboBox.SelectedIndex = 0;
        }
    }
}