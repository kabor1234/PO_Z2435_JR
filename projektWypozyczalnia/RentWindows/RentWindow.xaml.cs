using System.Windows;

namespace projektWypozyczalnia.RentWindows;

public partial class RentWindow : Window
{
    public RentWindow()
    {
        InitializeComponent();

        TotalValueOfEquipment.Text = "156 903,43 zł";
    }
    
    
    private void Rent_OnClick(object sender, RoutedEventArgs e)
    {
        
        //zrobić metodę, która będzie wyrzucała błąd jak będą złe wartości / puste pola
        //Checking null textboxes
        
        //Client column is null?
        if (NameOfCompanyTextBox.Text == "")
            MessageBox.Show("Pole \"nazwa firmy\" nie może być puste.");
        if(NumberOfCompanyTextBox.Text == "")
            MessageBox.Show("Pole \"NIP\" nie może być puste.");
        if(AddressOfCompanyTextBox.Text == "")
            MessageBox.Show("Pole \"Adres\" nie może być puste.");
        if(PostNumberTextBox.Text == "")
            MessageBox.Show("Pole \"Kod pocztowy\" nie może być puste.");
        if(NameOfPostEstablishmentTextBox.Text == "")
            MessageBox.Show("Pole \"Poczta\" nie może być puste.");
        
        //Contact person column is null?
        if(ContactPersonNameTextBox.Text == "")
            MessageBox.Show("Pole \"Imię\" nie może być puste.");
        if(ContactPersonSurnameTextBox.Text == "")
            MessageBox.Show("Pole \"Nazwisko\" nie może być puste.");
        if(ContactPersonEmailTextBox.Text == "")
            MessageBox.Show("Pole \"E-mail\" nie może być puste.");
        if(ContactPersonPhoneNumberTextBox.Text == "")
            MessageBox.Show("Pole \"Numer telefonu\" nie może być puste.");
        
        //Calendar is null?
        var selectedDate = DateSelectionsCalendar.SelectedDates;
        if( selectedDate.Count() <= 0 )
            MessageBox.Show("Brak wybranego zakresu dat w kalendarzu.");
        
        //Checking information in textboxes are correct
        if(!int.TryParse(NumberOfCompanyTextBox.Text, out int numberOfCompany) && NumberOfCompanyTextBox.Text.Length != 10 && NumberOfCompanyTextBox.Text != "")
            MessageBox.Show("Podano błędny NIP");
        if(!(PostNumberTextBox.Text.Length == 6 && PostNumberTextBox.Text[2] == '-' && PostNumberTextBox.Text.Substring(0,2).All(Char.IsDigit) && PostNumberTextBox.Text.Substring(3).All(Char.IsDigit))  && PostNumberTextBox.Text != "")
            MessageBox.Show("Błędny kod pocztowy");
        if(NameOfPostEstablishmentTextBox.Text.All(Char.IsDigit) && NameOfCompanyTextBox.Text !=  "")
            MessageBox.Show("Błąd w polu \"Poczta\" ");
    }
    
    
    private void CommentsTextBox_GotFocused(object sender, RoutedEventArgs e)
    {
        
        if (CommentsTextBox.Text == "Uwagi do wynajmu...")
        {
            CommentsTextBox.Text = string.Empty;
            CommentsTextBox.Foreground = System.Windows.Media.Brushes.Black;
        }
    }

    private void CommentsTexbox_LostFocused(object sender, RoutedEventArgs e)
    {

        if (string.IsNullOrWhiteSpace(CommentsTextBox.Text))
        {
            CommentsTextBox.Text = "Uwagi do wynajmu...";
            CommentsTextBox.Foreground = System.Windows.Media.Brushes.Gray;
        }
    }
    

    private void AddItems_OnClick(object sender, RoutedEventArgs e)
    {
        ChoosingAddingItemWindow choosingAddingItemWindow = new ChoosingAddingItemWindow();
        choosingAddingItemWindow.ShowDialog();
    }

    private void Cancel_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ChooseContractorFromAvaible_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}