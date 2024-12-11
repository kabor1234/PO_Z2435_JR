namespace projektWypozyczalnia;

public class LengthCategory
{
    public int LengthID { get; set; }
    public int SystemID { get; set; }
    public int Length { get; set; }

    public LengthCategory(int lengthID, int systemID, int length)
    {
        LengthID = lengthID;
        SystemID = systemID;
        Length = length;
    }
    
}