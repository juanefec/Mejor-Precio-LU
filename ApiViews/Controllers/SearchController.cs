using System.Collections.Generic;
using mejor_precio_2.Models;
using mejor_precio_2.API;
using mejor_precio_2.Persistance;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System;
using ApiViews.Models;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ApiViews.Controllers
{
    [Route("search")]
    public class SearchController : MejorPrecioControllerBase
    {       
        public IActionResult Index()
        {
            var name = HttpContext.Request.Query["name"];

            var idUser = this.User.FindFirstValue(ClaimTypes.Sid);

            var searchListViewModel = new SearchListViewModel();
            
            if (idUser == null){
                searchListViewModel.SearchList = new List <Search> ();
            }
            else{
                searchListViewModel.SearchList = new SearchHandler().GetSearchRecord(Guid.Parse(idUser));
            }

            searchListViewModel.BarcodeString = name;

            return View(searchListViewModel);
        }

        [HttpGet("byName")]
        public List<ProductInfo> Get(string nombre){
            return new ProductHandler().GetAllByName(nombre);
        }
        [HttpGet("fullproduct/{id}")]
        public Product getFullProduct(Guid id)
        {
            var searchHandler = new SearchHandler();
            if(this.User.Identity.IsAuthenticated){
                var idUser = this.User.FindFirstValue(ClaimTypes.Sid);
                searchHandler.CreateSearch(id, new Guid(idUser));
            }
            return searchHandler.GetFullProduct(id);

        }

        [HttpPost]
        public IActionResult DecodeBarcode([FromForm]SearchListViewModel file)
        {
            var barcode = new ProductHandler().GetBarcodeFromImage(file.BarcodeImage);

            var name = new ProductHandler().GetNameByBarcode(barcode);

            return RedirectToAction("Index", new { name = name });
        }
    }
}

