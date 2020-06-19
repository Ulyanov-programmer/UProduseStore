using Microsoft.VisualStudio.TestTools.UnitTesting;
using UlyanovProduseStore.BL.Controller;
using System;
using System.Collections.Generic;
using UlyanovProduseStore.BL.Model;

namespace UlyanovProduseStore.BL.Controller.Tests
{
    [TestClass()]
    public class ProductControllerTests
    {
        [TestMethod()]
        public void SetNameTest()
        {
            //Arrange
            Product product = new Product("X", 10, 1);

            //Act
            ProductController.SetName("NewNameOfProduct", product);

            //Assert
            Assert.AreEqual("NewNameOfProduct", ProductController.GetName(product));

        }

        [TestMethod()]
        public void SetCostTest()
        {
            //Arrange
            Product product = new Product("X", 10, 1);

            //Act
            ProductController.SetCost(500, product);

            //Assert
            Assert.AreEqual(500, ProductController.GetCost(product));

        }

        [TestMethod()]
        public void ShowProductsTest()
        {
            //Arrange

            //Act

            //Assert
            Assert.Fail();
        }

        [TestMethod()]
        public void AddProductsTest()
        {
            //Arrange
            
            //Act

            //Assert
        }
    }
}