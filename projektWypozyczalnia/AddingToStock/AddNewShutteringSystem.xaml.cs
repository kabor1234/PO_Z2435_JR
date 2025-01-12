using System.ComponentModel;
using System.Windows;
using projektWypozyczalnia.Classes;
using projektWypozyczalnia.ShowWindows;

namespace projektWypozyczalnia.AddingToStock;

public partial class AddNewShutteringSystem : Window
{
    public AddNewShutteringSystem()
    {
        InitializeComponent();
    }

    private void Cancel_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void AddShutteringSystemButton_OnClick(object sender, RoutedEventArgs e)
    {
        
        
        try
        {
            string nameOfShuttering = NameOfShutteringTextBox.Text.ToLower();
            string manufacturer = ManufacturerTextBox.Text;
            int length = int.Parse(LengthTextBox.Text);
            if(length <= 0) throw new FormatException();
            string widthsInput = WidthTextBox.Text;
            
            var widths = new List<int>();
            foreach (var width in widthsInput.Split(' '))
            {
                if (!string.IsNullOrWhiteSpace(width))
                {
                    var temp = int.Parse(width);
                    if (temp <= 0) throw new FormatException();
                    widths.Add(temp);
                }
            }
            
            if (ShowErrorMessage(nameOfShuttering, manufacturer)) return;


            DBUtility.AddShutteringSystemToDatabase(nameOfShuttering!, manufacturer, length, widths);
            
            MessageBox.Show($"Dodano: {nameOfShuttering}; {manufacturer}; {length}; {widthsInput}");

            MessageBox.Show("Pomyślnie dodano do bazy");
            Close();
        }

        catch (FormatException)
        {
            MessageBox.Show("Proszę wprowadzić poprawne dane liczbowe");
        }

        catch (Exception ex)
        {
            MessageBox.Show("Wystąpił błąd: " + ex.Message);
            Close();
        }
    }

    private static bool ShowErrorMessage(string nameOfShuttering, string manufacturer)
    {
        if (string.IsNullOrWhiteSpace(nameOfShuttering) && string.IsNullOrWhiteSpace(manufacturer))
        {
            MessageBox.Show("Nazwa systemu szalunkowego i producent nie mogą być puste.");
            return true;
        }
        if (string.IsNullOrWhiteSpace(nameOfShuttering))
        {
            MessageBox.Show("Pole \"nazwa systemu szalunkowego\" nie może być pusta.");
            return true;
        }
        if (string.IsNullOrWhiteSpace(manufacturer))
        {
            MessageBox.Show("Pole \"producent\" nie może być puste.");
            return true;
        }

        return false;
    }

    private void ShowShutteringSystems_OnClick(object sender, RoutedEventArgs e)
    {
        ShowShutteringSystemsWindow showShutteringSystemsWindow = new ShowShutteringSystemsWindow();
        showShutteringSystemsWindow.ShowDialog();
    }

    private void AddNewShutteringSystem_Closing(object? sender, CancelEventArgs cancelEventArgs)
    {
        AddShutteringToStockWindow newWindow = new AddShutteringToStockWindow();
        newWindow.Show();

    }
}