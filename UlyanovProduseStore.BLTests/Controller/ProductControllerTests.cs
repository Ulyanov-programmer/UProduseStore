using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
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
            string nullProduct = ProductController.SetName(newNameOfProduct, null, context);

            //Assert
            Assert.AreEqual(newNameOfProduct, newNameOfThisProduct);

            Assert.IsNull(nullNameOfThisProduct);
            Assert.IsNull(nullProduct);
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
            { Assert.Fail("Изменяемый продукт не был добавлен в БД!"); }

            //Act
            decimal newCostOfThisProduct = ProductController.SetCost(newCost, product, context);
            decimal minusCostResult = ProductController.SetCost(-1, product, context);
            decimal nullCostResult = ProductController.SetCost(0, product, context);
            decimal nullProductResult = ProductController.SetCost(newCost, null, context);

            //Assert
            Assert.AreEqual(newCost, newCostOfThisProduct);

            Assert.IsTrue(minusCostResult == -1);
            Assert.IsTrue(nullCostResult == -1);
            Assert.IsTrue(nullProductResult == -1);
        }

        [TestMethod()]
        public void AddProductsTest()
        {
            //Arrange
            var context = new UPSContext(UPSContext.StringConnectToMainServer);
            List<Product> prodFromDb = context.Products.ToList();

            var product = new Product($"TEST {Guid.NewGuid()}", 10);
            var productNullName = new Product(null, 10);
            var productMinusCost = new Product($"TEST {Guid.NewGuid()}", -1);
            var productNull = new Product(null, 0);

            //Act
            ProductController.AddProducts(context, product, prodFromDb);
            ProductController.AddProducts(context, productNullName, prodFromDb);
            ProductController.AddProducts(context, productMinusCost, prodFromDb);
            ProductController.AddProducts(context, productNull, prodFromDb);

            //Assert
            Assert.IsTrue(context.Products.Any(prod => prod.Name == product.Name));

            Assert.IsFalse(context.Products.Any(prod => prod.Name == null));
            Assert.IsFalse(context.Products.Any(prod => prod.Cost == -1));
            Assert.IsFalse(context.Products.Any(prod => prod.Name == null && prod.Cost == 0),
                                                "В базе данных есть экземпляры Product с Null значениями имени и стоимости!");
        }
    }
}