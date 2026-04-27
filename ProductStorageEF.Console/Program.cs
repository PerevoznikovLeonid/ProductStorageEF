using ProductStorageEF.Core.Model;

const string connectionString = @"Data Source=C:\Users\COLLEGE\RiderProjects\ProductStorageEF\ProductStorageEF.Console\products.db;";

var context = new ProductsContext(connectionString);
var db = new QueryHelper(context);

var productList = new List<Product>
{
    new Product
    {
        Article = "283039023",
        Name = "Футболка",
        Manufacturer = "T-Shirt Company",
        Price = 1000,
        Stock = 100
    },
    new Product
    {
        Article = "491205871",
        Name = "Джинсы",
        Manufacturer = "Levi's",
        Price = 3500,
        Stock = 45
    },
    new Product
    {
        Article = "672309124",
        Name = "Кроссовки",
        Manufacturer = "Nike",
        Price = 5200,
        Stock = 28
    },
    new Product
    {
        Article = "158432690",
        Name = "Свитшот",
        Manufacturer = "Adidas",
        Price = 2800,
        Stock = 73
    },
    new Product
    {
        Article = "935027481",
        Name = "Кепка",
        Manufacturer = "New Era",
        Price = 1200,
        Stock = 110
    }
};

db.AddProducts(productList);

Console.WriteLine();

db.GetAllProducts()
    .ToList()
    .ForEach(Console.WriteLine);

Console.WriteLine();

db.GetProductsByPriceLesserOrEqual(1000)
    .ToList()
    .ForEach(Console.WriteLine);

Console.WriteLine();

db.GetProductsByPriceGreaterOrEqual(1000)
    .ToList()
    .ForEach(Console.WriteLine);

Console.WriteLine();

db.GetProductsByName("Кроссовки")
    .ToList()
    .ForEach(Console.WriteLine);

Console.WriteLine();