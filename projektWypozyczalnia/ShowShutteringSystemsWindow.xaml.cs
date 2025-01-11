using System.Windows;

namespace projektWypozyczalnia;

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