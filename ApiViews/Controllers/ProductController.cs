using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiViews.Models;
using mejor_precio_2.API;
using mejor_precio_2.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace ApiViews.Controllers
{
    [Authorize (Roles = "Admin, Moderador")]
    public class ProductController : MejorPrecioControllerBase
    {
        public IActionResult Index()
        {
            var productListViewModel = new ProductListViewModel();
            productListViewModel.ProductList = new ProductHandler().GetAll();

            return View(productListViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]ProductViewModel product)
        {
            var extension = "";
            if(product.Image.ContentType == "image/jpeg"){
                extension = ".jpg";
            }else if(product.Image.ContentType == "image/png"){
                extension = ".png";
            }else{
                throw new Exception("No se puede otra cosa que no sea png o jpg");
            }

            var guidName = Guid.NewGuid().ToString().Replace("-","");

            var filePath = Path.Combine("wwwroot","Photos","Products",guidName+extension);
            using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                await product.Image.CopyToAsync(fileStream);
            }
            product.Route = guidName+extension;

            new ProductHandler().Create(product);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(Guid id)
        {
            var product = new ProductHandler().GetOneById(id);

            var productViewModel = new ProductViewModel(product);

            return View(productViewModel);
        }

        [HttpPost]
        public IActionResult Update([FromForm]ProductViewModel product)
        {
            new ProductHandler().Update(product);
            
            return RedirectToAction("Index");
        }

        public IActionResult Delete(Guid id)
        {
            new ProductHandler().Delete(id);
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
