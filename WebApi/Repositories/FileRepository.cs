using Lavoro.Data;
using Lavoro.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using IO = System.IO;

namespace WebApi.Repositories
{
    public class FileRepository : BaseRepository
    {
        public FileRepository(LaboroContext context) : base(context)
        { }

        /// <summary>
        /// Get all files in the file system
        /// </summary>
        /// <returns></returns>
        public List<object> GetAllFiles()
        {
            var files = ( from f in context.Files
                          select new {
                              id = f.Id,
                              name = f.Name,
                              type = f.Type,
                              owner = File.Owner(f.User),
                              size = f.Size,
                              modified = f.Modified.ToLongDateString(),
                              created = f.Created.ToLongDateString(),
                              opened = f.Opened.ToLongDateString(),
                              extention = f.Extention,
                              location = f.Location,
                              offline = f.Offline,
                              preview = f.Preview
                          } ).ToList();


            return files.ToList<object>();
        }

        /// <summary>
        /// Gets all fies a user has access to.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<object> GetAllFiles(int userId)
        {
            var files = ( from f in context.Files
                          where !f.UserId.HasValue || f.UserId == userId
                          select new {
                              id = f.Id,
                              name = f.Name,
                              type = f.Type,
                              owner = File.Owner(f.User),
                              size = f.Size,
                              modified = f.Modified.ToLongDateString(),
                              created = f.Created.ToLongDateString(),
                              opened = f.Opened.ToLongDateString(),
                              extention = f.Extention,
                              location = f.Location,
                              offline = f.Offline,
                              preview = f.Preview
                          } ).ToList();
            return files.ToList<object>();
        }

        /// <summary>
        /// Gets all files that the user owns.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<object> GetMyFiles(int userId)
        {
            var files = ( from f in context.Files
                          where f.UserId.HasValue && f.UserId.Value == userId
                          select new {
                              id = f.Id,
                              name = f.Name,
                              type = f.Type,
                              owner = File.Owner(f.User),
                              size = f.Size,
                              modified = f.Modified.ToLongDateString(),
                              created = f.Created.ToLongDateString(),
                              opened = f.Opened.ToLongDateString(),
                              extention = f.Extention,
                              location = f.Location,
                              offline = f.Offline,
                              preview = f.Preview
                          } ).ToList();


            return files.ToList<object>();
        }

        /// <summary>
        /// Gets files shared with this user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<object> GetFilesSharedWith(int userId)
        {
            var files = ( from f in context.Files
                          where !f.UserId.HasValue
                          select new {
                              id = f.Id,
                              name = f.Name,
                              type = f.Type,
                              owner = File.Owner(f.User),
                              size = f.Size,
                              modified = f.Modified.ToLongDateString(),
                              created = f.Created.ToLongDateString(),
                              opened = f.Opened.ToLongDateString(),
                              extention = f.Extention,
                              location = f.Location,
                              offline = f.Offline,
                              preview = f.Preview
                          } ).ToList();


            return files.ToList<object>();
        }

        /// <summary>
        /// Gets a path for the file that can be used to access the file using fileproviderservice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetFilePath(int id)
        {
            return context.Files.Find(id).Location;
        }

        /// <summary>
        /// Adds a new file to the database
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="size"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="path"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public bool AddFile(int owner, string size, string name, string type, string path, string extension)
        {
            
            //save file information to the database
            File dbFile = new File {
                UserId = owner,
                Size = size,
                Name = name,
                Type = type,
                Location = path,
                Extention= extension
            };
            context.Files.Add(dbFile);
            context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Deletes a file from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteFile(int id)
        {
            File file = context.Files.Find(id);
            string path = file.Location;
            context.Remove(file);
            context.SaveChanges();
            return path;

        }
    }
}
