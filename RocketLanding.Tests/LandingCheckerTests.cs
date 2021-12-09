using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using AutoFixture;
using System.Linq;
using System.Threading.Tasks;
using RocketLanding.Lib;

namespace RocketLanding.Tests
{
    [TestClass]
    public class LandingCheckerTests
    {
        private Fixture _fixture;
        private Random _rand;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _rand = new Random();
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException), ErrorMessage.LandingAreaSizeError)]
        [TestMethod]
        public void LandingChecker_NoSizeOfLandingPlatform_ArgumentOutOfRangeExceptionExpected()
        {
            // Arrange
            var landingArea = new Rectangle(0, 0, 0, _rand.Next());
            var landingPlatform = CreateValidRandomLanding();

            // Act
            new LandingCheckerService(landingArea, landingPlatform);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException), ErrorMessage.LandingPlatformSizeError)]
        [TestMethod]
        public void LandingChecker_NoSizeOfLandingArea_ArgumentOutOfRangeExceptionExpected()
        {
            // Arrange
            var landingPlatform = new Rectangle(0, 0, 0, _rand.Next());
            var landingArea = CreateValidRandomLanding();

            // Act
            new LandingCheckerService(landingArea, landingPlatform);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException), ErrorMessage.LandingPlatformOutOfArea)]
        [TestMethod]
        public void LandingChecker_LandingPlatformOutSideLadingArea_ArgumentOutOfRangeExceptionExpected()
        {
            // Arrange

            var landingArea = CreateValidRandomLanding();
            var landingPlatform = new Rectangle(landingArea.X - _rand.Next(), landingArea.Y - _rand.Next(), landingArea.Width / 5, landingArea.Width / 5);
            // Act
            new LandingCheckerService(landingArea, landingPlatform);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException), ErrorMessage.LandingPlatformOutOfArea)]
        [TestMethod]
        public void LandingChecker_InvalidSeparationUnits_ArgumentOutOfRangeExceptionExpected()
        {
            // Arrange

            var landingArea = CreateValidRandomLanding();
            var landingPlatform = CreateValidRandomLanding();
            // Act
            new LandingCheckerService(landingArea, landingPlatform, -10);
        }

        [TestMethod]
        public void CreateLandingChecker_WithConfigureLandingPlatformOnly_ResultShouldSuccess()

        {
            // Arrange
            var landingPlatform = CreateValidRandomLanding();

            // Act
            var checker = new LandingCheckerService(landingPlatform);

            //Assert
            Assert.AreEqual(checker.LandingPlatform, landingPlatform);
        }

        [TestMethod]
        [DataRow(0, 2)]
        [DataRow(10, 20)]
        [DataRow(-5, -6)]
        [DataRow(200, 200)]
        public void CheckLanding__OutOfPlatform_ShouldReturnOutOfPlatformMSG(int x, int y)
        {
            // Arrange
            var landingPlatform = new Rectangle(5, 5, 3, 3);
            var checker = new LandingCheckerService(landingPlatform);
            var rocketId = _fixture.Create<Guid>();

            // Act
            var result = checker.CheckLandingAvailability(new Point(x, y), rocketId);

            //Assert
            Assert.AreEqual(result, LandingStatus.OutOfPlatform.ToDescriptionString());
        }

        [TestMethod]
        [DataRow(5, 5)]
        [DataRow(5, 6)]
        [DataRow(12, 12)]
        [DataRow(10, 10)]
        public void CheckLanding__LandingInsidePlatform_ShouldReturnOkToLandingMSG(int x, int y)
        {
            // Arrange
            var landingPlatform = new Rectangle(5, 5, 10, 10);
            var checker = new LandingCheckerService(landingPlatform);
            var rocketId = _fixture.Create<Guid>();

            // Act
            var result = checker.CheckLandingAvailability(new Point(x, y), rocketId);

            //Assert
            Assert.AreEqual(result, LandingStatus.OkForLanding.ToDescriptionString());
        }

        [TestMethod]
        [DataRow(5, 5)]
        [DataRow(5, 6)]
        [DataRow(6, 5)]
        [DataRow(6, 6)]
        public void CheckLanding__LandingInsideOccupiedArea_ShouldClashMSG(int x, int y)
        {
            // Arrange
            var landingPlatform = new Rectangle(5, 5, 10, 10);
            var checker = new LandingCheckerService(landingPlatform);
            var rocketId1 = _fixture.Create<Guid>();
            var rocketId2 = _fixture.Create<Guid>();


            var validPointResult = checker.CheckLandingAvailability(new Point(6, 6), rocketId1);

            // Act
            var result = checker.CheckLandingAvailability(new Point(x, y), rocketId2);

            //Assert
            Assert.AreEqual(validPointResult, LandingStatus.OkForLanding.ToDescriptionString());
            Assert.AreEqual(result, LandingStatus.Clash.ToDescriptionString());
        }

        [TestMethod]
        [DataRow(5, 5)]
        [DataRow(5, 6)]
        [DataRow(6, 5)]
        [DataRow(6, 6)]
        public void CheckLanding__SameRocketManyTime_ShouldReturnOkLandingMessage(int x, int y)
        {
            // Arrange
            var landingPlatform = new Rectangle(5, 5, 10, 10);
            var checker = new LandingCheckerService(landingPlatform);
            var rocketId1 = _fixture.Create<Guid>();



            // Act
            var firstResult = checker.CheckLandingAvailability(new Point(6, 6), rocketId1);
            var secondresult = checker.CheckLandingAvailability(new Point(x, y), rocketId1);
            

            //Assert
            Assert.AreEqual(firstResult, LandingStatus.OkForLanding.ToDescriptionString());
            Assert.AreEqual(secondresult, LandingStatus.OkForLanding.ToDescriptionString());
        }

        [TestMethod]
        public void CheckLanding__MultiThreadsOnSameValidPoint_OneCanLandOtherCannot()
        {
            // Arrange
            var landingPlatform = new Rectangle(5, 5, 10, 10);
            var checker = new LandingCheckerService(landingPlatform);
            var rocketId1 = _fixture.Create<Guid>();
            var rocketId2 = _fixture.Create<Guid>();

            // Act
            Task<string> rocket1 = Task.Factory.StartNew(() => checker.CheckLandingAvailability(new Point(6, 6), rocketId1));

            Task<string> rocket2 = Task.Factory.StartNew(() => checker.CheckLandingAvailability(new Point(6, 6), rocketId2));

            Task.WaitAll(rocket1, rocket2);

            //Assert
            var okLandingMessage =
                rocket1.Result == LandingStatus.OkForLanding.ToDescriptionString()
                || rocket2.Result == LandingStatus.OkForLanding.ToDescriptionString();

            var clashMessageExist =
                rocket1.Result == LandingStatus.Clash.ToDescriptionString()
                || rocket2.Result == LandingStatus.Clash.ToDescriptionString();

            Assert.IsTrue(okLandingMessage);
            Assert.IsTrue(clashMessageExist);
        }

        [TestMethod]
        public void CheckLanding__MultiThreadsDifferentValidPoints_TwoCanLand()
        {
            // Arrange
            var landingPlatform = new Rectangle(5, 5, 10, 10);
            var checker = new LandingCheckerService(landingPlatform);
            var rocketId1 = _fixture.Create<Guid>();
            var rocketId2 = _fixture.Create<Guid>();


            // Act
            Task<string> rocket1 = Task.Factory.StartNew(() => checker.CheckLandingAvailability(new Point(6, 6), rocketId1));

            Task<string> rocket2 = Task.Factory.StartNew(() => checker.CheckLandingAvailability(new Point(12, 13),rocketId2));

            Task.WaitAll(rocket1, rocket2);

            //Assert
            var okLandingMessage = LandingStatus.OkForLanding.ToDescriptionString();

            Assert.AreEqual(rocket1.Result, okLandingMessage);
            Assert.AreEqual(rocket2.Result, okLandingMessage);
        }

        #region Private Methods

        private Rectangle CreateValidRandomLanding()
        {
            return _fixture
                .Build<Rectangle>()
                .With(a => a.X, _rand.Next(0, DefaultParameters.DefaultWidth))
                .With(a => a.Y, _rand.Next(0, DefaultParameters.DefaultHight))
                .Create();
        }

        #endregion Private Methods
    }
}