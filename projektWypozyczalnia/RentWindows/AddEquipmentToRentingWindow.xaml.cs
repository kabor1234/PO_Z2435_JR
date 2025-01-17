using System.Windows;
using projektWypozyczalnia.Classes;

namespace projektWypozyczalnia.RentWindows;

public partial class AddEquipmentToRentingWindow : Window
{
    private Dictionary<int, string> _equipmentDictionary = new();
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
            .OrderBy(name => name, StringComparer.CurrentCulture);
            
        foreach (var equipmentName in sortedEquipment)
        {
            EquipmentNameComboBox.Items.Add(equipmentName);
        }
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

    private void AddEquipmentToRent_onClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void Cancel_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}