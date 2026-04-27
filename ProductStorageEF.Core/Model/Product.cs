namespace ProductStorageEF.Model;

public class Product
{
    public int Id { get; set; }
    public required string Article { get; set; }
    public required string Name { get; set; }
    public required string Manufacturer { get; set; }
    public required double Price { get; set; }
    public required int Stock { get; set; }
    public bool IsDeleted { get; set; } = false;

    public override string ToString()
    {
        return !IsDeleted
            ? $"#{Id}: {Article}, {Name}, {Manufacturer}, {Price} р., {Stock} шт."
            : $"#{Id}: Товар удален.";
    }
}