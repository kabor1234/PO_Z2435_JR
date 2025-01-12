using System.Windows;
using projektWypozyczalnia.Classes;

namespace projektWypozyczalnia.ShowWindows;

public partial class ShowShutteringSystemsWindow : Window
{
    public ShowShutteringSystemsWindow()
    {
        InitializeComponent();
        ShowShutteringSystemsMethod();
    }
    private void ShowShutteringSystemsMethod()
    {
        try
        {
            var data = DBUtility.GetAllShutteringSystems();
            ActiveShutteringSystemsDg.ItemsSource = data;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Wystąpił błąd podczas ładowania danych: " + ex.Message);
        }
    }
}