using FileHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Asserts
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            FileHelperEngine<Result> engine = new FileHelperEngine<Result>();
            List<Result> results = new List<Result>();
            Product product = new Product("prod123_prd", "ProductName");
            results.Add(new Result().AddHeadings());
            results.Add(Log.AssertEquals("this", "that", product, "String FAIL", "Some comment"));
            results.Add(Log.AssertEquals("this", "this", product, "String PASS", "Some comment"));
            results.Add(Log.AssertEquals(1, 2, product, "int FAIL"));
            results.Add(Log.AssertEquals(1, 1, product, "int PASS"));
            results.Add(Log.AssertEquals(1, 2.5, product, "double FAIL"));
            results.Add(Log.AssertEquals(2.0, 2.5, product, "double FAIL"));
            results.Add(Log.AssertEquals(2.5, 2.5, product, "double PASS"));
            results.Add(Log.AssertEquals(true, false, product, "bool FAIL"));
            results.Add(Log.AssertEquals(true, true, product, "bool PASS"));
            results.Add(Log.assertTrue(true, product, "assertTrue PASS"));
            results.Add(Log.assertTrue(false, product, "assertTrue FAIL"));
            results.Add(Log.AssertFalse(false, product, "assertFalse PASS"));
            results.Add(Log.AssertFalse(true, product, "assertFalse FAIL"));
            results.Add(Log.AssertEquals("test", (string)null, product, "null Expected FAIL"));
            results.Add(Log.AssertEquals((string)null, (string)null, product, "both null PASS"));
            engine.WriteFile("output.txt", results);
        }
        [TestMethod]
        public void CustomFluentWait()
        {
            IWebDriver Driver = new ChromeDriver();
            string url = "file:///C:/Users/a038760/Desktop/test.htm";
            Driver.Manage().Window.Maximize();
            Driver.Navigate().GoToUrl(url);
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(60));
            wait.Until(drv => drv.FindElement(By.Id("count")).Text.Equals("2"));
            Driver.Close();
            Driver.Quit();
        }
    }
    [DelimitedRecord("|")]
    class Result
    {
        public string ProductId;
        public string ProductName;
        public string Validation;
        public string _Result;
        public string Actual;
        public string Expected;
        public string Comment;

        public Result AddHeadings()
        {
            ProductId = "ProductID";
            ProductName = "Product Name";
            Validation = "Validation";
            _Result = "Result";
            Actual = "Actual";
            Expected = "Expected";
            Comment = "Comment";

            return this;
        }
    }
    class Product
    {
        public string ProductId;
        public string ProductName;

        public Product(string productId, string productName)
        {
            ProductId = productId;
            ProductName = productName;
        }
    }
    class Validation
    {
        public string ValidationName;
        public string Result;
        public string Actual;
        public string Expected;
        public string Comment;
    }
    static class Log
    {
        static private readonly string FAIL = "FAIL";
        static private readonly string PASS = "PASS";

        static public Result AssertEquals<T>(T actual, T expected, Product product, string label, string comment = "")
        {
            string result = FAIL;
            if (actual == null & expected == null)
            {
                result = PASS;
            }
            else if (expected != null && actual.Equals(expected))
            {
                result = PASS;
            }

            Validation validation = new Validation();
            validation.ValidationName = label;
            validation.Comment = comment;
            validation.Actual = actual == null ? string.Empty : actual.ToString();
            validation.Expected = expected == null ? string.Empty : expected.ToString();
            validation.Result = result;

            return WriteMessage(product, validation);
        }
        static public Result assertTrue(bool actual, Product product, string label, string comment = "")
        {
            return AssertEquals(actual, true, product, label, comment);
        }
        static public Result AssertFalse(bool actual, Product product, string label, string comment = "")
        {
            return AssertEquals(actual, false, product, label, comment);
        }
        static public Result WriteMessage(Product product, Validation validation)
        {
            Trace.WriteLine($"[{validation.Result}] {validation.ValidationName}");
            Trace.WriteLine($"       Expected: {validation.Expected}");
            Trace.WriteLine($"         Actual: {validation.Actual}");

            return new Result()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Validation = validation.ValidationName,
                _Result = validation.Result,
                Actual = validation.Actual,
                Expected = validation.Expected,
                Comment = validation.Comment
            };
        }
    }
}
