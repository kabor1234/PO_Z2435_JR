namespace projektWypozyczalnia;

public class Lending
{
    public int LendID { get; set; }
    public int ClientID { get; set; }
    public string StartLendDate { get; set; }
    public string EndLendDate { get; set; }
    public string Comments { get; set; }

    public Lending(int lendID, int clientID, string startLendDate, string endLendDate, string comments)
    {
        LendID = lendID;
        ClientID = clientID;
        StartLendDate = startLendDate;
        EndLendDate = endLendDate;
        Comments = comments;
    }
}