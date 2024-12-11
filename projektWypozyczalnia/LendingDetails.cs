namespace projektWypozyczalnia;

public class LendingDetails
{
    public int DetailsID { get; set; }
    public int LendingID { get; set; }
    public int ProductID { get; set; }
    public int EquipmentID { get; set; }
    public int Amount { get; set; }

    public LendingDetails(int detailsID, int lendingID, int productID, int equipmentID, int amount)
    {
        DetailsID = detailsID;
        LendingID = lendingID;
        ProductID = productID;
        EquipmentID = equipmentID;
        Amount = amount;
    }
}