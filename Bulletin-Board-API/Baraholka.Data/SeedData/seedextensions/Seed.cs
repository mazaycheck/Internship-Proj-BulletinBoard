using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Baraholka.Web.Data;
using Baraholka.Web.Data.SeedData;
using Baraholka.Domain.Models;
using Baraholka.Data;

namespace Baraholka.Web.Helpers
{
    public class Seed
    {
        public static void SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            var roles = new List<Role>()
            {
                new Role{Name= "Admin" },
                new Role{Name = "Moderator"},
                new Role{Name = "Member"},
                new Role{Name= "Hacker" },
            };

            foreach (var role in roles)
            {
                roleManager.CreateAsync(role).Wait();
            }

            if (!userManager.Users.Any())
            {
                var userJsonData = System.IO.File.ReadAllText("Data/SeedData/jsondata/users.json");
                var format = "dd-MM-yyyy";
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
                var users = JsonConvert.DeserializeObject<List<User>>(userJsonData, dateTimeConverter);
                foreach (var user in users)
                {
                    userManager.CreateAsync(user, "password123").Wait();
                    userManager.AddToRoleAsync(user, "Member").Wait();
                }

                //For testing only
                var admin = new User() { UserName = "Admin", Email = "admin@myweb.com" };
                userManager.CreateAsync(admin, "password123").Wait();
                userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" }).Wait();
            }
        }

        public static void SeedAnnoucements(AppDbContext context, string rootpath)
        {
            if (!context.Annoucements.Any())
            {
                AnnoucementData.RootPath = rootpath;
                foreach (var item in AnnoucementData.GetData(10))
                {
                    context.Annoucements.Add(item);
                }

                context.SaveChanges();
            }
        }

        public static void SeedPhotos(AppDbContext context, string rootPath, IImageFileProcessor imageFileProcessor)
        {
            if (!context.Photos.Any())
            {
                var annoucements = context.Annoucements.Include(p => p.Photos).ToList();
                foreach (Annoucement item in annoucements)
                {
                    var category = GetCategoryFromAnnoucement(item.BrandCategoryId);
                    item.Photos = GetPhotos(category, item.AnnoucementId, rootPath, imageFileProcessor);
                }
                context.SaveChanges();
            }
        }

        private static List<Photo> GetPhotos(Categories category, int annoucementId, string rootPath, IImageFileProcessor imageFileProcessor)
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
            string annoucementIdImageFolder = Path.Combine(rootPath, "images", $"{annoucementId}");

            var imgUrls = imageFileProcessor.UploadFilesOnServer(listOfImages, annoucementIdImageFolder);

            return imgUrls.Select(x => new Photo() { PhotoUrl = x }).ToList();
        }

        private static Categories GetCategoryFromAnnoucement(int brandCategoryId)
        {
            var myswitch = new Dictionary<Func<int, bool>, Categories>
            {
             { x => x >= 100 && x <= 110 ,    Categories.Vehicles },
             { x => x >= 200 && x <= 210 ,    Categories.MobilePhones },
             { x => x >= 300 && x <= 310 ,    Categories.Shoes },
             { x => x >= 400 && x <= 410 ,    Categories.Clothes },
             { x => x >= 500 && x <= 510 ,    Categories.Computers },
             { x => x >= 600 && x <= 610 ,    Categories.Watches },
             { x => x >= 700 && x <= 710 ,    Categories.Furniture },
             { x => x >= 800 && x <= 810 ,    Categories.Appliances },
             { x => x >= 900 && x <= 910 ,    Categories.AudioVideo },
             { x => x >= 10 && x <= 11 ,    Categories.Services },
            };

            return myswitch.First(sw => sw.Key(brandCategoryId)).Value;
        }
    }
}