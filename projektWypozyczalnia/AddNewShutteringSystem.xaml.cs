using System.Windows;

namespace projektWypozyczalnia;

public partial class AddNewShutteringSystem : Window
{
    public AddNewShutteringSystem()
    {
        InitializeComponent();
    }

    private void Cancel_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void AddShutteringSystemButton_OnClick(object sender, RoutedEventArgs e)
    {
        DBUtility dbUtility = new DBUtility();
        
        try
        {
            string nameOfShuttering = NameOfShutteringTextBox.Text.ToLower();
            string manufacturer = ManufacturerTextBox.Text;
            int length = int.Parse(LengthTextBox.Text);
            string widthsInput = WidthTextBox.Text;
            var widths = widthsInput.Split(' ').Where(w => !string.IsNullOrWhiteSpace(w)).Select(w => int.Parse(w))
                .ToList();
            
            if (string.IsNullOrWhiteSpace(nameOfShuttering) && string.IsNullOrWhiteSpace(manufacturer))
            {
                MessageBox.Show("Nazwa systemu szalunkowego i producent nie mogą być puste.");
                return;
            }
            else if (string.IsNullOrWhiteSpace(nameOfShuttering))
            {
                MessageBox.Show("Pole \"nazwa systemu szalunkowego\" nie może być pusta.");
                return;
            }
            else
            {
                MessageBox.Show("Pole \"producent\" nie może być puste.");
                return;
            }
            
                
            dbUtility.AddShutteringSystemToDatabase(nameOfShuttering!, manufacturer, length, widths);
            
            MessageBox.Show($"Dodano: {nameOfShuttering}; {manufacturer}; {length}; {widthsInput}");

            MessageBox.Show("Pomyślnie dodano do bazy");
            this.Close();
        }

        catch (FormatException)
        {
            MessageBox.Show("Proszę wprowadzić poprawne dane liczbowe");
        }

        catch (Exception ex)
        {
            MessageBox.Show("Wystąpił błąd: " + ex.Message);
            this.Close();
        }
    }

    private void ShowShutteringSystems_OnClick(object sender, RoutedEventArgs e)
    {
        ShowShutteringSystemsWindow showShutteringSystemsWindow = new ShowShutteringSystemsWindow();
        showShutteringSystemsWindow.ShowDialog();
    }
}