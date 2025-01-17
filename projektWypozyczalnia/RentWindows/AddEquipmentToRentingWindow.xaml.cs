using System.Windows;
using System.Windows.Controls;
using projektWypozyczalnia.Classes;

namespace projektWypozyczalnia.RentWindows;

public partial class AddEquipmentToRentingWindow : Window
{
    private Dictionary<int, (string Name, int AmountInStock)> _equipmentDictionary = new();
    public AddEquipmentToRentingWindow()
    {
        InitializeComponent();
        LoadEquipment();
    }
    private void LoadEquipment()
    {
        EquipmentNameComboBox.Items.Clear();
        _equipmentDictionary = DBUtility.GetEquipmentList();

        var sortedEquipment = _equipmentDictionary.Values
            .OrderBy(e => e.Name, StringComparer.CurrentCulture);

        foreach (var equipment in sortedEquipment)
        {
            EquipmentNameComboBox.Items.Add(equipment.Name);
        }
    }
        
    private int GetEquipmentIdByName(string equipmentName)
    {
        foreach (var equipment in _equipmentDictionary)
        {
            if (equipment.Value.Name == equipmentName)
            {
                return equipment.Key;
            }
        }
        throw new Exception("Nie znaleziono osprzętu o tej nazwie.");
    }
    
    private (string Name, int AmountInStock) GetEquipmentById(int equipmentId)
    {
        if (_equipmentDictionary.TryGetValue(equipmentId, out var equipment))
        {
            return equipment;
        }
        throw new Exception("Nie znaleziono osprzętu o tym ID.");
    }
    private void EquipmentNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
    {
        if (EquipmentNameComboBox.SelectedItem != null)
        {
            string selectedEquipmentName = EquipmentNameComboBox.SelectedItem.ToString();
            int equipmentId = GetEquipmentIdByName(selectedEquipmentName);
            var equipment = GetEquipmentById(equipmentId);
            
            AvaibleAmountOfShuttering.Text = $"{equipment.AmountInStock}";
        }
    }

    private void AddEquipmentToRent_onClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void Cancel_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}