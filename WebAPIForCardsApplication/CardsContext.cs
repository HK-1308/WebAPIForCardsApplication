using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Threading.Tasks;

namespace WebAPIForCardsApplication
{
    public class CardsContext
    {
        private readonly IWebHostEnvironment hostEnvironment;

        private readonly DataSource dataSource;

        public CardsContext(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
            dataSource = new DataSource(hostEnvironment);
        }


        public async  Task<List<Card>> GetCards()
        {

            using (StreamReader file = File.OpenText(dataSource.DataSourcePath))
            {
                var cards = JsonConvert.DeserializeObject<List<Card>>(File.ReadAllText(dataSource.DataSourcePath));
                return cards;
            }

        }

        public async  Task<bool> CreateCard(Card card)
        {
            var cardsList = await GetCards();

            cardsList.Add(card);

            using (StreamWriter file = File.CreateText(dataSource.DataSourcePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, cardsList);
            }
            return true;
        }
        public async  Task<Card> GetCard(string id)
        {
            var cardsList = await GetCards();
            var card = cardsList.FirstOrDefault(c => c.Id == id);
            return card;
        }

        public async  Task<Card> UpdateCard(Card card)
        {
            var cardsList = await GetCards();
            cardsList.Remove(cardsList.FirstOrDefault(c => c.Id == card.Id));
            cardsList.Add(card);
            using (StreamWriter file = File.CreateText(dataSource.DataSourcePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, cardsList);
            }
            return card;
        }

        public async  Task<bool> DeleteCard(string id)
        {
            var cardsList = await GetCards();
            cardsList.Remove(cardsList.FirstOrDefault(c => c.Id == id));


            using (StreamWriter file = File.CreateText(dataSource.DataSourcePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, cardsList);
            }
            return true;
        }

        public async  Task<bool> DeleteSelectedCard(string[] ids)
        {
            var cardsList = await GetCards();
            foreach (var id in ids)
                cardsList.Remove(cardsList.FirstOrDefault(c => c.Id == id));
            using (StreamWriter file = File.CreateText(dataSource.DataSourcePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, cardsList);
            }
            return true;
        }

    }
}
