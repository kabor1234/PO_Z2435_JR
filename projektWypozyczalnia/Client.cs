namespace projektWypozyczalnia;

public class Client
{
    public int ClientID { get; set; }
    public string NameOfCompany { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

    public Client(int clientID, string nameOfCompany, string name, string surname, string address, string phoneNumber,
        string email)
    {
        ClientID = clientID;
        NameOfCompany = nameOfCompany;
        Name = name;
        Surname = surname;
        Address = address;
        PhoneNumber = phoneNumber;
        Email = email;
    }

}