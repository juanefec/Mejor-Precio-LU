using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiViews.Models;
using System.Security.Claims;
using mejor_precio_2.Models;
using mejor_precio_2.API;

namespace ApiViews.Controllers
{
    public class HomeController : MejorPrecioControllerBase
    {

        public IActionResult Index()
        {
            var name = HttpContext.Request.Query["name"];

            var idUser = this.User.FindFirstValue(ClaimTypes.Sid);

           var searchListViewModel = LoadSearchList(idUser,name);

            return View(searchListViewModel);
        }

        public SearchListViewModel LoadSearchList(string idUser, string name){


            var searchListViewModel = new SearchListViewModel();
            
            if (idUser == null){
                searchListViewModel.SearchList = new List <Search> ();
            }
            else{
                searchListViewModel.SearchList = new SearchHandler().GetSearchRecord(Guid.Parse(idUser));
            }

            searchListViewModel.BarcodeString = name;

            return searchListViewModel;
        }

        public IActionResult GmapsSVGMarker()
        {     
            

            return base.File( "~/Photos/GmapsIconpng.png","image/png");
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult DecodeBarcode([FromForm]SearchListViewModel file)
        {
            try{
                var barcode = new ProductHandler().GetBarcodeFromImage(file.BarcodeImage);

                var name = new ProductHandler().GetNameByBarcode(barcode);
                return RedirectToAction("Index", new { name = name });
            } 
            catch{
                this.ModelState.AddModelError("InvalidImage", "No ha seleccionado una imagen o es inválida");
                var name = HttpContext.Request.Query["name"];

                var idUser = this.User.FindFirstValue(ClaimTypes.Sid);

                var searchListViewModel = LoadSearchList(idUser,name);
                return View("Index", searchListViewModel);
            }

        }
    }
}
