namespace cairn.Exceptions
{
    internal class UnknownAccountException : UnknownException
    {
        public UnknownAccountException() {}
        public UnknownAccountException(string message) : base(message) {}
        public UnknownAccountException(string message, System.Exception innerException) : base(message, innerException) {}
    }
}