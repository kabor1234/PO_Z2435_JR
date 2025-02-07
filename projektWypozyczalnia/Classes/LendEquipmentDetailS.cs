namespace projektWypozyczalnia.Classes;

public class LendEquipmentDetailS
{
    public int LendingEquipmentDetailsID { get; set; }
    public int LendingID { get; set; }
    public int EquipmentID { get; set; }
    public int Amount { get; set; }
    public double TotalPriceOfEquipment { get; set; }
    public string ContactPersonName { get; set; }
    public string ContactPersonSurname { get; set; }
    public string ContactPersonPhoneNumber { get; set; }
    public string ContactPersonEmail { get; set; }

    public LendEquipmentDetailS(int lendingEquipmentDetailsID, int lendingID, int equipmentID, int amount, double totalPriceOfEquipment, string contactPersonName, string contactPersonSurname, string contactPersonPhoneNumber, string contactPersonEmail)
    {
        LendingEquipmentDetailsID = lendingEquipmentDetailsID;
        LendingID = lendingID;
        EquipmentID = equipmentID;
        Amount = amount;
        TotalPriceOfEquipment = totalPriceOfEquipment;
        ContactPersonName = contactPersonName;
        ContactPersonSurname = contactPersonSurname;
        ContactPersonPhoneNumber = contactPersonPhoneNumber;
        ContactPersonEmail = contactPersonEmail;
    }
}