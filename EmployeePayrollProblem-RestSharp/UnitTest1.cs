// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitTest1.cs" company="Bridgelabz">
//   Copyright © 2018 Company
// </copyright>
// <creator Name="Dheer Singh Meena"/>
// --------------------------------------------------------------------------------------------------------------------
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace EmployeePayrollProblem_RestSharp
{
    [TestClass]
    public class UnitTest1
    {
        RestClient client;

        [TestInitialize]
        public void SetUp()
        {
            //Initialize the base URL to execute requests made by the instance
            client = new RestClient("http://localhost:4000");
        }
        private IRestResponse GetEmployeeList()
        {
            //Arrange
            //Initialize the request object with proper method and URL
            RestRequest request = new RestRequest("/Employees/list", Method.GET);
            //Act
            // Execute the request
            IRestResponse response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// UC1 Retrieve all employee details in the json file
        /// </summary>
        [TestMethod]
        public void OnCallingGetAPI_ReturnEmployeeList()
        {
            IRestResponse response = GetEmployeeList();
            // Check if the status code of response equals the default code for the method requested
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            // Convert the response object to list of employees
            List<Employee> employeeList = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(6, employeeList.Count);
            foreach (Employee emp in employeeList)
            {
                Console.WriteLine("Id: " + emp.Id + "\t" + "Name: " + emp.Name + "\t" + "Salary: " + emp.Salary);
            }
        }
        /// <summary>
        /// UC2 Ability to add new employee to the json file in JSON server and return the same
        /// </summary>
        [TestMethod]
        public void OnCallingPostAPI_ReturnEmployeeObject()
        {
            //Arrange
            ///Initialize the request for POST to add new employee
            RestRequest request = new RestRequest("/Employees/list", Method.POST);
            JsonObject jsonObj = new JsonObject();
            jsonObj.Add("name", "Ritik");
            jsonObj.Add("salary", "50000");
            jsonObj.Add("id", "7");
            ///Added parameters to the request object such as the content-type and attaching the jsonObj with the request
            request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

            //Act
            IRestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Employee employee = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Ritik", employee.Name);
            Assert.AreEqual("50000", employee.Salary);
            Console.WriteLine(response.Content);
        }
        /// <summary>
        /// UC3 Ability to adding multiple employees to the json file using JSON server and returns the same
        /// </summary>
        [TestMethod]
        public void OnCallingPostAPIForAEmployeeListWithMultipleEMployees_ReturnEmployeeObject()
        {
            // Arrange
            List<Employee> employeeList = new List<Employee>();
            employeeList.Add(new Employee { Name = "Radha", Salary = "85536" });
            employeeList.Add(new Employee { Name = "Watson", Salary = "120123" });
            employeeList.Add(new Employee { Name = "Christiano Ronaldo", Salary = "123456" });
            //Iterate the loop for each employee
            foreach (var emp in employeeList)
            {
                ///Initialize the request for POST to add new employee
                RestRequest request = new RestRequest("/Employees/list", Method.POST);
                JsonObject jsonObj = new JsonObject();
                jsonObj.Add("name", emp.Name);
                jsonObj.Add("salary", emp.Salary);
                ///Added parameters to the request object such as the content-type and attaching the jsonObj with the request
                request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

                //Act
                IRestResponse response = client.Execute(request);

                //Assert
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                Employee employee = JsonConvert.DeserializeObject<Employee>(response.Content);
                Assert.AreEqual(emp.Name, employee.Name);
                Assert.AreEqual(emp.Salary, employee.Salary);
                Console.WriteLine(response.Content);
            }
        }
    }
}
