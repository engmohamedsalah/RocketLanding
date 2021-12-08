using System;
using System.Drawing;

namespace RocketLanding.Lib
{
    public class LandingChecker
    {
        public static Rectangle LandingArea { get; private set; }
        public Rectangle LandingPlatform { get; private set; }
        public Rectangle LastCheckedSpot { get; private set; }
        public int SeparationUnits { get; private set; }
        public int SeparationDiameter { get; private set; }

        private static readonly object _locker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="LandingChecker"/> class.
        /// </summary>
        /// <param name="landingPlatform">The landing platform.</param>
        public LandingChecker(Rectangle landingPlatform) :
            this(CreateLandingAreaDefault(), landingPlatform)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LandingChecker"/> class.
        /// </summary>
        /// <param name="landingArea">The landing area.</param>
        /// <param name="landingPlatform">The landing platform.</param>
        /// <param name="separationUnits">The separation units.</param>
        public LandingChecker(Rectangle landingArea, Rectangle landingPlatform, int separationUnits = 1)
        {
            CheckAreaParameters(landingArea, landingPlatform);
            LandingArea = landingArea;
            LandingPlatform = landingPlatform;
            SeparationUnits = separationUnits;
            SeparationDiameter = (SeparationUnits * 2) + 1;
            LastCheckedSpot = Rectangle.Empty;
        }

        /// <summary>
        /// Checks the landing.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        public string CheckLanding(Point point)
        {
            if (!LandingPlatform.Contains(point))
                return LandingStatus.OutOfPlatform.ToDescriptionString();

            lock (_locker)
            {
                if (LastCheckedSpot.Contains(point))
                    return LandingStatus.Clash.ToDescriptionString();

                //if the last point at the edge the lastCheckPoint will include some invalid area
                //but because the is check of validity point before so no worries about in correct message
                LastCheckedSpot = new Rectangle(point.X - SeparationUnits, point.Y - SeparationUnits, SeparationDiameter, SeparationDiameter);
            }

            return LandingStatus.OkForLanding.ToDescriptionString();
        }

        #region private methods

        /// <summary>
        /// Creates the landing area with default size.
        /// </summary>
        /// <returns></returns>
        private static Rectangle CreateLandingAreaDefault()
        {
            return new Rectangle(Parameters.DefaultX, Parameters.DefaultX, Parameters.DefaultWidth, Parameters.DefaultHight);
        }

        /// <summary>
        /// Checks the area parameters.
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
        private void CheckAreaParameters(Rectangle landingArea, Rectangle landingPlatform)
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