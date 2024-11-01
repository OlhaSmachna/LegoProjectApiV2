namespace LegoProjectApiV2.Services
{
    public class ApiServiceResponse<T>
    {
        public bool IsSuccessful { get; set; }

        public string ErrorMessage { get; set; }

        public string StackTrace { get; set; }

        public string ResultMessage { get; set; }

        public T Result { get; set; }
    }
}
