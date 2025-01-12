using System.ComponentModel;
using System.Windows;
using projektWypozyczalnia.Classes;
using projektWypozyczalnia.ShowWindows;

namespace projektWypozyczalnia.AddingToStock;

public partial class AddNewEquipmentItemWindow : Window
{
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
        Close();
    }

    private void AddNewEquipmentItemToDB_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            string nameOfEquipment = EquipmentNameTextBox.Text;

            if (string.IsNullOrWhiteSpace(nameOfEquipment))
            {
                MessageBox.Show("Nazwa osprzętu nie może być pusta.");
                return;
            }
            
            nameOfEquipment = nameOfEquipment.ToLower();
            
            DBUtility.AddEquipmentToDatabase(nameOfEquipment);
                
            MessageBox.Show($"Dodano osprzęt: {nameOfEquipment}");
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Wystąpił błąd: " + ex.Message);
        }
    }

    private void AddNewEquipmentItemWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        AddEquipmentToStockWindow newWindow = new AddEquipmentToStockWindow();
        newWindow.Show();
    }
}