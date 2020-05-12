using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApplication2.Models;

namespace WebApplication2.Data.SeedData
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
                list.Add(new Annoucement()
                {
                    AnnoucementId = Id,
                    Title = GetTitle(category),
                    Description = GetDescription(category),
                    Price = GetPrice(category),
                    UserId = GetUserId(),
                    BrandCategoryId = GetBrandCategory(category),
                    //Photos = GetPhotos(category, Id),
                    CreateDate = GetCreateTime()
                });

                Id++;
            }
            return list;
        }

        private static DateTime GetCreateTime()
        {
            int randomDay = Faker.RandomNumber.Next(1, 61);
            return DateTime.Now.Subtract(TimeSpan.FromDays(randomDay));
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
            var jsonData = System.IO.File.ReadAllText("Data/SeedData/annoucementTitles.json");

            JToken token = JObject.Parse(jsonData);

            List<string> titles = token.SelectToken(category.ToString()).Select(t => (string)t).ToList();

            var rnd = new Random();

            var rndNumber = rnd.Next(titles.Count());

            return titles[rndNumber];
        }

        // TESTING 12/05/2020
        //private static List<Photo> GetPhotos(Categories category, int annoucementId)
        //{
        //    var imageFolder = Path.Combine("Data/SeedData/seedimages", category.ToString());

        //    var images = Directory.EnumerateFiles(imageFolder).Select(x => Path.GetFileName(x)).ToList();

        //    var rand = new Random();
        //    var randSequence = Enumerable.Range(0, images.Count()).OrderBy(x => Guid.NewGuid()).ToList();

        //    var listOfImages = new List<Image>();
        //    foreach (var i in Enumerable.Range(1, 6))
        //    {
        //        var fileNumber = randSequence[i];
        //        var fileName = Path.Combine(imageFolder, images[fileNumber]);
        //        listOfImages.Add(Image.FromFile(fileName));
        //    }
        //    string annoucementIdImageFolder = Path.Combine(RootPath, "images", $"{annoucementId}");

        //    var imgUrls = UploadFilesOnServer(listOfImages, annoucementIdImageFolder);

        //    return imgUrls.Select(x => new Photo() { PhotoUrl = x }).ToList();
        //}
    }
}