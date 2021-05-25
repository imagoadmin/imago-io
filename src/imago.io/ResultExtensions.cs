using System.Collections.Generic;

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

        public string Message { get; set; }
        public ApiErrorCodes ErrorCode { get; set; }
    }

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
