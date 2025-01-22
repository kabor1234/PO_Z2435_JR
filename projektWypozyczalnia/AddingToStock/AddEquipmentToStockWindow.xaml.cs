using System.Windows;
using projektWypozyczalnia.Classes;

namespace projektWypozyczalnia.AddingToStock;

public partial class AddEquipmentToStockWindow : Window
{
        
        private Dictionary<int, (string Name, int AmountInStock)> _equipmentDictionary = new();

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

            if (int.TryParse(AmountOfEquipmentTextBox.Text, out int amount) && amount > 0)
            {
                DBUtility.AddEquipmentToStock(selectedEquipmentId, amount);
            
                Close();
                AddEquipmentToStockWindow addEquipmentToStockWindow = new AddEquipmentToStockWindow();
                addEquipmentToStockWindow.Show();
            }
            else 
                MessageBox.Show("Wprowadź poprawną wartość liczbową");



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

        
        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
}