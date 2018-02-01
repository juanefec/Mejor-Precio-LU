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
    [Route("users")]
    public class UserController : Controller
    {
        // GET api/usuarios
        [HttpGet]
        public List<UserInfo> Get()
        {
            return new UserHandler().GetAll();
        }

        [HttpGet("{id}")]
        public User Get(Guid id)
        {
            return new UserHandler().GetOneById(id);
        }

        // POST users/create
        
        [HttpPost]
        public Guid Post([FromBody]User userItem)
        {
            return new UserHandler().Create(userItem);
        }

        // PUT users/update/3
        [HttpPut]
        public string Put([FromBody]User userItem)
        {
            return new UserHandler().Update(userItem);
        }

        [HttpDelete]
        public string Delete([FromBody]Guid id)
        {
            return new UserHandler().Delete(id);
        }
    }
}
