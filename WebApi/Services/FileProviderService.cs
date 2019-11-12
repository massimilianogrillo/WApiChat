using Microsoft.AspNetCore.Http;
using System.IO;

namespace WebApi.Services
{
    /// <summary>
    /// Interacts with the File System
    /// </summary>
    public class FileProviderService : IFileProviderService
    {
        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        /// <summary>
        /// Gets file as a stream. Use GetBytes instead.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Stream GetStream(string path)
        {
           return new FileStream(path, FileMode.Open, FileAccess.Read);
        }

        /// <summary>
        /// Gets all the bytes in the file. Use this instead of GetStream
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public byte[] GetBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        /// <summary>
        /// Writes an uploaded file to the file system
        /// </summary>
        /// <param name="owner">Owner of the file</param>
        /// <param name="file">FIle uploaded from a form</param>
        /// <returns> Path of the file that can be used to read the file </returns>
        public string WriteToFileSystem(int owner, IFormFile file)
        {
            string path = FilePath(owner, file.FileName);
            string directory = Path.GetDirectoryName(path);

            if (File.Exists(path))
            {
                //generate new name
                int i = 1;
                string name = Path.GetFileNameWithoutExtension(path);
                string extension = Path.GetExtension(path);
                string newPath;
                do
                {
                    newPath = Path.Combine(directory, name + "_(" + i + ")" + extension);
                    i++;
                }
                while (File.Exists(newPath));
                path = newPath;
            }
            //create directory if it doesnt exist
            var info = Directory.CreateDirectory(directory);

            //write the file to file system
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return path;
        }

        /// <summary>
        /// Gets the path that the file should have
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        private string FilePath(int ownerId, string filename)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ownerId.ToString(), filename);
        }
    }
}
