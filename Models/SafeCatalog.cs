using System.Collections;
using System.Collections.Concurrent;

namespace APS_MVC.Models
{
    public record Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Product(int _Id, string _Name)
        {
            Id = _Id;
            Name = _Name;
        }
        public Product()
        {
            Id = 0;
            Name = "";
        }
    }

    public class SafeProductList
    {
        private readonly ConcurrentDictionary<int, Product> _products = new ConcurrentDictionary<int, Product>();
        public int Count => _products.Count;
        public void Add(Product product) => _products.TryAdd(product.Id, product);
        public void Remove(Product product) => _products.TryRemove(product.Id, out _);
        public IReadOnlyCollection<Product> GetAll() => _products.Values.ToArray();        
    }

    public class SafeCatalog
    {
        public SafeProductList Products { get; set; } = new ();
    }
}
