namespace Api.Error
{
    public class ApiReponse
    {
        public ApiReponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => " you have made A bad request",
                401 => " you are not Authenticated",
                405 => "you are not Authorized ",
                404 => "no reponse found ",
                500 => "Server error occured",
                _ => null
            };
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
