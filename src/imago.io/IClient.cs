using Imago.IO.Classes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Imago.IO.Client;

namespace Imago.IO
{
    // Interface that allows testing to take place through mocking
    public interface IClient
    {
        int MaxRetryAttempts { get; set; }
        HttpClient Direct { get; }
        Task<bool> SignIn(Credentials credentials, TimeSpan? timeout = null);
        string APIUrl { get; }

        Task<Result<List<Imagery>>> SearchForImagery(ImageryQueryParameters parameters, CancellationToken ct, TimeSpan? timeout = null);
        Task<Result<object>> SetImageAttributes(Guid id, string groupName, object attributeData, CancellationToken ct, TimeSpan? timeout = null);
        Task<Result<Imagery>> UpdateImagery(ImageryUpdateParameters parameters, CancellationToken ct, TimeSpan? timeout = null);
        Task<Result<ImageUpdateResult>> UploadImage(ImageUpdateParameters parameters, CancellationToken ct, TimeSpan? timeout = null);
        Task<Result<object>> UpdateBulkImageryAttributes(BulkAttributeUpdateParameters parameters, CancellationToken ct, TimeSpan? timeout = null);
        Task<Result<string>> DownloadImageToFile(ImageQueryParameters parameters, string fileName, CancellationToken ct, TimeSpan? timeout = null);
        Task<Result<List<Collection>>> SearchForCollection(CollectionQueryParameters parameters, CancellationToken ct, TimeSpan? timeout = null);
        Task<Result<UserContext>> GetUserContext(TimeSpan? timeout = null);
        Task<Result<Collection>> AddCollection(CollectionUpdateParameters parameters, CancellationToken ct, TimeSpan? timeout = null);
        Task<Result<List<Collection>>> AddCollectionBulk(List<CollectionUpdateParameters> parameters, CancellationToken ct, TimeSpan? timeout = null);
        Task<Result<string>> GetWorkspaceSASUrl(Guid id, Guid? imageId = null, string mimeType = null, TimeSpan? timeout = null);
    }
}
