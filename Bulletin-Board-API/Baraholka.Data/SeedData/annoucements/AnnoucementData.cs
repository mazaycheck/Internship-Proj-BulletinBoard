using Baraholka.Domain.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Baraholka.Data.Seed
{
    public enum Categories
    {
        Vehicles,
        MobilePhones,
        Shoes,
        Clothes,
        Computers,
        Watches,
        Furniture,
        Appliances,
        AudioVideo,
        Services
    }

    public class AnnoucementData
    {
        public static string RootPath { get; set; }

        public static int Id { get; set; } = 1;

        public static List<Annoucement> GetData(int count)
        {
            List<Annoucement> list = new List<Annoucement>();

            foreach (var item in (Categories[])Enum.GetValues(typeof(Categories)))
            {
                list.AddRange(GenerateAnnoucements(item, count));
            }

            return list;
        }

        public static List<Annoucement> GenerateAnnoucements(Categories category, int n)
        {
            List<Annoucement> list = new List<Annoucement>();
            for (int i = 0; i < n; i++)
            {
                int randomDay = Faker.RandomNumber.Next(1, 20);
                list.Add(new Annoucement()
                {
                    Title = GetTitle(category),
                    Description = GetDescription(category),
                    Price = GetPrice(category),
                    UserId = GetUserId(),
                    BrandCategoryId = GetBrandCategory(category),
                    CreateDate = DateTime.Now - TimeSpan.FromDays(randomDay),
                    ExpirationDate = DateTime.Now.AddDays(30 - randomDay),
                    IsActive = true
                });

                Id++;
            }
            return list;
        }

        private static int GetBrandCategory(Categories category)
        {
            int minId;
            int maxId;
            switch (category)
            {
                case Categories.Vehicles: minId = 100; maxId = 110; break;
                case Categories.MobilePhones: minId = 200; maxId = 210; break;
                case Categories.Shoes: minId = 300; maxId = 310; break;
                case Categories.Clothes: minId = 400; maxId = 410; break;
                case Categories.Computers: minId = 500; maxId = 510; break;
                case Categories.Watches: minId = 600; maxId = 610; break;
                case Categories.Furniture: minId = 700; maxId = 710; break;
                case Categories.Appliances: minId = 800; maxId = 810; break;
                case Categories.AudioVideo: minId = 900; maxId = 910; break;
                case Categories.Services: minId = 10; maxId = 11; break;
                default: minId = 10; maxId = 11; break;
            }
            return Faker.RandomNumber.Next(minId, maxId);
        }

        private static int GetPrice(Categories category)
        {
            int minPrice;
            int maxPrice;

            switch (category)
            {
                case Categories.Vehicles: minPrice = 5000; maxPrice = 25000; break;
                case Categories.Furniture: minPrice = 300; maxPrice = 3000; break;
                case Categories.Appliances: minPrice = 200; maxPrice = 1000; break;
                default: minPrice = 50; maxPrice = 500; break;
            }
            return Faker.RandomNumber.Next(minPrice, maxPrice);
        }

        private static int GetUserId()
        {
            return Faker.RandomNumber.Next(1, 11);
        }

        private static string GetDescription(Categories category)
        {
            var sb = new StringBuilder();
            var fk = Faker.Lorem.Paragraphs(4).ToList();
            foreach (var item in fk)
            {
                sb.Append("  ");
                sb.AppendLine(item);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static string GetTitle(Categories category)
        {
            var jsonDataFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "SeedData/jsondata/annoucementTitles.json");
            var jsonData = System.IO.File.ReadAllText(jsonDataFile);

            JToken token = JObject.Parse(jsonData);

            List<string> titles = token.SelectToken(category.ToString()).Select(t => (string)t).ToList();

            var rnd = new Random();

            var rndNumber = rnd.Next(titles.Count());

            return titles[rndNumber];
        }
    }
}