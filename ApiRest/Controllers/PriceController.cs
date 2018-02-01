using Microsoft.AspNetCore.Mvc;
using mejor_precio_2.Models;
using mejor_precio_2.API;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace mejor_precio_2.ApiRest.Controllers

{
    [Route("prices")]
    public class PriceController : Controller
    {
        [HttpPost]
        public Guid Post([FromBody]JObject data)
        {
                return new PriceHandler().Create(data);
        }

        [HttpPut]
        public IActionResult Put([FromBody]JObject data)
        {
            try
            {
                new PriceHandler().Update(data);
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(401,"No posee la autorización adecuada.");
            }
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]Guid id)
        {
            try
            {
                new PriceHandler().Delete(id);
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(401,"No posee la autorización adecuada.");
            }
        }

        [HttpGet]
        [Route("notifications")]
        public List<Notification> Get()
        {
            return new NotificationHandler().GetAllNotifications();
        }
    }
}