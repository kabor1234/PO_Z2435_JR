using System.Windows;
using Microsoft.Data.Sqlite;

namespace projektWypozyczalnia;

public partial class StockConditionWindow : Window
{
    private DBUtility dbUtility = new DBUtility();
    private bool showEquipment = false;

    public StockConditionWindow()
    {
        InitializeComponent();
        LoadShutteringData();
    }
    private void AddToStock_OnClick(object sender, RoutedEventArgs e)
    {
        var addShutteringToStockWindow = new AddShutteringToStockWindow();
        addShutteringToStockWindow.Show();
    }
    private void RemoveFromStock_OnClick(object sender, RoutedEventArgs e)
    {
        var removeFromStockWindow = new RemoveFromStockWindow();
        removeFromStockWindow.Show();
    }
    
    private void AddShutteringToStock_OnClick(object sender, RoutedEventArgs e)
    {
        var addShutteringToStock = new AddShutteringToStockWindow();
        addShutteringToStock.Show();
    }
    
    private void AddEquipmentToStock_OnClick(object sender, RoutedEventArgs e)
    {
        var addEquipmentToStockWindow = new AddEquipmentToStockWindow();
        addEquipmentToStockWindow.Show();
    }
    
    private void Close_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
    
    private void ShowEquipmentCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        showEquipment = true;
        LoadData();
    }
    
    private void ShowEquipmentCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        showEquipment = false;
        LoadData();
    }
    
    private void LoadData()
    {
        if (showEquipment)
        {
            LoadEquipmentData();
        }
        else
        {
            LoadShutteringData();
        }
    }
    
    private void LoadShutteringData()
    {
        try
        {
            var data = dbUtility.GetShutteringStockData();
            StockDataGrid.ItemsSource = data;
            
            SetColumnVisibilityForShuttering();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Błąd podczas ładowania danych szalunków: " + ex.Message);
        }
    }
    
    private void LoadEquipmentData()
    {
        try
        {
            var data = dbUtility.GetEquipmentStockData();
            StockDataGrid.ItemsSource = data;
            SetColumnVisibilityForEquipment();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Błąd podczas ładowania danych osprzętu: " + ex.Message);
        }
    }
    
    private void SetColumnVisibilityForShuttering()
    {
        StockDataGrid.Columns[0].Visibility = Visibility.Visible;  // Manufacturer
        StockDataGrid.Columns[1].Visibility = Visibility.Visible;  // Name
        StockDataGrid.Columns[2].Visibility = Visibility.Visible;  // Length
        StockDataGrid.Columns[3].Visibility = Visibility.Visible;  // Width
        StockDataGrid.Columns[4].Visibility = Visibility.Visible;  // Amount

        StockDataGrid.Columns[5].Visibility = Visibility.Collapsed;  // EquipmentName
        StockDataGrid.Columns[6].Visibility = Visibility.Collapsed;  // EquipmentAmount
    }
    
    private void SetColumnVisibilityForEquipment()
    {
        StockDataGrid.Columns[0].Visibility = Visibility.Collapsed;
        StockDataGrid.Columns[1].Visibility = Visibility.Collapsed;
        StockDataGrid.Columns[2].Visibility = Visibility.Collapsed;
        StockDataGrid.Columns[3].Visibility = Visibility.Collapsed;
        StockDataGrid.Columns[4].Visibility = Visibility.Collapsed;

        StockDataGrid.Columns[5].Visibility = Visibility.Visible;
        StockDataGrid.Columns[6].Visibility = Visibility.Visible;
    }
    
    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        var hwndSource = System.Windows.Interop.HwndSource.FromHwnd(new System.Windows.Interop.WindowInteropHelper(this).Handle);
        if (hwndSource != null) hwndSource.AddHook(HwndMessageHook);
    }

    private IntPtr HwndMessageHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        const int WM_SYSCOMMAND = 0x0112;
        const int SC_MOVE = 0xF010;
        if (msg == WM_SYSCOMMAND && (wParam.ToInt32() & 0xFFF0) == SC_MOVE) handled = true;
        return IntPtr.Zero;
    }
}