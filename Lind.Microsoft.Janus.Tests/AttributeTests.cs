using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lind.Microsoft.Janus.Tests
{
    [TestClass]
    public class AttributeTests
    {
        [TestMethod]
        public void TestTypeChange()
        {
            JanusAttribute attr = new JanusAttribute(new Uri("http://janus.wwidew.net/testattr"), "Test Attribute", typeof(string));
            dynamic dynAttr = attr;
            dynAttr /= typeof(int);
            Assert.AreEqual(typeof(int), attr.ValueType);
        }
    }
}
