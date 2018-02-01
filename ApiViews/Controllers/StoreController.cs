using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiViews.Models;
using mejor_precio_2.API;
using mejor_precio_2.Models;

namespace ApiViews.Controllers
{
    public class StoreController : MejorPrecioControllerBase
    {
        public IActionResult Index()
        {
            var storeListViewModel = new StoreListViewModel();
            storeListViewModel.StoreList = new StoreHandler().GetAll();

            return View(storeListViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromForm]StoreViewModel store)
        {
            try{
                new StoreHandler().Create(store);
            }
            catch{
                this.ModelState.AddModelError("ubicationInvalid", "La ubicación debe estar dentro de Capital Federal.");
                return View(store);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(Guid id)
        {
            var store = new StoreHandler().GetOneById(id);

            var storeViewModel = new StoreViewModel(store);

            return View(storeViewModel);
        }

        [HttpPost]
        public IActionResult Update([FromForm]StoreViewModel store)
        {
            try{
                new StoreHandler().Update(store);
            }
            catch{
                this.ModelState.AddModelError("ubicationInvalid", "La ubicación debe estar dentro de Capital Federal.");
                return View(store);
            }
            
            return RedirectToAction("Index");
        }

        public IActionResult Delete(Guid id)
        {
            new StoreHandler().Delete(id);
            
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
