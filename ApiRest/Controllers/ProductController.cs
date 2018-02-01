using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using mejor_precio_2.Models;
using mejor_precio_2.API;
using System.Data.SqlClient;
using System.Data;
using ZXing;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using mejor_precio_2.Persistance;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace mejor_precio_2.ApiRest.Controllers
{
    [Authorize(AuthenticationSchemes=CustomAuthHandler.Name)]
    [Route("products")]
    public class ProductController : Controller
    {
        // GET 
        [HttpGet]
        public List<ProductInfo> Get()
        {
            return new ProductHandler().GetAll();
        }

        [HttpGet("search")]
        public List<ProductInfo> Get(string nombre)
        {
            return new ProductHandler().GetAllByName(nombre);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Product Get(Guid id)
        {
            return new SearchHandler().GetFullProduct(id);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Product productItem)
        {
            try
            {
                new ProductHandler().Create(productItem);
                return StatusCode(200);

            }
            catch (AccessViolationException)
            {
                return StatusCode(401, "No posee la autorización adecuada.");
            }
        }

        // PUT
        [HttpPut]
        public IActionResult Put([FromBody]Product productItem)
        {
            try
            {
                new ProductHandler().Update(productItem);
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(401,"No posee la autorización adecuada.");
            }
        }

        // DELETE
        [HttpDelete]
        public IActionResult Delete([FromBody]Guid id)
        {
            try
            {
                new ProductHandler().Delete(id);
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(401,"No posee la autorización adecuada.");
            }
        }

        [HttpPost("searchbarcode")]
        public string DecodeBarcode(IFormFile file)
        {
            return new ProductHandler().GetBarcodeFromImage(file);
        }        
    }
}