using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using UlyanovProduseStore.BL.Model;
using System.IO;

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
            Employee employee = new Employee(Guid.NewGuid().ToString());

            //Act
            bool resultClien = ClientController.FindPerson(client); //Проверка данных о новом экземпляре. Должно быть False т.к клиент новый.
            bool resultEmp = ClientController.FindPerson(employee);

            bool resultClint2 = ClientController.FindPerson(client); //И вновь, но должно быть True т.к он уже был сохранён.
            bool resultEmp2 = ClientController.FindPerson(employee);
            File.Delete(client.GetPathToUserData()); //Стоит ли захламлять пк этими данными? Я думаю - нет.
            File.Delete(employee.GetPathToUserData());

            //Assert
            Assert.IsFalse(resultClien);
            Assert.IsTrue(resultClint2);
            Assert.IsFalse(resultEmp);
            Assert.IsTrue(resultEmp2);
        }

        [TestMethod()]
        public void BuyTest()
        {
            //Arrange
            List<Product> products = new List<Product>() { new Product("p1", 10, 1), new Product("p2", 50, 2) };
            Client client = new Client("Garry", products, 1.00F, 500);
            Client clientNull = null;

            //Act
            bool IsBuyComplete = ClientController.Buy(client);
            bool IsBuyCompleteNull = ClientController.Buy(clientNull);

            //Assert
            Assert.IsTrue(IsBuyComplete);
            Assert.IsFalse(IsBuyCompleteNull);
        }

        [TestMethod()]
        public void UpBalanceTest()
        {
            //Arrange
            Client client = new Client("NewClient");
            Client clientNull = null;

            //Act
            bool IsBalanceUpped = ClientController.UpBalance(client, 500);
            bool IsBalanceUppedNullClient = ClientController.UpBalance(clientNull, 500);
            bool IsBalanceUppedMinusBalance = ClientController.UpBalance(client, -100500);

            //Assert
            Assert.IsTrue(IsBalanceUpped);
            Assert.IsFalse(IsBalanceUppedNullClient);
            Assert.IsFalse(IsBalanceUppedMinusBalance);
        }
    }
}