using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopApp.BusinessLayer.Abstract;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }


        public IActionResult Index()
        {
            //Model üzerinden göndercez bu çok dopru değil
            //return View(_productService.GetAll());

            return View(new ProductListModel()
            {
                Products = _productService.GetAll()
            });

        }
    }
}