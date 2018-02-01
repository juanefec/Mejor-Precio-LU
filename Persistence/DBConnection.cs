using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace mejor_precio_2.Persistance
{
    public class DBConnection
    {
        public static string conStr = System.IO.File.ReadAllText(System.Environment.GetEnvironmentVariable("ConnectionStringConfig"));
    }
}