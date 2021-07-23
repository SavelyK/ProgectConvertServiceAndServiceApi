using Microsoft.AspNetCore.Http;

namespace RepositoryWebApi.Models
{
    public class FileUpload
    {
        public IFormFile files { get; set; }
    }
}
