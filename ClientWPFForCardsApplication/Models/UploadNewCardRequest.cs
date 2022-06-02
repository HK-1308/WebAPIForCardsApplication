using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ClientWPFForCardsApplication
{
    class UploadNewCardRequest
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
    }
}
