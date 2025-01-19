namespace projektWypozyczalnia.Classes;

public class LendingDetail
{
    public int DetailsID { get; set; }  // publiczne właściwości
    public int ContactPersonID { get; set; }
    public int LendingID { get; set; }
    public int ProductID { get; set; }
    public int EquipmentID { get; set; }
    public int Amount { get; set; }
    public decimal SummaryCostOfObject { get; set; }  // Dodanie pola dla ceny
    public string ContactPersonName { get; set; }  // publiczne właściwości
    public string ContactPersonSurname { get; set; }
    public string ContactPersonPhoneNumber { get; set; }
    public string ContactPersonEmail { get; set; }
    public string EquipmentName { get; set; }  // Dodanie pola dla nazwy sprzętu
    public string ProductName { get; set; }  // Nazwa szalunku
    public int ProductLength { get; set; }  // Długość szalunku
    public int ProductWidth { get; set; }  // Szerokość szalunku
    public int ProductStock { get; set; }  // Ilość szalunków na stanie
    public decimal ProductPrice { get; set; }  // Cena szalunku
    public string? Description { get; set; }  // Opcjonalny opis
    
}