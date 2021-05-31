using Imago.IO.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Imago.IO
{
    public enum ApiErrorCodes
    {
        None,
        NoWorkspaceFound,
        NoDatasetFound,
        NoCollectionFound,
        NoImageryTypeFound,
        NoImageTypeFound,
        NoFeatureDefinitionFound,
        NoFeatureTypeFound,
        NoLabelDefinitionFound,
        NoLabelTypeFound,
        NoImageryFound,
        NoImageFound,
        NoImageryDepthsDefined,
        NoImageryNameDefined,
        IllegalFeatureFormat,
        ImageTransferFailed,
        CreateCollectionFailed,
        CreateImageryFailed,
        CreateImageFailed,
        NoReadPermission,
        NoWritePermission,
        UnknownError,
        OperationCancelled
    }

    internal class ApiErrorCodeMapping
    {
        public static List<ApiErrorCodeMapping> Mappings = new List<ApiErrorCodeMapping>
        {
            new ApiErrorCodeMapping { Message = "NO_WORKSPACE_FOUND", ErrorCode = ApiErrorCodes.NoWorkspaceFound },
            new ApiErrorCodeMapping { Message = "NO_DATASET_FOUND", ErrorCode = ApiErrorCodes.NoDatasetFound },
            new ApiErrorCodeMapping { Message = "NO_COLLECTION_FOUND", ErrorCode = ApiErrorCodes.NoCollectionFound },
            new ApiErrorCodeMapping { Message = "NO_IMAGERYTYPE_FOUND", ErrorCode = ApiErrorCodes.NoImageryTypeFound },
            new ApiErrorCodeMapping { Message = "NO_IMAGETYPE_FOUND", ErrorCode = ApiErrorCodes.NoImageTypeFound },
            new ApiErrorCodeMapping { Message = "NO_FEATUREDEFINITION_FOUND", ErrorCode = ApiErrorCodes.NoFeatureDefinitionFound },
            new ApiErrorCodeMapping { Message = "NO_FEATURETYPE_FOUND", ErrorCode = ApiErrorCodes.NoFeatureTypeFound },
            new ApiErrorCodeMapping { Message = "NO_LABELDEFINITION_FOUND", ErrorCode = ApiErrorCodes.NoLabelDefinitionFound },
            new ApiErrorCodeMapping { Message = "NO_LABELTYPE_FOUND", ErrorCode = ApiErrorCodes.NoLabelTypeFound},
            new ApiErrorCodeMapping { Message = "NO_IMAGE_FOUND", ErrorCode = ApiErrorCodes.NoImageFound },
            new ApiErrorCodeMapping { Message = "NO_IMAGERY_FOUND", ErrorCode = ApiErrorCodes.NoImageryFound},
            new ApiErrorCodeMapping { Message = "NO_READ_PERMISSION", ErrorCode = ApiErrorCodes.NoReadPermission },
            new ApiErrorCodeMapping { Message = "NO_WRITE_PERMISSION", ErrorCode = ApiErrorCodes.NoWritePermission },
            new ApiErrorCodeMapping { Message = "CREATE_COLLECTION_FAILED", ErrorCode = ApiErrorCodes.CreateCollectionFailed },
            new ApiErrorCodeMapping { Message = "CREATE_IMAGERY_FAILED", ErrorCode = ApiErrorCodes.CreateImageryFailed },
            new ApiErrorCodeMapping { Message = "NO_IMAGERY_DEPTHS", ErrorCode = ApiErrorCodes.NoImageryDepthsDefined },
            new ApiErrorCodeMapping { Message = "NO_IMAGERY_NAME", ErrorCode = ApiErrorCodes.NoImageryNameDefined },
            new ApiErrorCodeMapping { Message = "ILLEGAL_FEATURE_FORMAT", ErrorCode = ApiErrorCodes.IllegalFeatureFormat },
            new ApiErrorCodeMapping { Message = "CREATE_IMAGE_FAILED", ErrorCode = ApiErrorCodes.CreateImageFailed },
            new ApiErrorCodeMapping { Message = "IMAGE_TRANSFER_FAILED", ErrorCode = ApiErrorCodes.ImageTransferFailed }
        };

        public static ApiErrorCodes Parse(string message)
        {
            var mapping = Mappings.FirstOrDefault(x => String.Equals(x.Message, message, StringComparison.OrdinalIgnoreCase));
            return mapping?.ErrorCode ?? ApiErrorCodes.UnknownError;
        }

        public string Message { get; set; }
        public ApiErrorCodes ErrorCode { get; set; }
    }

    internal static class ResultExtensions
    {
        public static Result<T> FromHttpStatusCode<T>(System.Net.HttpStatusCode statusCode) where T : class
        {
            return new Result<T>
            {
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

        /// <summary>
        /// This wraps the ProcessResult result in a task for use with the async variant.
        /// <see cref="ResultExtensions.ConvertToResult{T}(HttpResponseMessage, Func{HttpResponseMessage, string, Task{T}})"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <param name="processResponse"></param>
        /// <returns></returns>
        public static async Task<Result<T>> ConvertToResult<T>(this HttpResponseMessage response, Func<HttpResponseMessage, string, T> processResponse, JavaScriptSerializer serializer) where T : class
        {
            return await response.ConvertToResult((httpResponse, body) =>
            {
                var result = processResponse(response, body);
                return Task.FromResult(result);
            },
            serializer
            );
        }

        /// <summary>
        /// Process a http response and convert it to an api result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <param name="processResponse"></param>
        /// <returns></returns>
        public static async Task<Result<T>> ConvertToResult<T>(this HttpResponseMessage response, Func<HttpResponseMessage, string, Task<T>> processResponse, JavaScriptSerializer serializer) where T : class
        {
            string body = await response.Content.ReadAsStringAsync();
            ResultCode code = response.GetResultCode();

            var result = new Result<T>();

            switch (code)
            {
                case ResultCode.ok:
                    result.Value = await processResponse(response, body);
                    if (result.Value == null)
                    {
                        result.Code = ResultCode.failed;
                        result.Error = ApiErrorCodes.UnknownError;
                    }
                    break;
                case ResultCode.unauthorized:
                    result.Code = ResultCode.unauthorized;
                    break;
                default:
                    result.Code = ResultCode.failed;
                    result.Error = ApiErrorCodeMapping.Parse(GetErrorMessage(serializer, body));
                    break;
            }

            return result;
        }

        private static string GetErrorMessage(JavaScriptSerializer serializer, string body)
        {
            try
            {
                var errorResponse = serializer.Deserialize<ApiErrorResponse>(body);
                return errorResponse.Errors?.Select(x => x.Message).FirstOrDefault() ?? string.Empty;
            }
            catch
            {
                return body;
            }          
        }
    }
}
