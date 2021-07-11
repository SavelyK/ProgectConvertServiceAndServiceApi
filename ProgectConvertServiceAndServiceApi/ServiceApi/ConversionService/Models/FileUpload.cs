using Microsoft.AspNetCore.Http;


namespace ConversionService.Models
{
    public class FileUpload
    {
        public IFormFile files { get; set; }
    }
}
