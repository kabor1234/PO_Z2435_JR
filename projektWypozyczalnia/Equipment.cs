namespace projektWypozyczalnia;

public class Equipment
{
    public int EquipmentID { get; set; }
    public string NameOfEquipment { get; set; }
    public string Summary { get; set; }
    public int AmountInStock { get; set; }

    public Equipment(int equipmentID, string nameOfEquipment, string summary, int amountInStock)
    {
        EquipmentID = equipmentID;
        NameOfEquipment = nameOfEquipment;
        Summary = summary;
        AmountInStock = amountInStock;
    }
}