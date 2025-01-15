namespace projektWypozyczalnia;

public class Client
{
    private string NameOfCompany { get; set; }
    private string NIP { get; set; }
    private string Address { get; set; }
    private string PostNumber { get; set; }
    private string NameOfPostEstablishment { get; set; }

    public Client(string nameOfCompany, string nip, string address, string postNumber, string nameOfPostEstablishment)
    {
        NameOfCompany = nameOfCompany;
        NIP = nip;
        Address = address;
        PostNumber = postNumber;
        NameOfPostEstablishment = nameOfPostEstablishment;
    }
}