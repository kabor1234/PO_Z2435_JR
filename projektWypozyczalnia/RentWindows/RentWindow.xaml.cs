using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using projektWypozyczalnia.Classes;

namespace projektWypozyczalnia.RentWindows;

public partial class RentWindow : Window
{
    public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
    public bool IsRentConfirmed { get; set; } = false;

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

        //Checking information in textboxes are correct
        if (!int.TryParse(NumberOfCompanyTextBox.Text, out int numberOfCompany) &&
            NumberOfCompanyTextBox.Text.Length != 10 && NumberOfCompanyTextBox.Text != "")
            MessageBox.Show("Podano błędny NIP");
        if (!(PostNumberTextBox.Text.Length == 6 && PostNumberTextBox.Text[2] == '-' &&
              PostNumberTextBox.Text.Substring(0, 2).All(Char.IsDigit) &&
              PostNumberTextBox.Text.Substring(3).All(Char.IsDigit)) && PostNumberTextBox.Text != "")
            MessageBox.Show("Błędny kod pocztowy");
        if (NameOfPostEstablishmentTextBox.Text.All(Char.IsDigit) && NameOfCompanyTextBox.Text != "")
            MessageBox.Show("Błąd w polu \"Poczta\" ");

        IsRentConfirmed = true;
        
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


    private void AddItems_OnClick(object sender, RoutedEventArgs e)
    {
        ChoosingAddingItemWindow choosingAddingItemWindow = new ChoosingAddingItemWindow(this);
        choosingAddingItemWindow.ShowDialog();
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
        // Jeśli użytkownik anulował operację (np. kliknął "Anuluj")
        if (!IsRentConfirmed)
        {
            // Wyświetlamy pytanie, czy użytkownik chce anulować
            if (MessageBox.Show("Czy na pewno chcesz anulować? Zmiany nie będą zapisane.", "Anuluj",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // Usuwamy wszystkie przedmioty z DataGrid (ale nie zmieniamy stanu magazynowego w bazie)
                Items.Clear(); // Usuwamy przedmioty z DataGrid, ale nie z bazy danych
            }
            else
            {
                e.Cancel = true; // Zatrzymujemy zamknięcie okna, jeśli użytkownik nie chce anulować
            }
        }
        else
        {
            // Zatwierdzenie operacji, więc aktualizujemy stan magazynowy
            foreach (var rentedItem in Items) // Zakładamy, że Items to lista obiektów Item
            {
                var selectedWidthInfo = DBUtility.FetchShutteringInfo(rentedItem.Name);  // Pobieramy dane na podstawie nazwy przedmiotu

                if (selectedWidthInfo != null)
                {
                    int newAmountInStock = rentedItem.Amount; // Ilość wypożyczona przez klienta, którą trzeba dodać do magazynu

                    bool updateSuccessful = DBUtility.UpdateWidthStock(
                        selectedWidthInfo.NameOfShutteringSystem,  // Nazwa systemu szalunkowego
                        selectedWidthInfo.Length,                  // Długość
                        selectedWidthInfo.Width,                   // Szerokość
                        newAmountInStock                           // Nowa ilość w magazynie
                    );

                    if (updateSuccessful)
                    {
                        MessageBox.Show($"Stan magazynowy dla {rentedItem.Name} został przywrócony.");
                    }
                    else
                    {
                        MessageBox.Show($"Błąd podczas przywracania stanu magazynowego dla {rentedItem.Name}.");
                    }
                }
            }
        }

    }
}