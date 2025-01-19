using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using projektWypozyczalnia.Classes;

namespace projektWypozyczalnia.RentWindows;

public partial class RentWindow : Window
{
    public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
    public WidthForLength AvailableAmountItemInfo { get; set; }

    public RentWindow()
    {
        InitializeComponent();
        Items = new ObservableCollection<Item>();
        DataContext = this;
        Loaded += RentWindow_Loaded;
    }

    private void RentWindow_Loaded(object sender, RoutedEventArgs e)
    {
        AddedObjectDataGrid.ItemsSource = Items;
    }

    public void AddItemToList(Item item)
    {
        Items.Add(item);
        UpdateTotalValue();
    }


    private void StartRent_OnClick(object sender, RoutedEventArgs e)
    {

        //zrobić metodę, która będzie wyrzucała błąd jak będą złe wartości / puste pola
        //Checking null textboxes

        //Client column is null?
        if (NameOfCompanyTextBox.Text == "")
            MessageBox.Show("Pole \"nazwa firmy\" nie może być puste.");
        else
        {
            string nameOfCompany = NameOfCompanyTextBox.Text;
        }
        if (NumberOfCompanyTextBox.Text == "")
            MessageBox.Show("Pole \"NIP\" nie może być puste.");
        if (AddressOfCompanyTextBox.Text == "")
            MessageBox.Show("Pole \"Adres\" nie może być puste.");
        if (PostNumberTextBox.Text == "")
            MessageBox.Show("Pole \"Kod pocztowy\" nie może być puste.");
        if (NameOfPostEstablishmentTextBox.Text == "")
            MessageBox.Show("Pole \"Poczta\" nie może być puste.");

        //Contact person column is null?
        if (ContactPersonNameTextBox.Text == "")
            MessageBox.Show("Pole \"Imię\" nie może być puste.");
        if (ContactPersonSurnameTextBox.Text == "")
            MessageBox.Show("Pole \"Nazwisko\" nie może być puste.");
        if (ContactPersonEmailTextBox.Text == "")
            MessageBox.Show("Pole \"E-mail\" nie może być puste.");
        if (ContactPersonPhoneNumberTextBox.Text == "")
            MessageBox.Show("Pole \"Numer telefonu\" nie może być puste.");

        //Calendar is null?
        var selectedDate = DateSelectionsCalendar.SelectedDates;
        if (selectedDate.Count() <= 0)
            MessageBox.Show("Brak wybranego zakresu dat w kalendarzu.");
        else
        {
            DateTime startDate = selectedDate.Min();
            DateTime endDate = selectedDate.Max();
            
            string startDateString = startDate.ToString("dd/MM/yyyy");
            string endDateString = endDate.ToString("dd/MM/yyyy");
        }

        //Checking information in textboxes are correct
        if (NumberOfCompanyTextBox.Text.Length != 10 || !NumberOfCompanyTextBox.Text.All(char.IsDigit))
        {
            MessageBox.Show("Podano błędny NIP");
        }
        else
        {
            string numberOfCompany = NumberOfCompanyTextBox.Text;
        }
        if (!(PostNumberTextBox.Text.Length == 6 && PostNumberTextBox.Text[2] == '-' &&
              PostNumberTextBox.Text.Substring(0, 2).All(Char.IsDigit) &&
              PostNumberTextBox.Text.Substring(3).All(Char.IsDigit)) && PostNumberTextBox.Text != "")
            MessageBox.Show("Błędny kod pocztowy");
        else
        {
            string postNumber = PostNumberTextBox.Text;
        }
        if (NameOfPostEstablishmentTextBox.Text.All(Char.IsDigit) && NameOfCompanyTextBox.Text != "")
            MessageBox.Show("Błąd w polu \"Poczta\" ");
        else
        {
            string nameOfEstablishment = NameOfPostEstablishmentTextBox.Text;
        }
        
        Close();
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

    private void UpdateTotalValue()
    {
        double totalValue = 0;

        foreach (var item in AddedObjectDataGrid.Items)
        {
            if (item is Item currentItem)
            {
                totalValue += currentItem.TotalPrice;
            }
        }

        TotalValueOfEquipmentTextBlock.Text = $"{totalValue:F2} zł";
    }


    private void AddShutterings_OnClick(object sender, RoutedEventArgs e)
    {
        AddShutteringToRentWindow addShutterings = new AddShutteringToRentWindow(this, AvailableAmountItemInfo);
        addShutterings.ShowDialog();
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
            Close();
    }

    private void ChooseContractorFromAvaible_OnClick(object sender, RoutedEventArgs e)
    {

    }

    private void RentWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        
    }
}