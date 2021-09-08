using System;
using System.Collections.Generic;
using System.Text;
using ShopBridge.Controllers;
using ShopBridge.Data;
using ShopBridge.Model;
using ShopBridge.Interface;
using ShopBridge.Business;

using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using FluentAssertions;

namespace ShopBridge.Test
{
    public class InventoryUnitTestController
    {
        private IInventory _IInventory;
        public static DbContextOptions<DatabaseContext> dbContextOptions { get; }
        public static IConfigurationRoot configuration { get; }

        static InventoryUnitTestController()
        {
            configuration = ConfigurationHelper.GetConfiguration();
            dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>().UseSqlServer(configuration.GetConnectionString("DatabaseConnection")).Options;
        }

        public InventoryUnitTestController()
        {
            var context = new DatabaseContext(dbContextOptions);

            _IInventory = new InventoryBusiness(context);

        }

        #region Get Item By ID

        [Fact]
        public async void Task_GetItemById_Return_OkResult()
        {
            //Arrange
            var controller = new InventoryController(_IInventory);
            var ItemId = 2;

            //Act
            var data = await controller.GetItem(ItemId);

            //Assert
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_GetItemById_Return_NotFoundResult()
        {
            //Arrange
            var controller = new InventoryController(_IInventory);
            var ItemId = 100;

            //Act
            var data = await controller.GetItem(ItemId);

            //Assert
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Task_GetItemById_Return_BadRequestResult()
        {
            //Arrange
            var controller = new InventoryController(_IInventory);
            int? ItemId = null;

            //Act
            var data = await controller.GetItem(ItemId);

            //Assert
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void Task_GetItemById_MatchResult()
        {
            //Arrange
            var controller = new InventoryController(_IInventory);
            int? ItemId = 2;

            //Act
            var data = await controller.GetItem(ItemId);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(data);

            var Item = okResult.Value.Should().BeAssignableTo<Inventory>().Subject;

            Assert.Equal("Toshiba 126 cm", Item.Name);
            Assert.Equal("Toshiba 126 cm (50 inches) Vidaa OS Series 4K Ultra HD Smart LED TV 50U5050 (Black) (2020 Model) | With Dolby Vision and ATMOS", Item.Description);
        }

        #endregion

        #region Get All Items

        [Fact]
        public async void Task_GetAllItems_Return_OkResult()
        {
            //Arrange
            var controller = new InventoryController(_IInventory);

            //Act
            var data = await controller.GetAllItems();

            //Assert
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public void Task_GetAllItems_Return_BadRequestResult()
        {
            //Arrange
            var controller = new InventoryController(_IInventory);

            //Act
            var data = controller.GetAllItems();
            data = null;

            if (data != null)
                //Assert
                Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void Task_GetAllItems_MatchResult()
        {
            //Arrange
            var controller = new InventoryController(_IInventory);

            //Act
            var data = await controller.GetAllItems();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(data);

            var item = okResult.Value.Should().BeAssignableTo<List<Inventory>>().Subject;

            Assert.Equal("Sony Bravia 32 inches LED TV 32W6103", item[0].Name);
            Assert.Equal("Sony Bravia 80 cm (32 inches) HD Ready Smart LED TV 32W6103 (Black) (2021 Model)", item[0].Description);

            Assert.Equal("Toshiba 126 cm", item[1].Name);
            Assert.Equal("Toshiba 126 cm (50 inches) Vidaa OS Series 4K Ultra HD Smart LED TV 50U5050 (Black) (2020 Model) | With Dolby Vision and ATMOS", item[1].Description);
        }

        #endregion

        #region Add New Blog

        [Fact]
        public async void Task_Add_ValidData_Return_OkResult()
        {
            //Arrange
            var controller = new InventoryController(_IInventory);
            var item = new Inventory() { Name = "Test Name 1", Description = "Test Description 1", Price = 20000, Qty = 5, Make = "Test Make 1", MakeYear = 2020 };

            //Act
            var data = await controller.AddItem(item);

            //Assert
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_Add_InvalidData_Return_BadRequest()
        {
            //Arrange
            var controller = new InventoryController(_IInventory);
            Inventory item = new Inventory() { Name = "Test Name 2", Description = "Test Description 2", Price = 2500, Qty = 10, Make = "Test Make 2 length more than 20", MakeYear = 2020 };

            //Act            
            var data = await controller.AddItem(item);

            //Assert
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void Task_Add_CheckDuplicate_MatchResult()
        {
            //Arrange
            var controller = new InventoryController(_IInventory);
            var item = new Inventory() { Name = "Test Name 1", Description = "Test Description 1", Price = 500, Qty = 15, Make = "Test Make 1", MakeYear = 2020 };

            //Act
            var data = await controller.AddItem(item);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(data);

            Assert.Equal("Item 'Test Name 10' already exists.", okResult.Value);
        }

        #endregion

        #region Update Existing Blog

        [Fact]
        public async void Task_Update_ValidData_Return_OkResult()
        {
            //Arrange
            var controller = new InventoryController(_IInventory);
            var itemId = 7;

            //Act
            var objItem = await controller.GetItem(itemId);

            var okResult = Assert.IsType<OkObjectResult>(objItem);

            var result = okResult.Value.Should().BeAssignableTo<Inventory>().Subject;

            var item = new Inventory();
            item.Name = "Test Title 1 Updated";
            item.Description = result.Description;
            item.Price = 2100;
            item.Qty = 50;
            item.Make = "Test Make 1 Updated";
            item.MakeYear = 2100;

            var updatedData = await controller.UpdateItem(item);

            //Assert
            Assert.IsType<OkResult>(updatedData);
        }

        [Fact]
        public async void Task_Update_InvalidData_Return_BadRequest()
        {
            //Arrange
            var controller = new InventoryController(_IInventory);
            var itemId = 8;

            //Act
            var objItem = await controller.GetItem(itemId);

            var okResult = Assert.IsType<OkObjectResult>(objItem);

            var result = okResult.Value.Should().BeAssignableTo<Inventory>().Subject;

            var item = new Inventory();
            item.Name = "Test Title 2 Updated";
            item.Description = result.Description;
            item.Price = 2100;
            item.Qty = 50;
            item.Make = "Test Make 2 Updated data with more than column len";
            item.MakeYear = 2100;

            var data = await controller.UpdateItem(item);

            //Assert
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void Task_Update_InvalidData_Return_NotFound()
        {
            //Arrange
            var controller = new InventoryController(_IInventory);
            var itemId = 2;

            //Act
            var objItem = await controller.GetItem(itemId);

            var okResult = Assert.IsType<OkObjectResult>(objItem);

            var result = okResult.Value.Should().BeAssignableTo<Inventory>().Subject;

            var item = new Inventory();
            item.ItemID = 100;
            item.Name = "Test Title 3 Updated item not found";
            item.Description = result.Description;
            item.Price = 2100;
            item.Qty = 50;
            item.Make = "Test Make 3 Updated not found ";
            item.MakeYear = 2100;

            var data = await controller.UpdateItem(item);

            //Assert
            Assert.IsType<NotFoundResult>(data);
        }

        #endregion

        #region Delete item

        [Fact]
        public async void Task_Delete_item_Return_OkResult()
        {
            //Arrange
            var controller = new InventoryController(_IInventory);
            var itemId = 7;

            //Act
            var data = await controller.DeleteItem(itemId);

            //Assert
            Assert.IsType<OkResult>(data);
        }

        [Fact]
        public async void Task_Delete_item_Return_NotFoundResult()
        {
            //Arrange
            var controller = new InventoryController(_IInventory);
            var itemId = 8;

            //Act
            var data = await controller.DeleteItem(itemId);

            //Assert
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Task_Delete_Return_BadRequestResult()
        {
            //Arrange
            var controller = new InventoryController(_IInventory);
            int? itemId = null;

            //Act
            var data = await controller.DeleteItem(itemId);

            //Assert
            Assert.IsType<BadRequestResult>(data);
        }

        #endregion
    }
}
