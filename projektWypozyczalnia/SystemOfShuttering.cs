namespace projektWypozyczalnia;

public class SystemOfShuttering
{
    public int SystemID { get; set; }
    public string NameOfShuttering { get; set; }
    public string Manufacturer { get; set; }
    public string Summary { get; set; }

    public SystemOfShuttering(int systemID, string nameOfShuttering, string manufacturer, string summary)
    {
        SystemID = systemID;
        NameOfShuttering = nameOfShuttering;
        Manufacturer = manufacturer;
        Summary = summary;
    }
}