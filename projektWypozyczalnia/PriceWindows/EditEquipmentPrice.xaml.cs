using System.ComponentModel;
using System.Windows;
using projektWypozyczalnia.Classes;
using projektWypozyczalnia.ShowWindows;

namespace projektWypozyczalnia.Pricewindows;

public partial class EditEquipmentPrice : Window
{
    
    private Dictionary<int, string> _equipmentDictionary = new();
    public EditEquipmentPrice()
    {
        InitializeComponent();
        LoadEquipment();
    }

    private void EditPriceOfEquipment_OnClick(object sender, RoutedEventArgs e)
    {
        if (EquipmentNameComboBox.SelectedItem == null)
        {
            MessageBox.Show("Proszę wybrać osprzęt.");
            return;
        }
        
        string selectedEquipmentName = (string)EquipmentNameComboBox.SelectedItem;
        int selectedEquipmentId = GetEquipmentIdByName(selectedEquipmentName);
        
        if (!float.TryParse(NewPriceEquipmentTextBox.Text, out float newPrice) || newPrice <= 0)
        {
            MessageBox.Show("Cena musi być liczbą większą od 0.");
            return;
        }
        
        DBUtility.UpdateEquipmentPrice(selectedEquipmentId, newPrice);
    }
    
    private int GetEquipmentIdByName(string equipmentName)
    {
        foreach (var equipment in _equipmentDictionary)
        {
            if (equipment.Value == equipmentName)
            {
                return equipment.Key;
            }
        }
        throw new Exception("Nie znaleziono osprzętu o tej nazwie.");
    }
    private void LoadEquipment()
    {
        EquipmentNameComboBox.Items.Clear();
        _equipmentDictionary = DBUtility.GetEquipmentList();

        var sortedEquipment = _equipmentDictionary.Values
            .OrderBy(name => name, StringComparer.CurrentCulture);
        
        foreach (var equipmentName in sortedEquipment)
        {
            EquipmentNameComboBox.Items.Add(equipmentName);
        }
    }

    private void Cancel_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void EditEquipmentPriceWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        PriceListWindow priceListWindow = new PriceListWindow();
        priceListWindow.ShowDialog();
    }
}