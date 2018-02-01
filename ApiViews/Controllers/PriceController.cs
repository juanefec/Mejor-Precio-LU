using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiViews.Models;
using mejor_precio_2.API;
using mejor_precio_2.Models;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ApiViews.Controllers
{
    public class PriceController : MejorPrecioControllerBase
    {
        public IActionResult Index()
        {
            var priceListViewModel = new PriceListViewModel();
            priceListViewModel.PriceDataList = new PriceHandler().GetPriceDataList();

            return View(priceListViewModel);
        }

        
        public IActionResult Create()
        {
            var priceListViewModel = new PriceDataViewModel();
            
            priceListViewModel.ProductList = new ProductHandler().GetAll();
            priceListViewModel.StoreList = new StoreHandler().GetAll();

            return View(priceListViewModel);
        }

        [HttpPost]
        public IActionResult Create(PriceDataViewModel precio)
        {
            var priceData = new NewPriceData();
            priceData.IdProduct = precio.IdProduct;
            priceData.IdStore = precio.IdStore;
            priceData.Cost = precio.Cost;
            priceData.Report = "Pendiente";
            priceData.IdUser = Guid.Parse(this.User.FindFirstValue(ClaimTypes.Sid));

            new PriceHandler().Create(priceData);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Update(Guid id)
        {
            var priceInfo = new PriceHandler().GetOneById(id);

            var priceInfoViewModel = new PriceInfoViewModel(priceInfo);

            return View(priceInfoViewModel);
        }

        [HttpPost]
        public IActionResult Update(Price precio)
        {
            new PriceHandler().Update(precio);
            if(precio.Report == "Aceptado"){
                new PriceHandler().SetStateZeroToOldPrices(precio);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
