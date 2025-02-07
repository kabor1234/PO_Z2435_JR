using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using projektWypozyczalnia.Classes;

namespace projektWypozyczalnia.RentWindows;

public partial class RentShutteringsWindow : Window
{
    public ObservableCollection<ItemShutteringRent> Items { get; set; } = new ObservableCollection<ItemShutteringRent>();
    public WidthForLength AvailableAmountItemInfo { get; set; }

    public RentShutteringsWindow()
    {
        InitializeComponent();
        Items = new ObservableCollection<ItemShutteringRent>();
        DataContext = this;
        Loaded += RentWindow_Loaded;
    }

    private void RentWindow_Loaded(object sender, RoutedEventArgs e)
    {
        AddedObjectDataGrid.ItemsSource = Items;
    }
    public void AddItemToList(ItemShutteringRent item)
    {
        Items.Add(item);
        UpdateTotalValue();
    }
    private void StartRentShutterings_OnClick(object sender, RoutedEventArgs e)
    {
        string nameOfCompany = "";
        string numberOfCompany = "";
        string addressOfCompany = "";
        string postNumber = "";
        string nameOfEstablishment = "";
        string startDateString = "";
        string endDateString = "";
        string addressOfBuilding = "";
        string comments = "";
        
        string contactPersonName = "";
        string contactPersonSurname = "";
        string contactPersonPhoneNumber = "";
        string contactPersonEmail = "";
        
        
        //zrobić metodę, która będzie wyrzucała błąd jak będą złe wartości / puste pola
        //Checking null textboxes

        //Client column is null?
        if (NameOfCompanyTextBox.Text == "")
        {
            MessageBox.Show("Pole \"nazwa firmy\" nie może być puste.");
            return;
        }
        
        if (NumberOfCompanyTextBox.Text == "")
        {
            MessageBox.Show("Pole \"NIP\" nie może być puste.");
            return;
        }

        if (AddressOfCompanyTextBox.Text == "")
        {
            MessageBox.Show("Pole \"Adres\" nie może być puste.");
            return;
        }

        if (PostNumberTextBox.Text == "")
        {
            MessageBox.Show("Pole \"Kod pocztowy\" nie może być puste.");
            return;
        }

        if (NameOfPostEstablishmentTextBox.Text == "")
        {
            MessageBox.Show("Pole \"Poczta\" nie może być puste.");
            return;
        }

        if (AddressOfBuildingTextBox.Text == "")
        {
            MessageBox.Show("Pole \"Adres budowy\" nie może być puste.");
            return;
        }

        if (ContactPersonNameTextBox.Text == "")
        {
            MessageBox.Show("Pole \"Imię\" nie może być puste.");
            return;
        }

        if (ContactPersonSurnameTextBox.Text == "")
        {
            MessageBox.Show("Pole \"Nazwisko\" nie może być puste.");
            return;
        }

        if (ContactPersonEmailTextBox.Text == "")
        {
            MessageBox.Show("Pole \"E-mail\" nie może być puste.");
            return;
        }

        if (ContactPersonPhoneNumberTextBox.Text == "")
        {
            MessageBox.Show("Pole \"Numer telefonu\" nie może być puste.");
            return;
        }
        
        if (NumberOfCompanyTextBox.Text.Length != 10 || !NumberOfCompanyTextBox.Text.All(char.IsDigit))
        {
            MessageBox.Show("Podano błędny NIP");
        }
        else
        {
            numberOfCompany = NumberOfCompanyTextBox.Text;
        }
        if (!(PostNumberTextBox.Text.Length == 6 && PostNumberTextBox.Text[2] == '-' &&
              PostNumberTextBox.Text.Substring(0, 2).All(Char.IsDigit) &&
              PostNumberTextBox.Text.Substring(3).All(Char.IsDigit)) && PostNumberTextBox.Text != "")
            MessageBox.Show("Błędny kod pocztowy");
        else
        {
            postNumber = PostNumberTextBox.Text;
        }
        if (NameOfPostEstablishmentTextBox.Text.All(Char.IsDigit) && NameOfCompanyTextBox.Text != "")
            MessageBox.Show("Błąd w polu \"Poczta\" ");
        else
        {
            nameOfEstablishment = NameOfPostEstablishmentTextBox.Text;
        }

        nameOfCompany = NameOfCompanyTextBox.Text;
        numberOfCompany = NumberOfCompanyTextBox.Text;
        addressOfCompany = AddressOfCompanyTextBox.Text;
        postNumber = PostNumberTextBox.Text;
        nameOfEstablishment = NameOfPostEstablishmentTextBox.Text;
        addressOfBuilding = AddressOfBuildingTextBox.Text;
        
        contactPersonName = ContactPersonNameTextBox.Text;
        contactPersonSurname = ContactPersonSurnameTextBox.Text;
        contactPersonPhoneNumber = ContactPersonPhoneNumberTextBox.Text;
        contactPersonEmail = ContactPersonEmailTextBox.Text;
        
        // Sprawdzenie kalendarza
        var selectedDate = DateSelectionsCalendar.SelectedDates;
        if (selectedDate.Count <= 0)
        {
            MessageBox.Show("Brak wybranego zakresu dat w kalendarzu.");
            return;
        }
        
        DateTime startDate = selectedDate.Min();
        DateTime endDate = selectedDate.Max();

        startDateString = startDate.ToString("dd/MM/yyyy");
        endDateString = endDate.ToString("dd/MM/yyyy");
        
        comments = CommentsTextBox.Text;
        
        if (comments == "Uwagi do wynajmu...")
            comments = null;
        
        
            //Add ClientToDatabase
        
            DBUtility.AddClientToDatabase(nameOfCompany, numberOfCompany, addressOfCompany, postNumber,
                nameOfEstablishment);
            Client currentClient = DBUtility.GetClientByNumber(numberOfCompany);
            int clientID = currentClient.ClientID;
            
            //Add Lendings
            DBUtility.AddNewLending(clientID, startDateString, endDateString, addressOfBuilding, comments);
            
            List<RentalItems> allLendings = DBUtility.GetAllLendings();
            int lastLendingID = allLendings.LastOrDefault()?.LendID ?? 0;

            if (lastLendingID > 0)
            {
                MessageBox.Show("Pobrało lastLendID");

                // Dodaj szczegóły wypożyczenia szalunków
                DBUtility.AddShutteringLendingDetails(lastLendingID, Items, 
                    contactPersonName, contactPersonSurname, contactPersonEmail, contactPersonPhoneNumber);
        
                MessageBox.Show("Za dodaniem do bazy danych do shutteringdetails");
            }
            else
            {
                MessageBox.Show("Brak ostatniego wynajmu do dodania szczegółów.");
            }

            Close();
    }
    private void CommentsShutteringsTextBox_GotFocused(object sender, RoutedEventArgs e)
    {

        if (CommentsTextBox.Text == "Uwagi do wynajmu...")
        {
            CommentsTextBox.Text = string.Empty;
            CommentsTextBox.Foreground = System.Windows.Media.Brushes.Black;
        }
    }
    private void CommentsShutteringsTexbox_LostFocused(object sender, RoutedEventArgs e)
    {

        if (string.IsNullOrWhiteSpace(CommentsTextBox.Text))
        {
            CommentsTextBox.Text = "Uwagi do wynajmu...";
            CommentsTextBox.Foreground = System.Windows.Media.Brushes.Gray;
        }
    }
    public void UpdateTotalValue()
    {
        double totalValue = 0;

        foreach (var item in AddedObjectDataGrid.Items)
        {
            if (item is ItemShutteringRent currentItem)
            {
                totalValue += currentItem.TotalPriceOfShuttering;
            }
        }

        TotalValueOfEquipmentTextBlock.Text = $"{totalValue:F2} zł";
    }
    private void AddShutterings_OnClick(object sender, RoutedEventArgs e)
    {
        AddShutteringToRentWindow addShutterings = new AddShutteringToRentWindow(this, AvailableAmountItemInfo);
        addShutterings.ShowDialog();
    }
    private void ChooseContractorFromAvaible_OnClick(object sender, RoutedEventArgs e)
    {
        ChooseClientForShutteringsWindow chooseClientForShutteringsWindow = new ChooseClientForShutteringsWindow();
        bool? dialogResult = chooseClientForShutteringsWindow.ShowDialog();
        
        if (dialogResult == true)
        {
            Client selectedClient = chooseClientForShutteringsWindow.GetSelectedClient();
            
            if (selectedClient != null)
            {
                NameOfCompanyTextBox.Text = selectedClient.NameOfCompany;
                NumberOfCompanyTextBox.Text = selectedClient.NIP;
                AddressOfCompanyTextBox.Text = selectedClient.Address;
                PostNumberTextBox.Text = selectedClient.PostNumber;
                NameOfPostEstablishmentTextBox.Text = selectedClient.NameOfPostEstablishment;
            }
        }
    }
    private void RentWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        
    }
    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}