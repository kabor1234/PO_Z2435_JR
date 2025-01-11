using System.Windows;

namespace projektWypozyczalnia;

public partial class EditPriceWindow : Window
{
    DBUtility dbUtility = new DBUtility();
    public EditPriceWindow()
    {
        InitializeComponent();
    }

}