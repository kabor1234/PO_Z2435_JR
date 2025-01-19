namespace projektWypozyczalnia.Classes;

public class RentalItems
{
    public int LendID { get; set; }                   
    public string NumberOfLend { get; set; }          
    public string NameOfCompany { get; set; }          
    public bool IsItFinished { get; set; }             
    public int ClientID { get; set; }                  
    public string StartLendDate { get; set; }           
    public string EndLendDate { get; set; }            
    public string AddressOfBuilding { get; set; }       
    
}