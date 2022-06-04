namespace APS_MVC.Models
{
    public class Catalog
    {
        public List<Product> Products { get; set; } = new List<Product>();
    }

    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
