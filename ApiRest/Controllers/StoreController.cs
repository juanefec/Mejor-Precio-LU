using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using mejor_precio_2.Models;
using mejor_precio_2.API;
using mejor_precio_2.Persistance;
using System.Data.SqlClient;
using System.Data;

namespace mejor_precio_2.ApiRest.Controllers
{
    [Route("stores")]
    public class StoreController : Controller
    {      
        // GET 
        [HttpGet]
        public List<Store> Get()
        {
            return new StoreHandler().GetAll();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Store Get(Guid id)
        {   
            return new StoreHandler().GetOneById(id);

        }

        // POST api/values
        [HttpPost]
        public Guid Post([FromBody]Store storeItem)
        {
            return new StoreHandler().Create(storeItem);
        }

        // PUT api/values/5
        [HttpPut]
        public string Put([FromBody]Store storeItem)
        {
           return new StoreHandler().Update(storeItem);
        }

        // DELETE api/values/5
        [HttpDelete]
        public string Delete([FromBody]Guid id)
        {
            return new StoreHandler().Delete(id);
        }
    }
}
