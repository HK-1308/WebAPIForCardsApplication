using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIForCardsApplication
{
    public class UploadNewCardRequest
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
    }
}
