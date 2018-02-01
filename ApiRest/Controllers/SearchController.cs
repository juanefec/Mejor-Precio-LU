using System.Collections.Generic;
using mejor_precio_2.Models;
using mejor_precio_2.API;
using mejor_precio_2.Persistance;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System;

namespace mejor_precio_2.ApiRest.Controllers
{
    [Route("search")]
    public class SearchController : Controller
    {       
        [HttpGet]
        public List<ProductInfo> Get(string search)
        {
            return new ProductHandler().GetAllByName(search);
        }

        [HttpGet("fullproduct/{id}")]
        public Product getFullProduct(Guid id)
        {
            return new SearchHandler().GetFullProduct(id);
        }

        [HttpGet("historial/{idUser}")]
        public List<Search> GetSearchRecord(Guid idUser)
        {
            return new SearchHandler().GetSearchRecord(idUser);
        }
    }
}

