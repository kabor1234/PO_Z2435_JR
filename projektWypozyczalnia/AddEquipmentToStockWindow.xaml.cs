using System.ComponentModel;
using System.Windows;
using System.Globalization;


namespace projektWypozyczalnia;

public partial class AddEquipmentToStockWindow : Window
{
        
        private Dictionary<int, string> equipmentDictionary = new Dictionary<int, string>();

        public AddEquipmentToStockWindow()
        {
            InitializeComponent();
            LoadEquipment();
        }
        
        private void AddEquipmentToStock_onClick(object sender, RoutedEventArgs e)
        {
            if (EquipmentNameComboBox.SelectedItem == null)
            {
                MessageBox.Show("Proszę wybrać osprzęt.");
                return;
            }
            

            string selectedEquipmentName = (string)EquipmentNameComboBox.SelectedItem;
            int selectedEquipmentId = GetEquipmentIdByName(selectedEquipmentName);

            if (!int.TryParse(AmountOfEquipmentTextBox.Text, out int amount))
            {
                MessageBox.Show("Wprowadź poprawną liczbę dla ilości.");
                return;
            }

            DBUtility.AddEquipmentToStock(selectedEquipmentId, amount);
            
            Close();
        }
        
        private void AddNewEquipment_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
            AddNewEquipmentItemWindow addNewEquipmentWindow = new AddNewEquipmentItemWindow();
            addNewEquipmentWindow.ShowDialog();
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

        
        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
}