namespace Infrastructure.Exception
{
    public class CustomException : System.Exception
    {
        public int code;
        public CustomException(string message, int code = 2) : base(message)
        {
            this.code = code;
        }

        public CustomException(string message, System.Exception innerException, int code = 2) : base(message, innerException)
        {
            this.code = code;
        }
    }
}
