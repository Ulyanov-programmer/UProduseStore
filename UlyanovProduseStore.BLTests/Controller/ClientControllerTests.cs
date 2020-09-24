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
            string passwordOrID = Guid.NewGuid().ToString();
            string wrongPassword = passwordOrID + "X6X6X6";

            //Act
            //Сохранение новых пользователей.
            //Client currentClient = ClientController.RegistrationOfPerson<Client>(nameOfClient, passwordOrID) as Client;
            //Employee newEmployee = ClientController.RegistrationOfPerson<Employee>(nameOfEmp, passwordOrID) as Employee;

            ////Изменение и сохранение данных текущего клиента.
            //ClientController.UpBalance(currentClient, 500);

            ////Загрузка данных сохранённого клиента. 
            //Client loadedClient = ClientController.LoadOfPerson<Client>(nameOfClient, passwordOrID) as Client;

            //Client loadedClientWithWrongPassword = ClientController.LoadOfPerson<Client>(nameOfClient, wrongPassword) as Client;
            //Employee loadedEmployee = ClientController.LoadOfPerson<Employee>(nameOfEmp, passwordOrID) as Employee;

            ////Assert
            ////Сравнение данных текущего клиента и клиента загружаемого. Должны быть равны т.к данные загружаемого были созданы на основе текущего.
            //Assert.AreEqual(ClientController.GetBalance(currentClient), ClientController.GetBalance(loadedClient));
            //Assert.IsNull(loadedClientWithWrongPassword);
            //Assert.AreEqual(loadedClient.ToString(), loadedClient.ToString());

            //Assert.AreEqual(newEmployee.ToString(), loadedEmployee.ToString());
        }

        //[TestMethod()]
        //public void BuyTest()
        //{
        //    //Arrange
        //    List<Product> products = new List<Product>() { new Product("product1", 10, 0), new Product("product2", 50, 1) };
        //    Client client = new Client("Garry","X", products, 1.00F, 500);

        //    var SumCost = products.Select(x => ProductController.GetCost(x))
        //                          .Sum();
        //    decimal BalanceBeforeBuy = ClientController.GetBalance(client);

        //    Client clientNull = null;

        //    //Act
        //    bool IsBuyComplete = ClientController.Buy(client);
        //    bool IsBuyCompleteNull = ClientController.Buy(clientNull);

        //    //Assert
        //    Assert.IsTrue(IsBuyComplete);
        //    Assert.AreEqual(BalanceBeforeBuy - SumCost, ClientController.GetBalance(client));
        //    Assert.IsFalse(IsBuyCompleteNull);
        //}

        //[TestMethod()]
        //public void UpBalanceTest()
        //{
        //    //Arrange
        //    Client client = new Client("NewClient", "X");
        //    decimal clientBalanceBeforeUp = ClientController.GetBalance(client);
        //    Client clientNull = null;

        //    //Act
        //    bool IsBalanceUpped = ClientController.UpBalance(client, 500);
        //    bool IsBalanceUppedNullClient = ClientController.UpBalance(clientNull, 500);
        //    bool IsBalanceUppedMinusBalance = ClientController.UpBalance(client, -100500);

        //    //Assert
        //    Assert.IsTrue(IsBalanceUpped);
        //    Assert.AreEqual(clientBalanceBeforeUp + 500, ClientController.GetBalance(client));
        //    Assert.IsFalse(IsBalanceUppedNullClient);
        //    Assert.IsFalse(IsBalanceUppedMinusBalance);
        //}

        //[TestMethod()]
        //public void AddProductInBasketTest()
        //{
        //    //Arrange
        //    Client client = new Client(Guid.NewGuid().ToString(), "XX102134"); //ПАРОЛЬ НЕ МОЖЕТ БЫТЬ МЕНЕЕ 5 СИМВОЛОВ!!11

        //    //Act
        //    ClientController.AddProductInBasket(client, new Product("product", 10, 0));
        //    string nameOfSavedProduct = ClientController.GetListOfProduct(client).First()
        //                                                                         .ToString();
        //    //Assert
        //    Assert.AreEqual("product", nameOfSavedProduct);
        //}

        //[TestMethod()]
        //public void DeleteProductFromBasketTest()
        //{
        //    //Arrange
        //    List<Product> products = new List<Product>
        //    {
        //        new Product("product1", 10, 0),
        //        new Product("product1", 10, 0),
        //        new Product("product2", 50, 1)
        //    };

        //    Client client = new Client("Ivan", "X", products, 1.00, 5000);
        //    Client clientNull = null;

        //    string nameOfProductBeDeleted = "product1";

        //    int CountOfProductsEligibleBeforeRemoval = ClientController.GetListOfProduct(client)
        //                                               .Where(prod => ProductController.GetName(prod) == nameOfProductBeDeleted)
        //                                               .Count(); // Количество продуктов с одним названием ДО удаления.

        //    //Act
        //    bool productIsDeleted = ClientController.DeleteProductFromBasket(client, "product1");
        //    bool productIsDeletedFromClientNull = ClientController.DeleteProductFromBasket(clientNull, "product1");

        //    int CountOfProductsEligibleAfterRemoval = ClientController.GetListOfProduct(client)
        //                                              .Where(prod => ProductController.GetName(prod) == nameOfProductBeDeleted)
        //                                              .Count(); // Количество продуктов с одним названием ПОСЛЕ удаления.

        //    //Assert
        //    Assert.IsTrue(productIsDeleted);
        //    Assert.IsFalse(productIsDeletedFromClientNull);
        //    Assert.AreNotEqual(CountOfProductsEligibleBeforeRemoval, CountOfProductsEligibleAfterRemoval);
        //}

        //[TestMethod()]
        //public void RegistrationOfPersonTest()
        //{
        //    //Arrange
        //    string nameOfNewClient = Guid.NewGuid().ToString();
        //    string nameOfNewEmp = Guid.NewGuid().ToString();
        //    string passwordOrID = Guid.NewGuid().ToString();

        //    //Act
        //    Client client = ClientController.RegistrationOfPerson<Client>(nameOfNewClient, passwordOrID) as Client;
        //    Employee employee = ClientController.RegistrationOfPerson<Employee>(nameOfNewEmp, passwordOrID) as Employee;

        //    Client clientNullName = ClientController.RegistrationOfPerson<Client>(null, passwordOrID) as Client;
        //    Client clientNullPassword = ClientController.RegistrationOfPerson<Client>(nameOfNewClient, null) as Client;
        //    Client clientNullArguments = ClientController.RegistrationOfPerson<Client>(null, null) as Client;
        //    Client alreadyRegisteredClient = ClientController.RegistrationOfPerson<Client>(nameOfNewClient, passwordOrID) as Client;
        //    Client clientWhereTypeIsPerson = ClientController.RegistrationOfPerson<Person>(nameOfNewClient, passwordOrID) as Client;

        //    Employee employeeNullName = ClientController.RegistrationOfPerson<Client>(null, passwordOrID) as Employee;
        //    Employee employeetNullPassword = ClientController.RegistrationOfPerson<Client>(nameOfNewClient, null) as Employee;
        //    Employee employeetNullArguments = ClientController.RegistrationOfPerson<Client>(null, null) as Employee;
        //    Employee alreadyRegisteredEmployee = ClientController.RegistrationOfPerson<Client>(nameOfNewClient, passwordOrID) as Employee;
        //    Employee employeetWhereTypeIsPerson = ClientController.RegistrationOfPerson<Person>(nameOfNewClient, passwordOrID) as Employee;

        //    //Assert
        //    Assert.IsNotNull(client);
        //    Assert.IsNotNull(employee);

        //    Assert.IsNull(clientNullName);
        //    Assert.IsNull(clientNullPassword);
        //    Assert.IsNull(clientNullArguments);
        //    Assert.IsNull(alreadyRegisteredClient);
        //    Assert.IsNull(clientWhereTypeIsPerson);
        //    Assert.IsNull(employeeNullName);
        //    Assert.IsNull(employeetNullPassword);
        //    Assert.IsNull(employeetNullArguments);
        //    Assert.IsNull(alreadyRegisteredEmployee);
        //    Assert.IsNull(employeetWhereTypeIsPerson);
        //}
    }
}