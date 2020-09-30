using Microsoft.VisualStudio.TestTools.UnitTesting;
using UlyanovProduseStore.BL.Controller;
using System;
using System.Collections.Generic;
using UlyanovProduseStore.BL.Model;
using System.Linq;

namespace UlyanovProduseStore.BL.Controller.Tests
{
    [TestClass()]
    public class ProductControllerTests
    {
        [TestMethod()]
        public void SetNameTest()
        {
            //Arrange
            var context = new UPSContext(UPSContext.StringConnectToMainServer);

            string oldNameOfProduct = "OldNameTest";
            string newNameOfProduct = "NewNameTest";

            var product = new Product(oldNameOfProduct, 10);

            if (ProductController.AddProducts(context, product, context.Products.ToList()) is false)
            {
                Assert.Fail("Изменяемый продукт не был добавлен в БД!");
            }

            //Act
            string newNameOfThisProduct = ProductController.SetName(newNameOfProduct, product, context);
            string nullNameOfThisProduct = ProductController.SetName(null, product, context);

            //Assert
            Assert.AreEqual(newNameOfProduct, newNameOfThisProduct);
            Assert.IsNull(nullNameOfThisProduct);
        }

        [TestMethod()]
        public void SetCostTest()
        {
            //Arrange
            decimal oldCost = 10;
            decimal newCost = 15;

            var context = new UPSContext(UPSContext.StringConnectToMainServer);
            var product = new Product(Guid.NewGuid().ToString(), oldCost);

            if (ProductController.AddProducts(context, product, context.Products.ToList()) is false)
            {
                Assert.Fail("Изменяемый продукт не был добавлен в БД!");
            }

            //Act
            decimal newCostOfThisProduct = ProductController.SetCost(newCost, product, context);

            //Assert
            Assert.AreEqual(newCost, newCostOfThisProduct);

        }

        //[TestMethod()]
        //public void ShowProductsTest()
        //{
        //    //Arrange

        //    //Act

        //    //Assert
        //}

        //[TestMethod()]
        //public void AddProductsTest()
        //{
        //    //Arrange

        //    //Act

        //    //Assert
        //}

        //[TestMethod()]
        //public void LoadProductsTest()
        //{
        //    //Arrange
            
        //    //Act

        //    //Assert
        //}
    }
}