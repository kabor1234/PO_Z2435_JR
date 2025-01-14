using System.Windows;

namespace projektWypozyczalnia;

public partial class RentWindow : Window
{
    public RentWindow()
    {
        InitializeComponent();

        TotalValueOfEquipment.Text = "156 903,43 zł";
    }
    
    private void CommentsTextBox_GotFocused(object sender, RoutedEventArgs e)
    {
        
        if (CommentsTextBox.Text == "Uwagi do wynajmu...")
        {
            CommentsTextBox.Text = string.Empty;
            CommentsTextBox.Foreground = System.Windows.Media.Brushes.Black;
        }
    }

    private void CommentsTexbox_LostFocused(object sender, RoutedEventArgs e)
    {

        if (string.IsNullOrWhiteSpace(CommentsTextBox.Text))
        {
            CommentsTextBox.Text = "Uwagi do wynajmu...";
            CommentsTextBox.Foreground = System.Windows.Media.Brushes.Gray;
        }
    }

    private void Rent_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void AddItems_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void Cancel_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}