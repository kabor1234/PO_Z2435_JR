using System.Windows;

namespace projektWypozyczalnia;

public partial class AddEquipmentToStockWindow : Window
{
    public AddEquipmentToStockWindow()
    {
        InitializeComponent();
    }

    private void AddNewEquipmentItem_OnClick(object sender, RoutedEventArgs e)
    {
        AddNewEquipmentItem addNewEquipmentItem = new AddNewEquipmentItem();
        addNewEquipmentItem.Show();
    }

    private void Cancel_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void AddEquipmentToStock_onClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}