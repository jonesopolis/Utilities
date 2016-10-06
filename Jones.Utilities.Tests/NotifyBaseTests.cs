using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jones.Utilities.Tests
{
    [TestClass]
    public class NotifyBaseTests
    {
        [TestMethod]
        public void NotifyBase_NothingChanged_DoesNotFire()
        {
            int i = 0;
            var notify = new NotifyMock();

            notify.Test = "Hello";
            notify.PropertyChanged += (s, e) => i++;
            notify.Test = "Hello";

            Assert.IsTrue(i == 0);
        }

        [TestMethod]
        public void NotifyBase_PropertyChanged_Fires()
        {
            int i = 0;
            var notify = new NotifyMock();

            notify.Test = "Hello";
            notify.PropertyChanged += (s, e) => i++;
            notify.Test = "Hello Again";

            Assert.IsTrue(i == 1);
        }

        [TestMethod]
        public void NotifyBase_PropertyChanged_EventHasCorrectPropertyName()
        {
            var notify = new NotifyMock();

            string name = null;

            notify.Test = "Hello";
            notify.PropertyChanged += (s, e) => name = e.PropertyName;
            notify.Test = "Hello Again";

            Assert.IsTrue(name == nameof(NotifyMock.Test));
        }
    }

    internal class NotifyMock : NotifyBase
    {
        private string _test;
        public string Test
        {
            get { return _test; }
            set { SetProperty(ref _test, value); }
        }
    }
}
