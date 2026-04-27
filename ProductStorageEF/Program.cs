using ProductStorageEF.Model;

const string connectionString = @"Data Source=C:\Users\COLLEGE\RiderProjects\ProductStorageEF\ProductStorageEF\products.db;";

var db = new ProductsContext(connectionString);
db.Database.EnsureCreated();

// Добавление продуктов
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

db.AddRange(productList);
db.SaveChanges();

Console.WriteLine("Получение всех продуктов:");

var allProducts = db.Products.ToList();
foreach (var product in allProducts)
{
    Console.WriteLine(product);
}

Console.WriteLine("Получение продуктов по цене (больше 999 р.):");

var productsByPrice = db.Products.Where(p => p.Price > 999).ToList();
foreach (var product in productsByPrice)
{
    Console.WriteLine(product);
}

Console.WriteLine("Получение продуктов по названию (Кроссовки):");

var productsByName = db.Products.Where(p => p.Name == "Кроссовки").ToList();
foreach (var product in productsByName)
{
    Console.WriteLine(product);
}

Console.WriteLine("Получение продуктов по количеству (больше или равно 50):");

var productsByStock = db.Products.Where(p => p.Stock >= 50).ToList();
foreach (var product in productsByStock)
{
    Console.WriteLine(product);
}

Console.WriteLine("Изменение продукта (первого в таблице):");

var productToUpdate = db.Products.First();
productToUpdate.Price = 600;
db.SaveChanges();
Console.WriteLine(productToUpdate);

Console.WriteLine("Удаление продукта (первого в таблице):");

var productToDelete = db.Products.First();
productToDelete.IsDeleted = true;
db.SaveChanges();
Console.WriteLine(productToDelete);