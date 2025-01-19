namespace projektWypozyczalnia.Classes;

public class Client
{
    public int ClientID { get; set; }
    public string NameOfCompany { get; set; }
    public string NIP { get; set; }
    public string Address { get; set; }
    public string PostNumber { get; set; }
    public string NameOfPostEstablishment { get; set; }

    public string DisplayName => $"{NameOfCompany} - {NIP}";

    public Client(int clientID, string nameOfCompany, string nip, string address, string postNumber, string nameOfPostEstablishment)
    {
        ClientID = clientID;
        NameOfCompany = nameOfCompany;
        NIP = nip;
        Address = address;
        PostNumber = postNumber;
        NameOfPostEstablishment = nameOfPostEstablishment;
    }
}