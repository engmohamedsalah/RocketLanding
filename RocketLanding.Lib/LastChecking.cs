using System;
using System.Drawing;

namespace RocketLanding.Lib
{
    public class LastChecking
    {
        public Rectangle LastCheckedSpot { get; private set; }
        public Guid RocketId { get; private set; }
        public LastChecking()
        {
            LastCheckedSpot = Rectangle.Empty;
            RocketId = Guid.Empty;
        }
        /// <summary>
        /// Determines whether this instance can clash the specified point.
        /// </summary>
        /// <param name="point">The point(X,Y).</param>
        /// <param name="rocketId">The rocket identifier.</param>
        /// <returns>
        ///   <c>true</c> if this rocket can clash at a specified point; otherwise, <c>false</c>.
        /// </returns>
        public bool CanClash(Point point, Guid rocketId)
        {
            return LastCheckedSpot.Contains(point) && rocketId != RocketId;
            
        }
        /// <summary>
        /// Updates the last checking spot.
        /// </summary>
        /// <param name="spot">The spot.</param>
        /// <param name="rocketId">The rocket identifier.</param>
        public void Update(Rectangle spot, Guid rocketId)
        {
            LastCheckedSpot = spot;
            RocketId = rocketId;
        }
    }
}
