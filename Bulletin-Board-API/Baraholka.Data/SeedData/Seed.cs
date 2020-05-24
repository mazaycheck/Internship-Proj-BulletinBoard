using Baraholka.Domain.Models;
using Baraholka.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Baraholka.Data.Seed
{
    public class Seed
    {
        public static string SeedFolderPath
        {
            get => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "SeedData");
        }

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
                var fullpath = Path.Combine(SeedFolderPath, "jsondata/users.json");
                var userJsonData = System.IO.File.ReadAllText(fullpath);
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
                admin.RegistrationDate = DateTime.Now - TimeSpan.FromDays(30);
                admin.LockoutEnabled = false;
                admin.PhoneNumber = "12345678";
                admin.TownId = 1;                
                userManager.CreateAsync(admin, "password123").Wait();
                userManager.AddToRolesAsync(admin, new[] { "Admin" }).Wait();
            }
        }

        public static void SeedAnnoucements(AppDbContext context)
        {
            if (!context.Annoucements.Any())
            {
                foreach (var item in AnnoucementData.GetData(20))
                {
                    context.Annoucements.Add(item);
                }

                context.SaveChanges();
            }
        }

        public static void SeedPhotos(AppDbContext context, string rootPath, List<ImageFolderConfig> folders, IImageFileProcessor imageFileProcessor, IImageFileManager fileManager)
        {
            if (!context.Photos.Any())
            {                
                var annoucements = context.Annoucements.Include(p => p.Photos).ToList();
                foreach (Annoucement item in annoucements)
                {
                    var category = GetCategoryFromAnnoucement(item.BrandCategoryId);
                    item.Photos = GetPhotos(category, item.AnnoucementId, rootPath, folders, imageFileProcessor, fileManager);
                }
                context.SaveChanges();
            }
        }

        public static void SeedMessages(AppDbContext context)
        {
            if (!context.Messages.Any())
            {
                for (int i = 1; i <= 11; i++)
                {
                    for (int y = 1; y <= 11; y++)
                    {
                        Message newMessage = new Message()
                        {
                            DateTimeSent = DateTime.Now - TimeSpan.FromDays(Faker.RandomNumber.Next(1, 10)),
                            RecieverId = i,
                            SenderId = y,
                            IsRead = true,
                            Text = Faker.Lorem.Sentence(2),
                            DateTimeRead = DateTime.Now,
                        };
                        context.Messages.Add(newMessage);
                    }
                }
            }
        }

        private static List<Photo> GetPhotos(Categories category, int annoucementId, string rootPath, List<ImageFolderConfig> folders, IImageFileProcessor imageFileProcessor, IImageFileManager fileManager)
        {
            var seedRoot = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "SeedData");
            var seedFolder = Path.Combine(seedRoot, "seedimages");
            var imageFolder = Path.Combine(seedFolder, category.ToString());

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

            var imgUrls = fileManager.UploadImageFilesOnServer(listOfImages, annoucementIdImageFolder, folders);

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