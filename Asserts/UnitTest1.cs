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
            Product product = new Product() { ProductId = "prod123_prd", ProductName = "ProductName" };
            results.Add(new Result().addHeadings());
            results.Add(Log.assertEquals("this", "that", product, new Validation() { ValidationName = "String FAIL", Comment = "Some comment" }));
            results.Add(Log.assertEquals("this", "this", product, new Validation() { ValidationName = "String PASS", Comment = "Some comment" }));
            results.Add(Log.assertEquals(1, 2, product, new Validation() { ValidationName = "int FAIL" }));
            results.Add(Log.assertEquals(1, 1, product, new Validation() { ValidationName = "int PASS" }));
            results.Add(Log.assertEquals(1, 2.5, product, new Validation() { ValidationName = "double FAIL" }));
            results.Add(Log.assertEquals(2.0, 2.5, product, new Validation() { ValidationName = "double FAIL" }));
            results.Add(Log.assertEquals(2.5, 2.5, product, new Validation() { ValidationName = "double PASS" }));
            results.Add(Log.assertEquals(true, false, product, new Validation() { ValidationName = "bool FAIL" }));
            results.Add(Log.assertEquals(true, true, product, new Validation() { ValidationName = "bool PASS" }));
            results.Add(Log.assertTrue(true, product, new Validation() { ValidationName = "assertTrue PASS" }));
            results.Add(Log.assertTrue(false, product, new Validation() { ValidationName = "assertTrue FAIL" }));
            results.Add(Log.assertFalse(false, product, new Validation() { ValidationName = "assertFalse PASS" }));
            results.Add(Log.assertFalse(true, product, new Validation() { ValidationName = "assertFalse FAIL" }));
            results.Add(Log.assertEquals("test", (string)null, product, new Validation() { ValidationName = "null Expected FAIL" }));
            results.Add(Log.assertEquals((string)null, (string)null, product, new Validation() { ValidationName = "both null PASS" }));
            // test commits
            // this is a new commit
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
        public Result addHeadings()
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
        static readonly string FAIL = "FAIL";
        static readonly string PASS = "PASS";
        static public Result assertEquals<T>(T actual, T expected, Product product, Validation validation)
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

            validation.Actual = actual == null ? string.Empty : actual.ToString();
            validation.Expected = expected == null ? string.Empty : expected.ToString();
            validation.Result = result;

            return WriteMessage(product, validation);
        }
        static public Result assertTrue(bool actual, Product product, Validation validation)
        {
            return assertEquals(actual, true, product, validation);
        }
        static public Result assertFalse(bool actual, Product product, Validation validation)
        {
            return assertEquals(actual, false, product, validation);
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