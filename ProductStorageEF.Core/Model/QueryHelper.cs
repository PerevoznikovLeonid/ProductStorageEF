using ProductStorageEF.Model;

namespace ProductStorageEF.Core.Model;

public class QueryHelper
{
    private readonly ProductsContext _db;

    public QueryHelper(ProductsContext productsContext)
    {
        _db = productsContext;
        _db.Database.EnsureCreated();
    }
    
    public bool AddProducts(IEnumerable<Product> products)
    {
        _db.Products.AddRange(products.ToList());
        var result = _db.SaveChanges();
        return result > 0;
    }
    
    public IEnumerable<Product> GetAllProducts()
    {
        return _db.Products.ToList();
    }
    
    public IEnumerable<Product> GetProductsByPriceLesserOrEqual(double price)
    {
        return _db.Products.Where(p => p.Price <= price);
    }
    
    public IEnumerable<Product> GetProductsByPriceGreaterOrEqual(double price)
    {
        return _db.Products.Where(p => p.Price <= price);
    }
    
    public IEnumerable<Product> GetProductsByName(string name)
    {
        return _db.Products.Where(p => p.Name == name);
    }
    
    public IEnumerable<Product> GetProductsByStockLesserOrEqual(int stock)
    {
        return _db.Products.Where(p => p.Stock <= stock);
    }
    
    public IEnumerable<Product> GetProductsByStockGreaterOrEqual(int stock)
    {
        return _db.Products.Where(p => p.Stock <= stock);
    }
    
    public bool UpdateProducts(IEnumerable<Product> products)
    {
        _db.Products.UpdateRange(products.ToList());
        var result = _db.SaveChanges();
        return result > 0;
    }
    
    public bool DeleteProducts(IEnumerable<Product> products)
    {
        foreach (var product in products)
        {
            product.IsDeleted = true;
        }
        var result = _db.SaveChanges();
        return result > 0;
    }
}