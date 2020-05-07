using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Helpers;
using WebApplication2.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Text;

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
                
        private static List<Photo> GetPhotos(Categories category, int annoucementId)
        {
            var imageFolder = Path.Combine("Data/SeedData/seedimages", category.ToString());

            var images = Directory.EnumerateFiles(imageFolder).Select(x => Path.GetFileName(x)).ToList();

            var rand = new Random();
            var randSequence = Enumerable.Range(0, images.Count()).OrderBy(x => Guid.NewGuid()).ToList();

            var listOfImages = new List<Image>();
            foreach (var i in Enumerable.Range(1, 6))
            {
                var fileNumber = randSequence[i];
                var fileName = Path.Combine(imageFolder, images[fileNumber]);                
                listOfImages.Add(Image.FromFile(fileName));  
            }
            string annoucementIdImageFolder = Path.Combine(RootPath, "images", $"{annoucementId}");

            var imgUrls = ImageFileProcessor.UploadFilesOnServerAndGetListOfFileNames(listOfImages, annoucementIdImageFolder);

            return imgUrls.Select(x => new Photo() { PhotoUrl = x }).ToList();
            
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
                    Photos = GetPhotos(category, Id++),
                    CreateDate = GetCreateTime()
                });
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
            int brandCategoryId;
            int minId;
            int maxId;
            switch (category)
            {
                case Categories.Vehicles: minId = 100; maxId = 110;break;
                case Categories.MobilePhones: minId = 200; maxId = 210;break;
                case Categories.Shoes: minId = 300; maxId = 310;break;
                case Categories.Clothes: minId = 400; maxId = 410;break;
                case Categories.Computers: minId = 500; maxId = 510;break;
                case Categories.Watches: minId = 600; maxId = 610;break;
                case Categories.Furniture: minId = 700; maxId = 710;break;
                case Categories.Appliances: minId = 800; maxId = 810;break;
                case Categories.AudioVideo: minId = 900; maxId = 910;break;
                case Categories.Services: minId = 10; maxId = 11;break;                
                default: minId = 10; maxId = 11;break;                
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
                case Categories.Furniture:minPrice = 300; maxPrice = 3000;break;
                case Categories.Appliances:minPrice = 200; maxPrice = 1000;break;
                default: minPrice = 50; maxPrice = 500;break;
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

        public static List<Annoucement> GetData(int count)
        {

            List<Annoucement> list = new List<Annoucement>();
            list.AddRange(GenerateAnnoucements(Categories.Appliances, count));
            list.AddRange(GenerateAnnoucements(Categories.Vehicles, count));
            list.AddRange(GenerateAnnoucements(Categories.Watches, count));
            list.AddRange(GenerateAnnoucements(Categories.MobilePhones, count));
            list.AddRange(GenerateAnnoucements(Categories.Clothes, count));
            list.AddRange(GenerateAnnoucements(Categories.Shoes, count));
            list.AddRange(GenerateAnnoucements(Categories.Furniture, count));
            list.AddRange(GenerateAnnoucements(Categories.Services, count));
            list.AddRange(GenerateAnnoucements(Categories.AudioVideo, count));
            list.AddRange(GenerateAnnoucements(Categories.Computers, count));
            return list;
        }

             //new Annoucement() {
             //    AnnoucementId = 1,
             //    Title = "New Honda car",
             //    Description = "Brand new Honda",
             //    CreateDate = DateTime.Now,
             //    Price = 5000,
             //    UserId = 1,
             //    BrandCategoryId = 106,    
             //    Photos = GetPhotos("Vehicles",1)
             //},
             //new Annoucement() {
             //    AnnoucementId = 2,
             //    Title = "Iphone 6",
             //    Description = "Used phone",
             //    CreateDate = DateTime.Now,
             //    Price = 250,
             //    UserId = 2,
             //    BrandCategoryId = 205,
             //    Photos = GetPhotos("Mobile Phones",2)
             //},
             //new Annoucement() {
             //    AnnoucementId = 3,
             //    Title = "Adidas Sneackers", 
             //    Description = "From 2020 Collection", 
             //    CreateDate = DateTime.Now, 
             //    Price = 5000, 
             //    UserId = 3, 
             //    BrandCategoryId = 302,
             //    Photos = GetPhotos("Clothes",3)
             //},
             //new Annoucement() {
             //    AnnoucementId = 4,
             //    Title = "Nokia 1100",
             //    Description = "Old School phone",
             //    CreateDate = DateTime.Now - TimeSpan.FromDays(10),
             //    Price = 50,
             //    UserId = 3,
             //    BrandCategoryId = 207,
             //    Photos = GetPhotos("Mobile Phones",4)
             //},
             //new Annoucement() {
             //    AnnoucementId = 5,
             //    Title = "Reebok SportSuit",
             //    Description = "From 2019 Collection",
             //    CreateDate = DateTime.Now,
             //    Price = 100,
             //    UserId = 1,
             //    BrandCategoryId = 307,
             //    Photos = GetPhotos("Clothes",5)
             //},
             //new Annoucement() {
             //    AnnoucementId = 6,
             //    Title = "2010 Toyota Corolla",
             //    Description = "Comfortable car",
             //    CreateDate = DateTime.Now,
             //    Price = 6000,
             //    UserId = 2,
             //    BrandCategoryId = 107,
             //    Photos = GetPhotos("Vehicles",6)
             //},
             //            new Annoucement() {
             //    AnnoucementId = 7,
             //    Title = "New Honda car",
             //    Description = "Brand new Honda",
             //    CreateDate = DateTime.Now,
             //    Price = 5000,
             //    UserId = 1,
             //    BrandCategoryId = 104,
             //    Photos = GetPhotos("Vehicles",7)
             //},
             //new Annoucement() {
             //    AnnoucementId = 8,
             //    Title = "Iphone 6",
             //    Description = "Used phone",
             //    CreateDate = DateTime.Now,
             //    Price = 250,
             //    UserId = 2,
             //    BrandCategoryId = 209,
             //    Photos = GetPhotos("Mobile Phones",8)
             //},
             //new Annoucement() {
             //    AnnoucementId = 9,
             //    Title = "Adidas Sneackers",
             //    Description = "From 2020 Collection",
             //    CreateDate = DateTime.Now,
             //    Price = 5000,
             //    UserId = 3,
             //    BrandCategoryId = 306,
             //    Photos = GetPhotos("Clothes",9)
             //},
             //new Annoucement() {
             //    AnnoucementId = 10,
             //    Title = "Nokia 1100",
             //    Description = "Old School phone",
             //    CreateDate = DateTime.Now - TimeSpan.FromDays(10),
             //    Price = 50,
             //    UserId = 3,
             //    BrandCategoryId = 207,
             //    Photos = GetPhotos("Mobile Phones",10)
             //},



             //             new Annoucement() {
             //    AnnoucementId = 11,
             //    Title = "New Honda car",
             //    Description = "Brand new Honda",
             //    CreateDate = DateTime.Now,
             //    Price = 5000,
             //    UserId = 1,
             //    BrandCategoryId = 101,
             //    Photos = GetPhotos("Vehicles",11)
             //},
             //new Annoucement() {
             //    AnnoucementId = 12,
             //    Title = "Iphone 6",
             //    Description = "Used phone",
             //    CreateDate = DateTime.Now,
             //    Price = 250,
             //    UserId = 2,
             //    BrandCategoryId = 08,
             //    Photos = GetPhotos("Mobile Phones",12)
             //},
             //new Annoucement() {
             //    AnnoucementId = 13,
             //    Title = "Adidas Sneackers",
             //    Description = "From 2020 Collection",
             //    CreateDate = DateTime.Now,
             //    Price = 5000,
             //    UserId = 3,
             //    BrandCategoryId = 301,
             //    Photos = GetPhotos("Clothes",13)
             //},
             //new Annoucement() {
             //    AnnoucementId = 14,
             //    Title = "Nokia 1100",
             //    Description = "Old School phone",
             //    CreateDate = DateTime.Now - TimeSpan.FromDays(10),
             //    Price = 50,
             //    UserId = 3,
             //    BrandCategoryId = 204,
             //    Photos = GetPhotos("Mobile Phones",14)
             //},
             //new Annoucement() {
             //    AnnoucementId = 15,
             //    Title = "Reebok SportSuit",
             //    Description = "From 2019 Collection",
             //    CreateDate = DateTime.Now,
             //    Price = 100,
             //    UserId = 1,
             //    BrandCategoryId = 303,
             //    Photos = GetPhotos("Clothes",15)
             //},
             //new Annoucement() {
             //    AnnoucementId = 16,
             //    Title = "2010 Toyota Corolla",
             //    Description = "Comfortable car",
             //    CreateDate = DateTime.Now,
             //    Price = 6000,
             //    UserId = 2,
             //    BrandCategoryId = 102,
             //    Photos = GetPhotos("Vehicles",16)
             //},
             //            new Annoucement() {
             //    AnnoucementId = 17,
             //    Title = "New Honda car",
             //    Description = "Brand new Honda",
             //    CreateDate = DateTime.Now,
             //    Price = 5000,
             //    UserId = 1,
             //    BrandCategoryId = 106,
             //    Photos = GetPhotos("Vehicles",17)
             //},
             //new Annoucement() {
             //    AnnoucementId = 18,
             //    Title = "Iphone 6",
             //    Description = "Used phone",
             //    CreateDate = DateTime.Now,
             //    Price = 250,
             //    UserId = 2,
             //    BrandCategoryId = 204,
             //    Photos = GetPhotos("Mobile Phones",18)
             //},
             //new Annoucement() {
             //    AnnoucementId = 19,
             //    Title = "Adidas Sneackers",
             //    Description = "From 2020 Collection",
             //    CreateDate = DateTime.Now,
             //    Price = 5000,
             //    UserId = 3,
             //    BrandCategoryId = 300,
             //    Photos = GetPhotos("Clothes",19)
             //},
             //new Annoucement() {
             //    AnnoucementId = 20,
             //    Title = "Nokia 1100",
             //    Description = "Old School phone",
             //    CreateDate = DateTime.Now - TimeSpan.FromDays(10),
             //    Price = 50,
             //    UserId = 3,
             //    BrandCategoryId = 200,
             //    Photos = GetPhotos("Mobile Phones",20)
             //},

        

    


    }
}
