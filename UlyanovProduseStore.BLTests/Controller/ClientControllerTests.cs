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
        public void LoadClientTest()
        {
            //Arrange
            var context = new UPSContext(UPSContext.StringConnectToMainServer);

            string nameOfClient = Guid.NewGuid().ToString();
            string nameOfEmp = Guid.NewGuid().ToString();
            string passwordOrID = Guid.NewGuid().ToString();
            string wrongPassword = passwordOrID + "X6X6X6";

            //Act
            //Сохранение новых пользователей.
            Client currentClient = ClientController.RegistrationOfPerson<Client>(nameOfClient, passwordOrID, context) as Client;
            Employee newEmployee = ClientController.RegistrationOfPerson<Employee>(nameOfEmp, passwordOrID, context) as Employee;

            //Изменение и сохранение данных текущего клиента.
            ClientController.UpBalance(currentClient, 500, context);

            //Загрузка данных сохранённого клиента. 
            Client loadedClient = ClientController.LoadOfPerson<Client>(nameOfClient, passwordOrID, context) as Client;

            Client loadedClientWithWrongPassword = ClientController.LoadOfPerson<Client>(nameOfClient, wrongPassword, context) as Client;
            Employee loadedEmployee = ClientController.LoadOfPerson<Employee>(nameOfEmp, passwordOrID, context) as Employee;

            //Assert
            //Сравнение данных текущего клиента и клиента загружаемого. Должны быть равны т.к данные загружаемого были созданы на основе текущего.
            Assert.AreEqual(ClientController.GetBalance(currentClient), ClientController.GetBalance(loadedClient));
            Assert.IsNull(loadedClientWithWrongPassword);
            Assert.AreEqual(loadedClient.ToString(), loadedClient.ToString());

            Assert.AreEqual(newEmployee.ToString(), loadedEmployee.ToString());
        }

        [TestMethod()]
        public void BuyTest()
        {
            //Arrange
            var context = new UPSContext(UPSContext.StringConnectToMainServer);
            var client = new Client(Guid.NewGuid().ToString(), "TEST", 500000);
            var basket = new Basket(client);

            for (int index = 0; index < 5; index++)
            {
                basket.Products.Add(new Product("product" + index, 10));
            }

            var sumCost = basket.Products.Sum(prod => prod.Cost);
            decimal balanceBeforeBuy = ClientController.GetBalance(client);


            Basket basketNull = null;

            //Act
            bool isBuyComplete = ClientController.Buy(basket, context);
            bool isBuyCompleteNull = ClientController.Buy(basketNull, context);

            //Assert
            Assert.IsTrue(isBuyComplete);
            Assert.AreEqual(balanceBeforeBuy - sumCost, ClientController.GetBalance(client));
            Assert.IsFalse(isBuyCompleteNull);
        }

        [TestMethod()]
        public void UpBalanceTest()
        {
            //Arrange
            decimal sum = 500;
            var context = new UPSContext(UPSContext.StringConnectToMainServer);
            var client = new Client("NewClient", "TEST");
            decimal clientBalanceBeforeUp = client.Balance;
            Client clientNull = null;

            //Act
            decimal uppedBalance = ClientController.UpBalance(client, sum, context);
            decimal uppedBalanceNullClient = ClientController.UpBalance(clientNull, sum, context);
            decimal uppedBalanceMinusBalance = ClientController.UpBalance(client, -100500, context);

            //Assert
            Assert.AreEqual(clientBalanceBeforeUp + sum, uppedBalance);
            Assert.IsTrue(uppedBalanceNullClient == -1);
            Assert.IsTrue(uppedBalanceMinusBalance == -1);
        }

        [TestMethod()]
        public void AddProductInBasketTest()
        {
            //Arrange
            var products = new List<Product>();
            var client = new Client(Guid.NewGuid().ToString(), "TEST", 500000);
            var basket = new Basket(client);

            for (int i = 0; i < 5; i++)
            {
                products.Add(new Product("product" + i, 10));
            }

            //Act
            for (int index = 0; index < products.Count; index++)
            {
                ClientController.AddProductInBasket(basket, products[index]);
            }

            //Assert
            for (int index = 0; index < products.Count; index++)
            {
                Assert.AreEqual(basket.Products[index], products[index]);
            }
        }

        [TestMethod()]
        public void DeleteProductFromBasketTest()
        {
            //Arrange
            string nameOfProductBeDeleted = "product1";

            var products = new List<Product>()
            {
                new Product(nameOfProductBeDeleted, 10),
                new Product(nameOfProductBeDeleted, 12),
                new Product("won't Delete", 13),
            };
            var client = new Client(Guid.NewGuid().ToString(), "TEST", 500000);
            var basket = new Basket(client, products);

            // Количество продуктов с одним названием ДО удаления.
            int countOfProductBeforeRemoval = basket.Products.Count(prod => prod.Name == nameOfProductBeDeleted);

            //Act
            bool productIsDeleted = ClientController.DeleteProductFromBasket(basket, nameOfProductBeDeleted);
            bool productIsDeletedFromNull = ClientController.DeleteProductFromBasket(null, nameOfProductBeDeleted);
            bool productIsDeletedWithNull = ClientController.DeleteProductFromBasket(basket, null);
            bool productIsDeletedNull = ClientController.DeleteProductFromBasket(null, null);

            // Количество продуктов с одним названием ПОСЛЕ удаления.
            int countOfProductAfterRemoval = basket.Products.Count(prod => prod.Name == nameOfProductBeDeleted);

            //Assert
            Assert.IsTrue(productIsDeleted);
            Assert.IsFalse(productIsDeletedFromNull);
            Assert.IsFalse(productIsDeletedWithNull);
            Assert.IsFalse(productIsDeletedNull);

            Assert.AreNotEqual(countOfProductBeforeRemoval, countOfProductAfterRemoval);
        }

        [TestMethod()]
        public void RegistrationOfPersonTest()
        {
            //Arrange
            var context = new UPSContext(UPSContext.StringConnectToMainServer);
            string nameOfNewClient = Guid.NewGuid().ToString();
            string nameOfNewEmp = Guid.NewGuid().ToString();
            string passwordOrID = Guid.NewGuid().ToString();

            //Act
            Client client = ClientController.RegistrationOfPerson<Client>(nameOfNewClient, passwordOrID, context) as Client;
            Employee employee = ClientController.RegistrationOfPerson<Employee>(nameOfNewEmp, passwordOrID, context) as Employee;

            Client clientNullName = ClientController.RegistrationOfPerson<Client>(null, passwordOrID, context) as Client;
            Client clientNullPassword = ClientController.RegistrationOfPerson<Client>(nameOfNewClient, null, context) as Client;
            Client clientNullArguments = ClientController.RegistrationOfPerson<Client>(null, null, context) as Client;
            Client alreadyRegisteredClient = ClientController.RegistrationOfPerson<Client>(nameOfNewClient, passwordOrID, context) as Client;
            Client clientWhereTypeIsPerson = ClientController.RegistrationOfPerson<Person>(nameOfNewClient, passwordOrID, context) as Client;

            Employee employeeNullName = ClientController.RegistrationOfPerson<Client>(null, passwordOrID, context) as Employee;
            Employee employeetNullPassword = ClientController.RegistrationOfPerson<Client>(nameOfNewClient, null, context) as Employee;
            Employee employeetNullArguments = ClientController.RegistrationOfPerson<Client>(null, null, context) as Employee;
            Employee alreadyRegisteredEmployee = ClientController.RegistrationOfPerson<Client>(nameOfNewClient, passwordOrID, context) as Employee;
            Employee employeetWhereTypeIsPerson = ClientController.RegistrationOfPerson<Person>(nameOfNewClient, passwordOrID, context) as Employee;

            //Assert
            Assert.IsNotNull(client);
            Assert.IsNotNull(employee);

            Assert.IsNull(clientNullName);
            Assert.IsNull(clientNullPassword);
            Assert.IsNull(clientNullArguments);
            Assert.IsNull(alreadyRegisteredClient);
            Assert.IsNull(clientWhereTypeIsPerson);
            Assert.IsNull(employeeNullName);
            Assert.IsNull(employeetNullPassword);
            Assert.IsNull(employeetNullArguments);
            Assert.IsNull(alreadyRegisteredEmployee);
            Assert.IsNull(employeetWhereTypeIsPerson);
        }
    }
}