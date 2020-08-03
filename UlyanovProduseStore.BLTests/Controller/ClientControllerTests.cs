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
        public void LoadClientTest()
        {
            //Arrange
            string nameOfClient = Guid.NewGuid().ToString();
            string nameOfEmp = Guid.NewGuid().ToString();
            string passwordOrSecondName = Guid.NewGuid().ToString();
            string wrongPassword = passwordOrSecondName + "X6X6X6";
            var context = new UProduseStoreContext(ClientController.ConnectToTestServer);

            //Act
            //Сохранение новых пользователей.
            Client newClient = ClientController.RegistrationOfPerson<Client>(nameOfClient, passwordOrSecondName, context) as Client;
            Employee newEmployee = ClientController.RegistrationOfPerson<Employee>(nameOfEmp, passwordOrSecondName, context) as Employee;

            //Изменение и сохранение данных текущего клиента.
            ClientController.UpBalance(newClient, 500, context);

            //Загрузка данных сохранённого клиента. 
            Client loadedClient = ClientController.LoadOfPerson<Client>(nameOfClient, passwordOrSecondName, context) as Client;
            Client loadedClientWithWrongPassword = ClientController.LoadOfPerson<Client>(nameOfClient, wrongPassword, context) as Client;

            Employee loadedEmployee = ClientController.LoadOfPerson<Employee>(nameOfEmp, passwordOrSecondName, context) as Employee;

            //Assert
            //Сравнение данных текущего клиента и клиента загружаемого. Должны быть равны т.к данные загружаемого были созданы на основе текущего.
            Assert.AreEqual(ClientController.GetBalance(newClient), ClientController.GetBalance(loadedClient));
            Assert.IsNull(loadedClientWithWrongPassword);
            Assert.AreEqual(newClient.ToString(), loadedClient.ToString());

            Assert.AreEqual(newEmployee.ToString(), loadedEmployee.ToString());
        }

        [TestMethod()]
        public void BuyTest()
        {
            //Arrange
            var context = new UProduseStoreContext(ClientController.ConnectToTestServer);

            List<Product> products = new List<Product>()
            {
                new Product("product1", 10),
                new Product("product2", 50)
            };

            Client client = new Client(Guid.NewGuid().ToString(), "X", products, 500); /* У экземпляра Client в конструктор 
                                                                                          которого была помещена коллекция продуктов
                                                                                          уже вызван WriteProductInFileBasket, 
                                                                                          т.е имеет записанный файл с этими продуктами. */
            var sumCost = products.Select(x => ProductController.GetCost(x))
                                  .Sum();
            if (sumCost > client.Balance)
            {
                Assert.Fail(); //Баланс пользователя должен быть больше суммарной стоимости продуктов!
            }
            decimal BalanceBeforeBuy = ClientController.GetBalance(client);

            Client clientNull = null;
            Client clientNullBusket = new Client(Guid.NewGuid().ToString(), "X", null, 500);
            Client clientNullBalance = new Client(Guid.NewGuid().ToString(), "X", products, 0);

            //Act
            //Перед методом покупки клиента нужно зарегистрировать!
            ClientController.Registration_OfFullPerson<Client>(client, context);

            bool isBuyComplete = ClientController.Buy(client, context, true);
            bool isBuyCompleteNull = ClientController.Buy(clientNull, context, true);
            bool isBuyCompleteNullBusket = ClientController.Buy(clientNullBusket, context, true);
            bool isBuyCompleteNullBalance = ClientController.Buy(clientNullBalance, context, true);

            client = ClientController.LoadOfPerson<Client>(client.Name, client.Password, context) as Client;

            //Assert
            Assert.IsTrue(isBuyComplete);
            Assert.AreEqual(BalanceBeforeBuy - sumCost, ClientController.GetBalance(client));

            Assert.IsFalse(isBuyCompleteNull);
            Assert.IsFalse(isBuyCompleteNullBusket);
            Assert.IsFalse(isBuyCompleteNullBalance);
        }

        [TestMethod()]
        public void DeleteProductFromBasketTest()
        {
            //Arrange
            string nameOfProductBeDeleted = "product1";
            string nameof_aNonExistent_Product = "qeupoghpqejvbwepirghqefvojq]=j=30fu103=f0u=1304f=-3f139u13славасоветскомусоюзу-98fh-13f=1u34=90t\n";

            List<Product> products = new List<Product>
            {
                new Product(nameOfProductBeDeleted, 10),
                new Product(nameOfProductBeDeleted, 10),
                new Product("product2", 50)
            };
            List<Product> productsNull = null;

            Client client = new Client(Guid.NewGuid().ToString(), "X", products, 5000);
            Client clientNull = null;
            Client clientWithNullBasket = new Client(Guid.NewGuid().ToString(), "X", productsNull, 5000);


            int countOfProductsEligibleBeforeRemoval = ClientController.ReadFileWithListOfProduct(client, false)
                                                                       .Where(prod => ProductController.GetName(prod) == nameOfProductBeDeleted)
                                                                       .Count(); // Количество продуктов с названием "имя удаляемого продукта" ДО удаления.
            //Act
            bool isDeleted = ClientController.DeleteProductFromBasket(client, nameOfProductBeDeleted);

            int сountOfProductsEligibleAfterRemoval = ClientController.ReadFileWithListOfProduct(client, false)
                                                      .Where(prod => ProductController.GetName(prod) == nameOfProductBeDeleted)
                                                      .Count(); // Количество продуктов с названием "имя удаляемого продукта" ПОСЛЕ удаления.


            bool isDeletedFromNullClient = ClientController.DeleteProductFromBasket(clientNull, nameOfProductBeDeleted);
            bool isDeletedFromClient_WithNullBasket = ClientController.DeleteProductFromBasket(clientWithNullBasket, nameOfProductBeDeleted);
            bool isDeleted_aNonExistent_Product = ClientController.DeleteProductFromBasket(clientNull, nameof_aNonExistent_Product);
            bool isDeleted_NullString = ClientController.DeleteProductFromBasket(clientNull, null);

            
            //Assert
            Assert.IsTrue(isDeleted);
            Assert.AreNotEqual(countOfProductsEligibleBeforeRemoval, сountOfProductsEligibleAfterRemoval);

            Assert.IsFalse(isDeletedFromNullClient);
            Assert.IsFalse(isDeletedFromClient_WithNullBasket);
            Assert.IsFalse(isDeleted_aNonExistent_Product);
            Assert.IsFalse(isDeleted_NullString);
        }

        [TestMethod()]
        public void RegistrationOfPersonTest()
        {
            //Arrange
            string nameOfNewClient = Guid.NewGuid().ToString();
            string nameOfNewEmp = Guid.NewGuid().ToString();
            string passwordOrID = Guid.NewGuid().ToString();
            var context = new UProduseStoreContext(ClientController.ConnectToTestServer);

            //Act
            Client client = ClientController.RegistrationOfPerson<Client>(nameOfNewClient, passwordOrID, context) as Client;
            Employee employee = ClientController.RegistrationOfPerson<Employee>(nameOfNewEmp, passwordOrID, context) as Employee;


            Client clientNullName = ClientController.RegistrationOfPerson<Client>(null, passwordOrID, context) as Client;
            Client clientNullPassword = ClientController.RegistrationOfPerson<Client>(nameOfNewClient, null, context) as Client;
            Client clientNullPath = ClientController.RegistrationOfPerson<Client>(nameOfNewClient, passwordOrID, null) as Client;
            Client clientNullArguments = ClientController.RegistrationOfPerson<Client>(null, null, null) as Client;

            Client alreadyRegisteredClient = ClientController.RegistrationOfPerson<Client>(nameOfNewClient, passwordOrID, context) as Client;
            Client clientWhereTypeIsPerson = ClientController.RegistrationOfPerson<Person>(nameOfNewClient, passwordOrID, context) as Client;

            Employee employeeNullName = ClientController.RegistrationOfPerson<Employee>(null, passwordOrID, context) as Employee;
            Employee employeetNullPassword = ClientController.RegistrationOfPerson<Employee>(nameOfNewEmp, null, context) as Employee;
            Employee employeeNullPath = ClientController.RegistrationOfPerson<Employee>(nameOfNewEmp, passwordOrID, null) as Employee;
            Employee employeetNullArguments = ClientController.RegistrationOfPerson<Employee>(null, null, null) as Employee;
            Employee alreadyRegisteredEmployee = ClientController.RegistrationOfPerson<Employee>(nameOfNewEmp, passwordOrID, context) as Employee;
            Employee employeetWhereTypeIsPerson = ClientController.RegistrationOfPerson<Person>(nameOfNewEmp, passwordOrID, context) as Employee;

            //Assert
            Assert.IsNotNull(client);
            Assert.IsNotNull(employee);

            Assert.IsNull(employeeNullPath);
            Assert.IsNull(clientNullPath);
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

        [TestMethod()]
        public void WriteProductInFileBasketTest()
        {
            //Arrange
            string name = Guid.NewGuid().ToString();
            Client client = new Client(name, "PASSWORD" + name);

            Product product = new Product("PROD" + name, 1500);
            Product productNULL = null;

            List<Product> productS = new List<Product>();
            productS.Add(new Product("PROD1" + name, 15));
            productS.Add(new Product("PROD2" + name, 15));

            List<Product> productS_null = null;

            //Act
            ClientController.WriteProductInFileBasket(client, product);
            var list_With_Product_OfClient = ClientController.ReadFileWithListOfProduct(client, false);
            ClientController.WriteProductInFileBasket(client, productS);
            var list_With_Products_OfClient = ClientController.ReadFileWithListOfProduct(client, false);

            bool whenClientNull = ClientController.WriteProductInFileBasket(null, product);
            bool whenProductNull = ClientController.WriteProductInFileBasket(client, productNULL);
            bool whenClient_And_ProductNull = ClientController.WriteProductInFileBasket(null, productNULL);
            bool whenProductS_Null = ClientController.WriteProductInFileBasket(client, productS_null);
            bool whenClient_With_ProductSNull = ClientController.WriteProductInFileBasket(null, productS);


            //Assert
            Assert.IsNotNull(list_With_Product_OfClient);
            Assert.IsNotNull(list_With_Products_OfClient);

            Assert.IsFalse(whenClientNull);
            Assert.IsFalse(whenProductNull);
            Assert.IsFalse(whenClient_And_ProductNull);
            Assert.IsFalse(whenProductS_Null);
            Assert.IsFalse(whenClient_With_ProductSNull);
        }
    }
}