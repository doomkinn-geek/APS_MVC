﻿using APS_MVC.Models;
using APS_MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace APS_MVC.Controllers
{
    public class CatalogController : Controller
    {        
        private static SafeCatalog _catalog = new();
        private static INotificationSender _notification;

        public CatalogController(INotificationSender notification)
        {
            _notification = notification;
        }

        [HttpGet]
        public IActionResult Products()
        {
            return View(_catalog);
        }

        public IActionResult AddProduct()
        {
            return View();
        }
        public IActionResult RemoveProduct()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product, CancellationToken cancellationToken)
        {
            if (product != null)
            {
                _catalog.Products.Add(product);                
                await _notification.SendEmailAsync(product, cancellationToken);
            }
            return View(_catalog);
        }
        [HttpPost]
        public IActionResult RemoveProduct(Product product)
        {
            if(product != null)
            {
                _catalog.Products.Remove(product);
            }
            return View(_catalog);
        }
    }
}
