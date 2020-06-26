using UlyanovProduseStore.BL.Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
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
            string nameOfClient = Guid.NewGuid().ToString();
            string nameOfEmployee = Guid.NewGuid().ToString();

            //Act
            Client newClient = ClientController.FindPerson<Client>(nameOfClient) as Client;
            Employee newEmployee = ClientController.FindPerson<Employee>(nameOfEmployee) as Employee;
            ClientController.UpBalance(newClient, 500);

            var loadedClient = ClientController.FindPerson<Client>(nameOfClient) as Client;
            //т.к данные были изменёны и сохранены методом ранее, у загружаемого клиента данные должны быть такими же.
            var loadedEmployee = ClientController.FindPerson<Employee>(nameOfEmployee) as Employee;

            //Assert
            Assert.AreEqual(ClientController.GetBalance(newClient), ClientController.GetBalance(loadedClient));
            Assert.AreEqual(loadedClient.ToString(), loadedClient.ToString());

            Assert.AreEqual(newEmployee.ToString(), loadedEmployee.ToString());
        }

        [TestMethod()]
        public void BuyTest()
        {
            //Arrange
            List<Product> products = new List<Product>() { new Product("product1", 10, 0), new Product("product2", 50, 1) };
            Client client = new Client("Garry", products, 1.00F, 500);

            var SumCost = products.Select(x => ProductController.GetCost(x))
                                  .Sum();
            decimal BalanceBeforeBuy = ClientController.GetBalance(client);

            Client clientNull = null;

            //Act
            bool IsBuyComplete = ClientController.Buy(client);
            bool IsBuyCompleteNull = ClientController.Buy(clientNull);

            //Assert
            Assert.IsTrue(IsBuyComplete);
            Assert.AreEqual(BalanceBeforeBuy - SumCost, ClientController.GetBalance(client));
            Assert.IsFalse(IsBuyCompleteNull);
        }

        [TestMethod()]
        public void UpBalanceTest()
        {
            //Arrange
            Client client = new Client("NewClient");
            decimal clientBalanceBeforeUp = ClientController.GetBalance(client);
            Client clientNull = null;

            //Act
            bool IsBalanceUpped = ClientController.UpBalance(client, 500);
            bool IsBalanceUppedNullClient = ClientController.UpBalance(clientNull, 500);
            bool IsBalanceUppedMinusBalance = ClientController.UpBalance(client, -100500);

            //Assert
            Assert.IsTrue(IsBalanceUpped);
            Assert.AreEqual(clientBalanceBeforeUp + 500, ClientController.GetBalance(client));
            Assert.IsFalse(IsBalanceUppedNullClient);
            Assert.IsFalse(IsBalanceUppedMinusBalance);
        }

        [TestMethod()]
        public void AddProductInBasketTest()
        {
            //Arrange
            Client client = new Client(Guid.NewGuid().ToString());

            //Act
            ClientController.AddProductInBasket(client, new Product("product", 10, 0));
            string nameOfSavedProduct = ClientController.GetListOfProduct(client).First()
                                                                                 .ToString();
            //Assert
            Assert.AreEqual("product", nameOfSavedProduct);
        }
    }
}