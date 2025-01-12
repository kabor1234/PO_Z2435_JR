using System.ComponentModel;
using System.Windows;
using System.Globalization;

namespace projektWypozyczalnia;

public partial class EditEquipmentPrice : Window
{
    
    private Dictionary<int, string> equipmentDictionary = new Dictionary<int, string>();
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
        foreach (var equipment in equipmentDictionary)
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
        equipmentDictionary = DBUtility.GetEquipmentList();
            
        var sortedEquipment = equipmentDictionary.Values
            .OrderBy(equipment => equipment, StringComparer.Create(CultureInfo.GetCultureInfo("pl-PL"), false))
            .ToList();

        foreach (var equipment in equipmentDictionary)
        {
            EquipmentNameComboBox.Items.Add(equipment.Value);
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