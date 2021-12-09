using System.ComponentModel;

namespace RocketLanding.Lib
{
    /// <summary>
    /// the enmus for the message that should return for any rocket as for landing
    /// </summary>
    public enum LandingStatus
    {
        [Description("ok for landing")]
        OkForLanding,

        [Description("clash")]
        Clash,

        [Description("out of platform")]
        OutOfPlatform
    }

    public static class LandingStatusExtensions
    {
        /// <summary>
        /// Converts from enum to description string.
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns></returns>
        public static string ToDescriptionString(this LandingStatus val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}