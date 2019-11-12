using Microsoft.AspNetCore.Http;
using System.IO;

namespace WebApi.Services
{
    public interface IFileProviderService
    {
        string WriteToFileSystem(int owner, IFormFile file);
        void DeleteFile(string path);
        Stream GetStream(string path);
        byte[] GetBytes(string path);
    }
}
