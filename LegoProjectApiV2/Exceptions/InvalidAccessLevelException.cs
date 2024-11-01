namespace LegoProjectApiV2.Exceptions
{
    public class InvalidAccessLevelException : Exception
    {
        public int StatusCode { get; }
        public InvalidAccessLevelException() { }
        public InvalidAccessLevelException(string message) : base(message)
        {
            StatusCode = 40100;
        }
    }
}
