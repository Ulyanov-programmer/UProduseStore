using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using UlyanovProduseStore.BL.Controller;

namespace UlyanovProduseStore.BL.Model.Tests
{
    [TestClass()]
    public class CreateClientTests
    {
        [TestMethod()]
        public void ClientControllersTest()
        {
            //Arrange
            string name = Guid.NewGuid().ToString();
            string password = Guid.NewGuid().ToString();

            string nameForFull = name + "FULL";
            string passwordForFull = password + "FULL";
            //List<Product> products = new List<Product> { new Product("prod1", 100, 1) };

            //Act
            Client client = new Client(name, password);
            Client clientNullName = new Client(null, password);
            Client clientNullPassword = new Client(name, null);

            //Client fullClient = new Client(nameForFull, passwordForFull, products, 1.00, 1000);
            //Client fullClientNullName = new Client(null, passwordForFull, products, 1.00, 1000);
            //Client fullClientNullPassword = new Client(nameForFull, null, products, 1.00, 1000);
            //Client fullClientNullBasket = new Client(nameForFull, passwordForFull, null, 1.00, 1000);
            //Client fullClientInvalidDiscountFactor = new Client(nameForFull, passwordForFull, products, 0.13141421, 1000);
            //Client fullClientMinusBalance = new Client(nameForFull, passwordForFull, products, 1.00, -5153213);

            //Assert
            Assert.AreEqual(name, client.ToString());
            Assert.AreEqual(password, ClientController.GetPassword(client));
            Assert.IsNotNull(ClientController.GetBalance(client));
            //Assert.IsNotNull(ClientController.GetDiscountRate(client));
            //Assert.IsNotNull(ClientController.GetListOfProduct(client));


            Assert.AreEqual(nameForFull, nameForFull.ToString());
            //Assert.AreEqual(passwordForFull, ClientController.GetPassword(fullClient));
            //Assert.IsNotNull(ClientController.GetBalance(fullClient));
            //Assert.IsNotNull(ClientController.GetDiscountRate(fullClient));
            //Assert.IsNotNull(ClientController.GetListOfProduct(fullClient));


            Assert.IsNull(clientNullName.ToString());
            Assert.IsNull(ClientController.GetPassword(clientNullPassword));

            //Assert.IsNull(fullClientNullName.ToString());
            //Assert.IsNull(ClientController.GetPassword(fullClientNullPassword));
            //Assert.IsNull(ClientController.GetListOfProduct(fullClientNullBasket));
            //Assert.AreEqual(default, ClientController.GetDiscountRate(fullClientInvalidDiscountFactor));
            //Assert.AreEqual(default, ClientController.GetBalance(fullClientMinusBalance));
        }
    }
}