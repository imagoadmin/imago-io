using Imago.IO.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Imago.IO.Client;

namespace Imago.IO
{
    public interface IClient
    {
        int MaxRetryAttempts { get; set; }
        Task<bool> SignIn(Credentials credentials, TimeSpan? timeout = null);

        Task<Result<List<Imagery>>> SearchForImagery(ImageryQueryParameters parameters, CancellationToken ct, TimeSpan? timeout = null);
        Task<Result<string>> DownloadImageToFile(ImageQueryParameters parameters, string fileName, CancellationToken ct, TimeSpan? timeout = null);
        Task<Result<List<Collection>>> SearchForCollection(CollectionQueryParameters parameters, CancellationToken ct, TimeSpan? timeout = null);
        Task<Result<UserContext>> GetUserContext(TimeSpan? timeout = null);
    }
}
