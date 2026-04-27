namespace ProductStorageEF;

public class Product
{
    public long Id { get; set; }
    public required string Article { get; set; }
    public required string Name { get; set; }
    public required string Manufacturer { get; set; }
    public double Price { get; set; }
    public long Stock { get; set; }
}