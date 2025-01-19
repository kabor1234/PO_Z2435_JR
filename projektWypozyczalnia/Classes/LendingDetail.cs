namespace projektWypozyczalnia.Classes;

public class LendingDetail
{
    public string NumberOfLend { get; set; }
    public string ClientName { get; set; }
    public string StartLendDate { get; set; }
    public string EndLendDate { get; set; }
    public string AddressOfBuilding { get; set; }
    public string Comments { get; set; }
    public int isItFinished { get; set; }
}