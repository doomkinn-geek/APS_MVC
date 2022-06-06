using System.ComponentModel.DataAnnotations;

namespace APS_MVC.Models
{
    public class CategoryModel
    {
        public string Price;
        public List<Category> Categories { get; set; } = new();
        public int CategoryId { get; set; }
        [StringLength(20)]
        public string Name { get;set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
