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
            string nameOfShuttering = NameOfShutteringTextBox.Text;
            string manufacturer = ManufacturerTextBox.Text;
            int length = int.Parse(LengthTextBox.Text);
            string widthsInput = WidthTextBox.Text;
            var widths = widthsInput.Split(' ').Where(w => !string.IsNullOrWhiteSpace(w)).Select(w => int.Parse(w))
                .ToList();
            
            
            MessageBox.Show(widthsInput);
            
            dbUtility.AddShutteringSystemToDatabase(nameOfShuttering, manufacturer, length, widths);

            MessageBox.Show("Pomyślnie dodano do bazy");
        }

        catch (FormatException)
        {
            MessageBox.Show("Proszę wprowadzić poprawne dane liczbowe");
        }

        catch (Exception ex)
        {
            MessageBox.Show("Wystąpił błąd: " + ex.Message);
        }
    }
}