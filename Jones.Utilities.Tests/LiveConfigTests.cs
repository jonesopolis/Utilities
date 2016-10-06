using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jones.Utilities.Tests
{
    [TestClass]
    public class LiveConfigTests
    {
        private string _json = @"{ ""Test"": ""Hello"" }";
        private string _file;

        [TestInitialize]
        public void Setup()
        {

            _file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{Guid.NewGuid()}.txt");
            File.WriteAllText(_file, _json);             
        }

        [TestMethod]
        public void LiveConfig_TestSetup_TempFileCreated()
        {
            Assert.IsTrue(File.Exists(_file));
        } 

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void LiveConfig_FileDoesntExist_Throws()
        {
            var config = new LiveConfig<ConfigObject>("test");
        }

        [TestMethod]
        public void LiveConfig_FileChanged_ChangedEventRaised()
        {
            bool success = false;
            var config = new LiveConfig<ConfigObject>(_file);
            config.Changed += () => success = true;

            File.WriteAllText(_file, @"{ ""Test"": ""Test"" }");

            Thread.Sleep(1000);

            Assert.IsTrue(success);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_file))
            {
                File.Delete(_file);
            }
        } 
    }

    internal class ConfigObject
    {
        public string Test { get; set; }
    }
}
