namespace Imago.IO
{
    internal static class ResultExtensions
    {
        public static Result<T> FromHttpStatusCode<T>(System.Net.HttpStatusCode statusCode) where T : class
        {
            return new Result<T> {
                Code = HttpStatusCodeToStatusCode(statusCode),
            };
        }

        public static ResultCode GetResultCode(this System.Net.Http.HttpResponseMessage response)
        {
            return HttpStatusCodeToStatusCode(response.StatusCode);
        }

        public static ResultCode HttpStatusCodeToStatusCode(System.Net.HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case System.Net.HttpStatusCode.Unauthorized: return ResultCode.unauthorized;
                case System.Net.HttpStatusCode.OK: return ResultCode.ok;
                default: return ResultCode.failed;
            }
        }
    }
}
