using System.ComponentModel;
using System.Windows;
using projektWypozyczalnia.ShowWindows;


namespace projektWypozyczalnia;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private void RentalButton_OnClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Działa");
        RentingsWindow rentingsWindow = new RentingsWindow();
        rentingsWindow.Show();
    }
    

    private void StockConditionButton_OnClick(object sender, RoutedEventArgs e)
    {
        StockConditionWindow stockConditionWindow = new StockConditionWindow();
        stockConditionWindow.Show();
    }

    private void PriceListButton_OnClick(object sender, RoutedEventArgs e)
    {
        PriceListWindow priceListWindow = new PriceListWindow();
        priceListWindow.Show();
    }

    private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
    
    private void ExitButton_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
    
    private void MainClosing(object? sender, CancelEventArgs e)
    {
        Application.Current.Shutdown();
    }


}