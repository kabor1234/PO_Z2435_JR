namespace projektWypozyczalnia.Classes;

public class LendShutteringDetails
{
    public int LendingShutteringDetailID { get; set; }
    public int LendingShutteringID { get; set; }
    public int ProductID { get; set; }
    public int AmountOfShutterings { get; set; }
    public double TotalPriceOfShuttering { get; set; }
    public string ContactPersonName { get; set; }
    public string ContactPersonSurname { get; set; }
    public string ContactPersonPhoneNumber { get; set; }
    public string ContactPersonEmail { get; set; }

    public LendShutteringDetails(int lendingShutteringDetailId, int lendingShutteringId, int productId, int amountOfShutterings, 
                                    double totalPriceOfShuttering, string contactPersonName, string contactPersonSurname, string contactPersonPhoneNumber, string contactPersonEmail)
    {
        LendingShutteringDetailID = lendingShutteringDetailId;
        LendingShutteringID = lendingShutteringId;
        ProductID = productId;
        AmountOfShutterings = amountOfShutterings;
        TotalPriceOfShuttering = totalPriceOfShuttering;
        ContactPersonName = contactPersonName;
        ContactPersonSurname = contactPersonSurname;
        ContactPersonPhoneNumber = contactPersonPhoneNumber;
        ContactPersonEmail = contactPersonEmail;
    }
}