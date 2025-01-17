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
        RentingsWindow rentingsWindow = new RentingsWindow();
        rentingsWindow.Owner = this;
        rentingsWindow.Closed += (s, args) =>
        {
            OverlayRectangle.Visibility = Visibility.Collapsed;
        };
        OverlayRectangle.Visibility = Visibility.Visible;
        rentingsWindow.Show();
    }
    

    private void StockConditionButton_OnClick(object sender, RoutedEventArgs e)
    {
        StockConditionWindow stockConditionWindow = new StockConditionWindow();
        stockConditionWindow.Owner = this;
        stockConditionWindow.Closed += (s, args) =>
        {
            OverlayRectangle.Visibility = Visibility.Collapsed;
        };
        OverlayRectangle.Visibility = Visibility.Visible;
        stockConditionWindow.Show();
    }

    private void PriceListButton_OnClick(object sender, RoutedEventArgs e)
    {
        PriceListWindow priceListWindow = new PriceListWindow();
        priceListWindow.Owner = this;
        priceListWindow.Closed += (s, args) =>
        {
            OverlayRectangle.Visibility = Visibility.Collapsed;
        };
        OverlayRectangle.Visibility = Visibility.Visible;
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
    
    public void ShowOverlay()
    {
        OverlayRectangle.Visibility = Visibility.Visible;
    }

    public void HideOverlay()
    {
        OverlayRectangle.Visibility = Visibility.Collapsed;
    }


}