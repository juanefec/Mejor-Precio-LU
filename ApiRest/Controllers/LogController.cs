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
    
    
    public class LogController : Controller
    {
        // GET 
        [HttpGet]
        public Object Get()
        {
            return new {};
        }               
    }
}