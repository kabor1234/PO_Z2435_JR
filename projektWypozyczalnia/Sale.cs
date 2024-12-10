namespace projektWypozyczalnia;

public class Sale
{
    public int SalesID { get; set; }
    public int ClientID { get; set; }
    public string SaleDate { get; set; }
    public string Comments { get; set; }

    public Sale(int salesID, int clientID, string saleDate, string comments)
    {
        SalesID = salesID;
        ClientID = clientID;
        SaleDate = saleDate;
        Comments = comments;
    }
}