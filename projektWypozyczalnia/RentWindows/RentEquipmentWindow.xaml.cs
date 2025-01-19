using System.Windows;

namespace projektWypozyczalnia.RentWindows;

public partial class RentEquipmentWindow : Window
{
    public RentEquipmentWindow()
    {
        InitializeComponent();
    }

    private void StartRentEquipment_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
    
    private void CommentsEquipmentTextBox_GotFocused(object sender, RoutedEventArgs e)
    {

        if (CommentsTextBox.Text == "Uwagi do wynajmu...")
        {
            CommentsTextBox.Text = string.Empty;
            CommentsTextBox.Foreground = System.Windows.Media.Brushes.Black;
        }
    }

    private void CommentsEquipmentTexbox_LostFocused(object sender, RoutedEventArgs e)
    {

        if (string.IsNullOrWhiteSpace(CommentsTextBox.Text))
        {
            CommentsTextBox.Text = "Uwagi do wynajmu...";
            CommentsTextBox.Foreground = System.Windows.Media.Brushes.Gray;
        }
    }

    private void AddEquipent_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ChooseContractorFromAvaible_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}