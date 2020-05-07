using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Controllers;
using WebApplication2.Data.Repositories;
using WebApplication2.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MockQueryable.Moq;
using System;
//using System.Web.Http.Results;

namespace NUnitTestProject1
{
    public class TownControllerTests
    {
        private readonly Mock<IGenericRepository<Town>> _mockRepo;
        private readonly TownsController _controller;

        public TownControllerTests()
        {
            _mockRepo = new Mock<IGenericRepository<Town>>();
            _controller = new TownsController(_mockRepo.Object);
        }
        [SetUp]
        public void Setup()
        {
            var towns = new List<Town>() {
                    new Town() { TownId = 1 , Title = "Chisinau" },
                    new Town(){ TownId = 2, Title =  "Balti"},
                    new Town(){ TownId = 3 , Title = "Cahul" } };

            var mockTownsList = towns.AsQueryable().BuildMock();

            _mockRepo.Setup(x => x.GetAll()).Returns(mockTownsList.Object);

            _mockRepo.Setup(x => x.GetById(5)).ReturnsAsync(new Town() { TownId = 5, Title = "New York" });


        }

        [Test]
        public void Returns_IActionResult()
        {
            var result = _controller.Get();
            Assert.IsInstanceOf<Task<IActionResult>>(result);
        }

        [Test]
        public async Task Converts_To_OkObjectResult_And_Not_Null()
        {
            var okResult = await _controller.Get() as OkObjectResult;
            Assert.IsNotNull(okResult);
        }

        [Test]
        public async Task Returns_List_Of_Towns()
        {
            var towns = ((OkObjectResult)await _controller.Get()).Value as List<Town>;
            Assert.IsNotNull(towns);
        }

        [Test]
        public async Task Returns_3_Entities()
        {
            var towns = ((OkObjectResult)await _controller.Get()).Value as List<Town>;
            Assert.AreEqual(3, towns.Count());
        }

        [Test]
        public async Task Returns_Town_Type()
        {
            var actionResult = await _controller.Get(5);
            Assert.IsInstanceOf<IActionResult>(actionResult);
            var town = ((OkObjectResult)actionResult).Value;
            Assert.IsInstanceOf<Town>(town);
        }
        [Test]
        public async Task Returns_Town_With_Id_5_Name_New_York()
        {
            var actionResult = await _controller.Get(5);            
            var town = ((OkObjectResult)actionResult).Value as Town;
            Assert.AreEqual(town.TownId, 5);
            Assert.AreEqual(town.Title, "New York");            
        }

        [Test]
        public async Task Update_Does_Not_throw()
        {
            var town1 = new Town() { TownId = 1, Title = "The Chisinau" };
            _mockRepo.Setup(x => x.Update(town1));            
            Assert.DoesNotThrowAsync(() => _controller.Put(town1));

        }

        [Test]
        public void Update_Throws_Exception()
        {            
            var town2 = new Town() { TownId = 2 };
            var town3 = new Town() { Title = "Great Balti" };
            
            _mockRepo.Setup(x => x.Update(town2)).Throws(new ArgumentException("Not Enough Arguments"));
            _mockRepo.Setup(x => x.Update(town3)).Throws(new ArgumentException("Primary Id not set"));
            
            Assert.ThrowsAsync<ArgumentException>(async () => await _controller.Put(town2), "Not Enough Arguments");
            Assert.ThrowsAsync<ArgumentException>(async () => await _controller.Put(town3), "Primary Id not set");            
        }

        [Test]
        public async Task Update_Model_State_Returns_BadRequest()
        {
            _controller.ModelState.AddModelError("Title", "Required field missing");

            var town4 = new Town() { TownId = 4 };

            var result = _controller.TryValidateModel(town4);

            _mockRepo.Setup(x => x.Update(town4)).Throws(new ArgumentException("Not Enough Arguments"));

            

            var response = await _controller.Put(town4);

            //_controller.ModelState.ClearValidationState("Title");

            Assert.IsInstanceOf<BadRequestObjectResult>(response);
            Assert.AreEqual(400, ((BadRequestObjectResult)response).StatusCode);
            //Assert.AreEqual("Required field missing", ((BadRequestObjectResult)response).Value["Title"]);


        }

    }
}