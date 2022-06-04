using APS_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace APS_MVC.Controllers
{
    public class CatalogController : Controller
    {
        private static Catalog _catalog = new();

        [HttpGet]
        public IActionResult Products()
        {
            return View(_catalog);
        }

        public IActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddProduct(Product model)
        {
            _catalog.Products.Add(model);
            return View(_catalog);
        }
    }
}
