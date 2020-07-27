using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UlyanovProduseStore.BL.Model.Tests
{
    [TestClass()]
    public class CreateEmployeeTests
    {
        [TestMethod()]
        public void EmployeeTest()
        {
            //Arrange
            string name = Guid.NewGuid().ToString();

            //Act
            Employee employee = new Employee(name, name + "SECONdNAME");

            //Assert
            Assert.IsNotNull(employee);
            Assert.AreEqual(name, employee.ToString());
        }
    }
}