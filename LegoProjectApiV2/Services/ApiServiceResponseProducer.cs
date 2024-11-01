namespace LegoProjectApiV2.Services
{
    public class ApiServiceResponseProducer
    {
        public ApiServiceResponse<T> ProduceResponse<T>(string message, T result, Exception exception = null)
        {
            ApiServiceResponse<T> apiServiceResponse = new ApiServiceResponse<T>();

            apiServiceResponse.ResultMessage = message;

            if (exception != null)
            {
                if(exception.InnerException != null)
                    apiServiceResponse.ErrorMessage = exception.InnerException.Message;
                else
                    apiServiceResponse.ErrorMessage = exception.Message;
                apiServiceResponse.StackTrace = exception.StackTrace;
            }

            if (!EqualityComparer<T>.Default.Equals(result, default(T)) && exception == null)
                apiServiceResponse.IsSuccessful = true;

            apiServiceResponse.Result = result;

            return apiServiceResponse;
        }
    }
}
