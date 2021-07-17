using Microsoft.AspNetCore.Http;

namespace LibraryModels
{
    public class FileUpload
    {
        public IFormFile files { get; set; }
    }
}
