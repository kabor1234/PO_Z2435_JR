namespace projektWypozyczalnia;

public class PriceList
{
    public int PriceID { get; set; }
    public int? ProductID { get; set; }
    public int? EquipmentID { get; set; }
    public float Price { get; set; }

    public PriceList(int priceID, int productID, int equipmentID, float price)
    {
        PriceID = priceID;
        ProductID = productID;
        EquipmentID = equipmentID;
        Price = price;
    }
}