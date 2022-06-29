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
}
