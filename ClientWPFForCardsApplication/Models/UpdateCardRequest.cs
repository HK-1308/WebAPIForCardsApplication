using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientWPFForCardsApplication.Models
{
    class UpdateCardRequest
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string CurrentImage { get; set; }

        public string NewImage { get; set; }
    }
}
