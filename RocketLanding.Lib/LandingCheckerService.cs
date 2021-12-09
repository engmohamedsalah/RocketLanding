using System;
using System.Drawing;

namespace RocketLanding.Lib
{
    public class LandingCheckerService
    {
        public static Rectangle LandingArea { get; private set; }
        public Rectangle LandingPlatform { get; private set; }
        public LastChecking LastChecking { get; private set; }
        public int SeparationUnits { get; private set; }
        public int SeparationDiameter { get; private set; }

        private static readonly object _locker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="LandingCheckerService"/> class.
        /// </summary>
        /// <param name="landingPlatform">The landing platform.</param>
        public LandingCheckerService(Rectangle landingPlatform, int separationUnits = 1) :
            this(CreateDefaultLandingArea(), landingPlatform, separationUnits)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LandingCheckerService"/> class.
        /// </summary>
        /// <param name="landingArea">The landing area.</param>
        /// <param name="landingPlatform">The landing platform.</param>
        /// <param name="separationUnits">The separation units.</param>
        public LandingCheckerService(Rectangle landingArea, Rectangle landingPlatform, int separationUnits = 1)
        {
            CheckArguments(landingArea, landingPlatform);
            LandingArea = landingArea;
            LandingPlatform = landingPlatform;
            SeparationUnits = separationUnits;
            SeparationDiameter = (SeparationUnits * 2) + 1;
            LastChecking = new LastChecking();
        }

        /// <summary>
        /// Checks the landing.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        public string CheckLandingAvailability(Point point,Guid rocketId)
        {
            if (!LandingPlatform.Contains(point))
                return LandingStatus.OutOfPlatform.ToDescriptionString();

            lock (_locker)
            {
                if (LastChecking.CanClash(point, rocketId))
                    return LandingStatus.Clash.ToDescriptionString();

                var newSpot = new Rectangle(point.X - SeparationUnits, point.Y - SeparationUnits, SeparationDiameter, SeparationDiameter);
                LastChecking.Update(newSpot, rocketId);
            }

            return LandingStatus.OkForLanding.ToDescriptionString();
        }

        #region private methods

        /// <summary>
        /// Creates the landing area with default size. 100*100
        /// </summary>
        /// <returns></returns>
        private static Rectangle CreateDefaultLandingArea()
        {
            return new Rectangle(DefaultParameters.DefaultX, DefaultParameters.DefaultX, DefaultParameters.DefaultWidth, DefaultParameters.DefaultHight);
        }

        /// <summary>
        /// Checks Arguments.
        /// </summary>
        /// <param name="landingArea">The landing area.</param>
        /// <param name="landingPlatform">The landing platform.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// landingArea
        /// or
        /// landingPlatform
        /// or
        /// landingPlatform
        /// or
        /// SeparationUnits
        /// </exception>
        private void CheckArguments(Rectangle landingArea, Rectangle landingPlatform)
        {
            if (landingArea.Height == 0 || landingArea.Width == 0)
                throw new ArgumentOutOfRangeException(nameof(landingArea), ErrorMessage.LandingAreaSizeError);

            if (landingPlatform.Height == 0 || landingPlatform.Width == 0)
                throw new ArgumentOutOfRangeException(nameof(landingPlatform), ErrorMessage.LandingPlatformSizeError);

            if (!landingArea.Contains(LandingPlatform))
                throw new ArgumentOutOfRangeException(nameof(landingPlatform), ErrorMessage.LandingPlatformOutOfArea);

            if (SeparationUnits < 0 || SeparationUnits >= landingArea.Width)
                throw new ArgumentOutOfRangeException(nameof(SeparationUnits), ErrorMessage.SeparationUnitsLessIsInvalid);
        }

        #endregion private methods
    }
}