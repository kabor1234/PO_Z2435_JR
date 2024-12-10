namespace projektWypozyczalnia;

public class SaleDetails
{
    public int SalesDetailsID { get; set; }
    public int SalesID { get; set; }
    public int ProductID { get; set; }
    public int EquipmentID { get; set; }
    public int Amount { get; set; }
    public float PricePerUnit { get; set; }

    public SaleDetails(int salesDetailsID, int salesID, int productID, int equipmentID, int amount, float pricePerUnit)
    {
        SalesDetailsID = salesDetailsID;
        SalesID = salesID;
        ProductID = productID;
        EquipmentID = equipmentID;
        Amount = amount;
        PricePerUnit = pricePerUnit;
    }
}