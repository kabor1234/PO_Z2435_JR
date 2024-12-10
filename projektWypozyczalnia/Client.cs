namespace projektWypozyczalnia;

public class Client
{
    int ClientID { get; set; }
    string NameOfCompany { get; set; }
    string Name { get; set; }
    string Surname { get; set; }
    string Address { get; set; }
    string PhoneNumber { get; set; }
    string Email { get; set; }

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