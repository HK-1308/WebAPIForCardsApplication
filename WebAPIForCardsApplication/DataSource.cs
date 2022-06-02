using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIForCardsApplication
{
    public class DataSource
    {

        private readonly IWebHostEnvironment hostEnvironment;

        private const string FILE_NAME = "CardsData.json";

        public DataSource(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public string DataSourcePath { get { return Path.Combine(hostEnvironment.WebRootPath, FILE_NAME); } }

    }
}
