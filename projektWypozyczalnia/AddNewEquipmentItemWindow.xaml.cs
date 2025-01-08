using System.Windows;

namespace projektWypozyczalnia;

public partial class AddNewEquipmentItemWindow : Window
{
    private DBUtility dbUtility = new DBUtility();
    public event EventHandler EquipmentAdded;
    public AddNewEquipmentItemWindow()
    {
        InitializeComponent();
    }

    private void ShowEquipmentItems_OnClick(object sender, RoutedEventArgs e)
    {
        ShowAllEquipmentItems window = new ShowAllEquipmentItems();
        window.Show();
    }

    private void Cancel_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void AddNewEquipmentItemToDB_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            string nameOfEquipment = EquipmentNameTextBox.Text.ToString();

            if (string.IsNullOrWhiteSpace(nameOfEquipment))
            {
                MessageBox.Show("Nazwa osprzętu nie może być pusta.");
                return;
            }

            dbUtility.AddEquipmentToDatabase(nameOfEquipment);
                
            MessageBox.Show($"Dodano osprzęt: {nameOfEquipment}");
            EquipmentAdded?.Invoke(this, EventArgs.Empty);
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Wystąpił błąd: " + ex.Message);
        }
    }
}