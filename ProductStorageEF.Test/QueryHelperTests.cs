using Microsoft.Data.Sqlite;
using ProductStorageEF.Core.Model;

namespace ProductStorageEF.Test;

public class QueryHelperTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ProductsContext _context;
    private readonly QueryHelper _helper;

    public QueryHelperTests()
    {
        var connString = $"DataSource=file:test{Guid.NewGuid():N}?mode=memory&cache=shared";
        _connection = new SqliteConnection(connString);
        _connection.Open();
        
        _context = new ProductsContext(connString);
        _helper = new QueryHelper(_context);
    }
    
    public void Dispose()
    {
        _connection.Close();
        _context.Dispose();
    }

    private static Product CreateSampleProduct(int id, string article = "A100",
        string name = "Sample", string manufacturer = "M1", double price = 99.99,
        int stock = 10) =>
        new Product
        {
            Id = id,
            Article = article,
            Name = name,
            Manufacturer = manufacturer,
            Price = price,
            Stock = stock
        };

    // AddProducts
    [Fact]
    public void AddProducts_ValidProducts_ReturnsTrue()
    {
        var products = new List<Product> { CreateSampleProduct(1) };
        var result = _helper.AddProducts(products);
        Assert.True(result);
        Assert.Single(_helper.GetAllProducts());
    }

    [Fact]
    public void AddProducts_EmptyList_ReturnsFalse()
    {
        var result = _helper.AddProducts([]);
        Assert.False(result);
        Assert.Empty(_helper.GetAllProducts());
    }

    // GetAllProducts
    [Fact]
    public void GetAllProducts_AfterAdd_ReturnsAll()
    {
        _helper.AddProducts([CreateSampleProduct(1), CreateSampleProduct(2, "A200")]);
        var all = _helper.GetAllProducts().ToList();
        Assert.Equal(2, all.Count);
        Assert.Contains(all, p => p.Article == "A100");
        Assert.Contains(all, p => p.Article == "A200");
    }

    [Fact]
    public void GetAllProducts_Empty_ReturnsEmpty()
    {
        Assert.Empty(_helper.GetAllProducts());
    }

    // GetProductsByPriceLesserOrEqual
    [Theory]
    [InlineData(50.0, 0)]
    [InlineData(99.99, 1)]
    [InlineData(100.0, 2)]
    public void GetProductsByPriceLesserOrEqual_ReturnsCorrectProducts(
        double limit, int expectedCount)
    {
        _helper.AddProducts([
            CreateSampleProduct(1, price: 99.99),
            CreateSampleProduct(2, "A200", price: 100.0)
        ]);

        var result = _helper.GetProductsByPriceLesserOrEqual(limit);
        Assert.Equal(expectedCount, result.Count());
    }

    // GetProductsByPriceGreaterOrEqual
    [Theory]
    [InlineData(100.01, 0)]
    [InlineData(100.0, 1)]
    [InlineData(99.99, 2)]
    public void GetProductsByPriceGreaterOrEqual_ShouldReturnCorrectProducts(
        double limit, int expectedCount)
    {
        _helper.AddProducts([
            CreateSampleProduct(1, price: 99.99),
            CreateSampleProduct(2, "A200", price: 100.0)
        ]);

        var result = _helper.GetProductsByPriceGreaterOrEqual(limit);
        Assert.Equal(expectedCount, result.Count());
    }

    // GetProductsByName
    [Fact]
    public void GetProductsByName_ExactMatch_ReturnsMatching()
    {
        _helper.AddProducts([
            CreateSampleProduct(1, name: "Item1"),
            CreateSampleProduct(2, "A200", name: "Item2"),
            CreateSampleProduct(3, "A300", name: "Item1")
        ]);
        var result = _helper.GetProductsByName("Item1").ToList();
        Assert.Equal(2, result.Count);
        Assert.All(result, p => Assert.Equal("Item1", p.Name));
    }

    [Fact]
    public void GetProductsByName_NoMatch_ReturnsEmpty()
    {
        _helper.AddProducts([CreateSampleProduct(1, name: "X")]);
        Assert.Empty(_helper.GetProductsByName("Y"));
    }

    // GetProductsByStockLesserOrEqual
    [Fact]
    public void GetProductsByStockLesserOrEqual_ReturnsCorrect()
    {
        _helper.AddProducts([
            CreateSampleProduct(1, stock: 5),
            CreateSampleProduct(2, "A200", stock: 10),
            CreateSampleProduct(3, "A300", stock: 15)
        ]);
        var result = _helper.GetProductsByStockLesserOrEqual(10).ToList();
        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.Stock == 5);
        Assert.Contains(result, p => p.Stock == 10);
    }

    // GetProductsByStockGreaterOrEqual
    [Fact]
    public void GetProductsByStockGreaterOrEqual_ShouldReturnCorrect()
    {
        _helper.AddProducts([
            CreateSampleProduct(1, stock: 5),
            CreateSampleProduct(2, "A200", stock: 10),
            CreateSampleProduct(3, "A300", stock: 15)
        ]);
        var result = _helper.GetProductsByStockGreaterOrEqual(10).ToList();
        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.Stock == 10);
        Assert.Contains(result, p => p.Stock == 15);
    }

    // UpdateProducts
    [Fact]
    public void UpdateProducts_ModifiesExistingProducts()
    {
        var product = CreateSampleProduct(1, name: "Old");
        _helper.AddProducts([product]);

        var toUpdate = _helper.GetAllProducts().First();
        toUpdate.Name = "New";
        toUpdate.Price = 50.0;

        var updated = _helper.UpdateProducts([toUpdate]);
        Assert.True(updated);

        var refreshed = _helper.GetAllProducts().First();
        Assert.Equal("New", refreshed.Name);
        Assert.Equal(50.0, refreshed.Price);
    }

    [Fact]
    public void UpdateProducts_EmptyList_ReturnsFalse()
    {
        var result = _helper.UpdateProducts([]);
        Assert.False(result);
    }

    // DeleteProducts
    [Fact]
    public void DeleteProducts_SetsIsDeletedTrue()
    {
        var p1 = CreateSampleProduct(1);
        var p2 = CreateSampleProduct(2, "A200");
        _helper.AddProducts([p1, p2]);

        var result = _helper.DeleteProducts([p1]);
        Assert.True(result);

        var all = _helper.GetAllProducts().ToList();
        Assert.Equal(2, all.Count);
        var deleted = all.Single(p => p.Id == 1);
        Assert.True(deleted.IsDeleted);
        var notDeleted = all.Single(p => p.Id == 2);
        Assert.False(notDeleted.IsDeleted);
    }

    [Fact]
    public void DeleteProducts_EmptyList_ReturnsFalse()
    {
        var result = _helper.DeleteProducts([]);
        Assert.False(result);
    }
}