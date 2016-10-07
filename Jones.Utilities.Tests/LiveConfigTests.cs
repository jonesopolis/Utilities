using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jones.Utilities.Tests
{
    [TestClass]
    public class LiveConfigTests
    {
        private const string _json = @"{ ""Test"": ""Hello"" }";
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
        [ExpectedException(typeof (FileNotFoundException))]
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

            config.Watch();

            File.WriteAllText(_file, @"{ ""Test"": ""Test"" }");

            Assert.IsTrue(success);
        }

        [TestMethod]
        public async Task LiveConfig_FileChanged_ConfigUpdated()
        {
            var tcs = new TaskCompletionSource<bool>();
            var config = new LiveConfig<ConfigObject>(_file);

            config.Changed += () => tcs.SetResult(true);
            config.Watch();

            File.WriteAllText(_file, @"{ ""Test"": ""Test"" }");
            
            if (await Task.WhenAny(tcs.Task, Task.Delay(2000)) == tcs.Task)
            {
                Assert.IsTrue(config.Configuration.Test == "Test");
            }
            else
            {
                throw new Exception();
            }
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