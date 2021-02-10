using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.IO.Interfaces
{
    public interface IImage
    {
        Guid ImageTypeId { get; set; }
        string url { get; set; }
        DateTime? uploadedOn { get; set; }
        string uploadedBy { get; set; }
        int? width { get; set; }
        int? height { get; set; }
        Int64? fileSize { get; set; }
        string MimeType { get; set; }
        string Checksum { get; set; }
    }
}
