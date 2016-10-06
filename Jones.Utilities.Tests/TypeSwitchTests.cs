using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Jones.Utilities.Tests
{
    [TestClass]
    public class TypeSwitchTests
    {
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void TypeSwitch_TypeNotAdded_Throws()
        {
            new TypeSwitch<object>().Execute("uh oh");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TypeSwitch_AddTypeTwice_Throws()
        {
            new TypeSwitch<object>()
                .Add<string>(s => { })
                .Add<string>(s => { })
                .Execute(123);
        }

        [TestMethod]
        public void TypeSwitch_CorrectlySetUp_FindsCorrectType()
        {
            int val = 0;

            new TypeSwitch<object>()
                .Add<string>(s => { })
                .Add<int>(i => val = i)
                .Execute(123);

            Assert.AreEqual(val, 123);
        }
    }
}
