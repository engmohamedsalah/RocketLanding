using Microsoft.VisualStudio.TestTools.UnitTesting;
using RocketLanding.Lib;

namespace RocketLanding.Tests
{
    [TestClass]
    public class LandingStatusExtensionsTests
    {
        [TestMethod()]
        [DataRow(LandingStatus.Clash, "clash")]
        [DataRow(LandingStatus.OutOfPlatform, "out of platform")]
        [DataRow(LandingStatus.OkForLanding, "ok for landing")]
        public void ToDescriptionStringTest(LandingStatus val, string expected)
        {
            var result = LandingStatusExtensions.ToDescriptionString(val);
            Assert.AreEqual(result, expected);
        }
    }
}