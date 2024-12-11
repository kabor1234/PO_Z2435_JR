namespace projektWypozyczalnia;

public class ShutteringProduct
{
    public int ProductID { get; set; }
    public int LengthID { get; set; }
    public int Width { get; set; }
    public int AmountInStock { get; set; }

    public ShutteringProduct(int productID, int lengthID, int width, int amountInStock)
    {
        ProductID = productID;
        LengthID = lengthID;
        Width = width;
        AmountInStock = amountInStock;
    }
}