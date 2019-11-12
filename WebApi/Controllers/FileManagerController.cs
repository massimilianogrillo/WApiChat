using Lavoro.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using WebApi.Repositories;
using WebApi.Services;

namespace WebApi.Controllers
{

    [Route("api/[controller]-[action]")]
    [ApiController]
    public class FileManagerController : BaseController
    {
        private IFileProviderService fileService;
        public FileManagerController(LaboroContext context, IFileProviderService service) : base(context)
        {
            fileService = service;
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<object>> AllFiles(int id)
        {
            FileRepository repository = new FileRepository(context);
            string owners = Request.Query["owners"].FirstOrDefault();
            List<object> files;

            if (owners == "all")
                files = repository.GetAllFiles(id);
            else if (owners == "my")
                files = repository.GetMyFiles(id);
            else
                files = repository.GetFilesSharedWith(id);

            return files;
        }

        [HttpGet("{id}")]
        public FileResult Download(int id)
        {
            FileRepository repository = new FileRepository(context);
            string path = repository.GetFilePath(id);
          
            //var result = new HttpResponseMessage(HttpStatusCode.OK) {
            //    Content = new StreamContent(fileService.GetStream(path))
            //};
            //result.Content.Headers.ContentDisposition =
            //    new ContentDispositionHeaderValue("attachment") {
            //        FileName = path
            //    };
            //result.Content.Headers.ContentType =
            //    new MediaTypeHeaderValue("application/octet-stream");

            return File(fileService.GetBytes(path), "application/octet-stream", Path.GetFileName(path));
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            FileRepository repository = new FileRepository(context);
            //delete from database
            string path = repository.DeleteFile(id);

            // delete from file system
            fileService.DeleteFile(path);
        }

        [HttpPost("{ownerId}")]
        public ActionResult<bool> Add(int ownerId)
        {
            FileRepository repository = new FileRepository(context);
            var file = Request.Form.Files.FirstOrDefault();

            //write to file system
            string path = fileService.WriteToFileSystem(ownerId, file);

            // add file to the database
            string size = ( (int)( file.Length / 1024f ) ).ToString() + "kb";
            string name = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);
            var response = repository.AddFile(ownerId, size, name, file.ContentType, path, extension);
            return response;
        }




    }
}
