using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIForCardsApplication
{
    public class UpdateCardRequest
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string CurrentImage { get; set; }

        public string NewImage { get; set; }
    }
}
