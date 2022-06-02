using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIForCardsApplication
{
    public class Card
    {
        public string Id { get; set; } 
        public string Title {get; set;}

        public string ImageName { get; set; }

    }
}
