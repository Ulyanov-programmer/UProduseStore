using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            var context = new UProduseStoreContext(ClientController.ConnectToTestServer);
            string newNameOfProduct = "NewNameOfProduct";

            Product productToBeAdded = new Product(Guid.NewGuid().ToString(), 40);
            int idOf_AddedProduct = ProductController.AddProducts(productToBeAdded, context);

            if (idOf_AddedProduct == -1)
            {
                Assert.Fail("Продукт не был добавлен!");
            }

            //Act
            ProductController.SetName(newNameOfProduct, productToBeAdded, context);
            var loadedProduct = context.Products.FirstOrDefault(prod => prod.Id == idOf_AddedProduct);

            //Assert
            Assert.AreEqual(newNameOfProduct, loadedProduct.Name);
        }

        [TestMethod()]
        public void SetCostTest()
        {
            //Arrange
            var context = new UProduseStoreContext(ClientController.ConnectToTestServer);
            decimal newCost = 500;
            Product product = new Product(Guid.NewGuid().ToString(), 23);

            if (ProductController.AddProducts(product, context) == -1)
            {
                Assert.Fail("Продукт не был добавлен!");
            }

            //Act
            ProductController.SetCost(newCost, product, context);

            //Assert
            Assert.AreEqual(newCost, product.Cost);
        }

        [TestMethod()]
        public void AddProductsTest()
        {
            //Arrange
            var product = new Product(Guid.NewGuid().ToString(), 10);
            Product productNull = null;

            var context = new UProduseStoreContext(ClientController.ConnectToTestServer);
            UProduseStoreContext contextNull = null;

            //Act
            ProductController.AddProducts(product, context);
            var listWith_LoadedProduct = ProductController.LoadProducts(context);
            bool isTheList_HasThisProduct = listWith_LoadedProduct.Any(prod => prod.Name == product.Name);

            // Должны вернуть -1 и не записать экземпляр Product, поскольку имеют некорректные аргументы.
            int resultId_WhenProductNull = ProductController.AddProducts(productNull, context);
            int resultId_WhencontextNull = ProductController.AddProducts(product, contextNull);

            //Assert
            Assert.IsTrue(isTheList_HasThisProduct);
            Assert.AreEqual(-1, resultId_WhenProductNull);
            Assert.AreEqual(-1, resultId_WhencontextNull);
        }
    }
}