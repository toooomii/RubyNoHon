using NUnit.Framework;
using System;
using RubyNoHon;

namespace RubyNoHonTest
{
    [TestFixture()]
    public class Test
    {
        [Test()]
        public void TestCase()
        {
            Gear gear = new Gear(1, 1, 3, 1);
            Assert.AreEqual(5, gear.GearInch());
        }
    }
}
