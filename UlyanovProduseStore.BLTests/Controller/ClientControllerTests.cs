using Microsoft.VisualStudio.TestTools.UnitTesting;
using UlyanovProduseStore.BL.Controller;
using System;
using System.Collections.Generic;
using UlyanovProduseStore.BL.Model;

namespace UlyanovProduseStore.BL.Controller.Tests
{
    [TestClass()]
    public class ClientControllerTests
    {
        [TestMethod()]
        public void FindClientTest()
        {
            //Arrange
            Client client = new Client(Guid.NewGuid().ToString());

            //Act
            bool result = ClientController.FindClient(client); //Проверка данных о новом клиенте. Должно быть False т.к клиент новый.

            bool result2 = ClientController.FindClient(client); //И вновь, но должно быть True т.к клиент уже был сохранён.

            //Assert
            Assert.IsFalse(result);
            Assert.IsTrue(result2);
        }

        [TestMethod()]
        public void BuyTest()
        {
            //Arrange
            List<Product> products = new List<Product>() { new Product("p1", 10, 1), new Product("p2", 50, 2) };
            Client client = new Client("Garry", products, 1.00F, 500);

            //Act
            bool IsBuyComplete = ClientController.Buy(client);
            //Assert
            Assert.IsTrue(IsBuyComplete);
        }

        [TestMethod()]
        public void UpBalanceTest()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}