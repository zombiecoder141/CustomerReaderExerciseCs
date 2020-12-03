﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CustomerReader
{
    public class CustomerReader
    {
        
        static void Main(string[] args)
        {

             string filePath = "..\\..\\..\\doc\\";

            CustomerReader cr = new CustomerReader();
      

            cr.ReadCustomersCsv(filePath+"customers.csv");
            cr.ReadCustomersXml(filePath + "customers.xml");
            cr.ReadCustomersJson(filePath+"customers.json");

            Console.WriteLine("Added this many customers: " + cr.getCustomers().Count + "\n");
            cr.DisplayCustomers();
            Console.ReadLine();
        }
            
       
        private readonly List<Customer> customers;

        public CustomerReader()
        {
            customers = new List<Customer>();
        }

        public List<Customer> getCustomers()
        {
            return customers;
        }

        /*
         * This method reads customers from a CSV file and puts them into the customers list.
         */
        public void ReadCustomersCsv(string customer_file_path)
        {

            try
            {
                StreamReader br = new StreamReader(File.Open(customer_file_path, FileMode.Open));
                string line = br.ReadLine();

                while (line != null)
                {
                    //String[] attributes = line.split(" , ");
                    String[] attributes = line.Split(',');

                    Customer customer = new Customer();
                    customer.email = attributes[0];
                    customer.fn = attributes[1];
                    customer.ln = attributes[2];
                    customer.phone = attributes[3];
                    customer.streetAddress = attributes[4];
                    customer.city = attributes[5];
                    customer.state = attributes[6];
                    customer.zipCode = attributes[7];

                    customers.Add(customer);
                    line = br.ReadLine();
                }
            }
            catch (FileNotFoundException e)
            {
                Console.Write("Unable to find customer CSV File");
                Console.Write(e.StackTrace);
            }
            catch (IOException ex)
            {
                Console.Write("Unable to read cusomer CSV File");
                Console.Write(ex.StackTrace);
            }
        }

        public void ReadCustomersXml(String customerFilePath)
        {
            try
            {
                var doc = new XmlDocument();
                doc.Load(customerFilePath);

                XmlNodeList nList = doc.GetElementsByTagName("Customers");

                for (int temp = 0; temp < nList.Count; temp++)
                {
                    XmlNode nNode = nList[temp];
                    Console.WriteLine("\nCurrent Element :" + nNode.Name);
                    if (nNode.NodeType == XmlNodeType.Element)
                    {
                        Customer customer = new Customer();
                        XmlElement eElement = (XmlElement)nNode;

                        customer.email = eElement.GetElementsByTagName("Email").Item(0).InnerText;
                        customer.fn = eElement.GetElementsByTagName("FirstName").Item(0).InnerText;
                        customer.ln = eElement.GetElementsByTagName("LastName").Item(0).InnerText;
                        customer.phone = eElement.GetElementsByTagName("PhoneNumber").Item(0).InnerText;

                        XmlElement aElement = (XmlElement)eElement.GetElementsByTagName("Address").Item(0);

                        customer.streetAddress = aElement.GetElementsByTagName("StreetAddress").Item(0).InnerText;
                        customer.city = aElement.GetElementsByTagName("City").Item(0).InnerText;
                        customer.state = aElement.GetElementsByTagName("State").Item(0).InnerText;
                        customer.zipCode = aElement.GetElementsByTagName("ZipCode").Item(0).InnerText;

                        customers.Add(customer);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write("Unable to read cusomer XML File");
                Console.Write(e.StackTrace);
            }
        }


        public void ReadCustomersJson(String customersFilePath)
        {
            //JsonTextReader reader = (new JsonTextReader(System.IO.File.OpenText(customersFilePath));
            //JObject reader = JObject.Parse(File.ReadAllText(customersFilePath));
            JsonTextReader reader = new JsonTextReader(System.IO.File.OpenText(customersFilePath));

            try
            {
                JArray obj = (JArray)JToken.ReadFrom(reader);
                //JArray a = (JArray)reader);
                //

                foreach (JObject o in obj)
                {
                    Customer customer = new Customer();
                    JObject record = (JObject)o;

                    String email = (String)record["Email"];
                    customer.email = email;

                    String firstName = (String)record["FirstName"];
                    customer.fn = firstName;

                    String lastName = (String)record["LastName"];
                    customer.ln = lastName;

                    String phone = (String)record["PhoneNumber"];
                    customer.phone = phone;

                    JObject address = (JObject)record["Address"];

                    String streetAddress = (String)address["StreetAddress"];
                    customer.streetAddress = streetAddress;

                    String city = (String)address["City"];
                    customer.city = city;

                    String state = (String)address["State"];
                    customer.state = state;

                    String zipCode = (String)address["ZipCode"];
                    customer.zipCode = zipCode;

                    customers.Add(customer);
                }
            }
            catch (FileNotFoundException e)
            {
                Console.Write("Unable to find cusomer Json File");
                Console.Write(e.StackTrace);
            }
            catch (IOException e)
            {
                Console.Write(e.StackTrace);
            }
            catch (JsonException e)
            {
                Console.Write(e.StackTrace);
            }
        }

        public void DisplayCustomers()
        {
            foreach (Customer customer in this.customers)
            {
                String customerString = "";
                customerString += "Email: " + customer.email.ToLower() + "\n";
                customerString += "First Name: " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(customer.fn.ToLower()) + "\n";
                customerString += "Last Name: " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(customer.ln.ToLower()) + "\n";
                customerString += "Full Name: " + customer.GetFormattedName() + "\n";
                customerString += "Phone Number: " + customer.phone + "\n";
                customerString += "Street Address: " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(customer.streetAddress.ToLower()) + "\n";
                customerString += "City: " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(customer.city.ToLower()) + "\n";
                customerString += "State: " + customer.state.ToUpper() + "\n";
                customerString += "Zip Code: " + customer.zipCode + "\n";

                Console.WriteLine(customerString);
            }
        }
    }
}
