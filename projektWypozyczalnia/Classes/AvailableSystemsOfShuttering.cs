namespace projektWypozyczalnia.Classes;

public class AvailableSystemsOfShuttering
{
    public string? NameOfShutteringSystem { get; set; }
    public string? Manufacturer { get; set; }
    public int Length { get; set; }
    public int Width { get; set; }

    public AvailableSystemsOfShuttering(string? nameOfShutteringSystem, string? manufacturer, int length, int width)
    {
        NameOfShutteringSystem = nameOfShutteringSystem;
        Manufacturer = manufacturer;
        Length = length;
        Width = width;
    }
    
}