namespace RocketLanding.Lib
{
    public class ErrorMessage
    {
        public const string LandingAreaSizeError = "Landing Area Height or Width is 0";
        public const string LandingPlatformSizeError = "Landing Platform Height or Width is 0";
        public const string LandingPlatformOutOfArea = "Landing Platform is Out Of Area ";
        public const string SeparationUnitsLessIsInvalid = "Separation Units should be greater than zero and less than landing area size";
    }
}