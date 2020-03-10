using Microsoft.AspNetCore.Http;

namespace TransactionManager.Components.Models.Requests
{
    public class UploadFileRequest
    {
        public IFormFile File { get; set; }
    }
}
