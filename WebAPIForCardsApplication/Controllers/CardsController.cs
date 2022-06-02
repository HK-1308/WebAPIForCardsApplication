using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIForCardsApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly IWebHostEnvironment _appEnvironment;

        CardsContext cardsContext;
        public CardsController(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
            cardsContext = new CardsContext(_appEnvironment);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Card>>> Get()
        {
            return await cardsContext.GetCards();
        }

        [HttpGet ("{id}")]
        public async Task<ActionResult<Card>> Get(string id)
        {
            return await cardsContext.GetCard(id);
        }

        [HttpPut]
        public async Task<ActionResult<Card>> Put(UpdateCardRequest request)
        {
            string fileName = CreateJPEG(request.Title, request.NewImage);
            DeleteJPEG(request.CurrentImage);
            return await cardsContext.UpdateCard(new Card { Id = request.Id, Title = request.Title, ImageName = fileName});
        }

        [HttpPost]
        public async Task<ActionResult<Card>> Post(UploadNewCardRequest request)
        {

            string fileName = CreateJPEG(request.Title, request.Image);

            Card card = new Card() { Id = request.Id, ImageName = fileName, Title = request.Title };

            if (request == null)
            {
                return BadRequest();
            }
            await cardsContext.CreateCard(card);
            return Ok(card);
        }

        private string CreateJPEG(string name,string image)
        {

            string wwwRootPath = _appEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(name);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + ".jpg";
            string path = Path.Combine(wwwRootPath + "/images/", fileName);

            var buffer = Convert.FromBase64String(image);
            using (var ms = new MemoryStream(buffer))
            {
                using (var fs = new FileStream(path, FileMode.Create))
                {
                    ms.WriteTo(fs);
                }
            }
            return fileName;
        }

        private void DeleteJPEG(string name)
        {
            string wwwRootPath = _appEnvironment.WebRootPath;
            string path = Path.Combine(wwwRootPath + "/images/", name);
            System.IO.File.Delete(path);
        }
        // DELETE api/cards/2
        [HttpDelete("{id}")]
        public async Task<ActionResult<Card>> Delete(string id)
        {
            var card = await cardsContext.GetCard(id);
            DeleteJPEG(card.ImageName);
            await cardsContext.DeleteCard(id);
            return Ok();
        }
    }
}
