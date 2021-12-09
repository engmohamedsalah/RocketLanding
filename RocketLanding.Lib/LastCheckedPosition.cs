using System;
using System.Drawing;

namespace RocketLanding.Lib
{
    public class LastCheckedPosition
    {
        public   Rectangle LastCheckedArea { get; private set; }
        public Guid RocketId { get; private set; }
        public LastCheckedPosition()
        {
            LastCheckedArea = Rectangle.Empty;
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
        public bool CanClashWithPoint(Point point, Guid rocketId)
        {
            return LastCheckedArea.Contains(point) && rocketId != RocketId;
            
        }
        /// <summary>
        /// Updates the last checking spot.
        /// </summary>
        /// <param name="spot">The spot.</param>
        /// <param name="rocketId">The rocket identifier.</param>
        public void Update(Rectangle spot, Guid rocketId)
        {
            LastCheckedArea = spot;
            RocketId = rocketId;
        }
    }
}
