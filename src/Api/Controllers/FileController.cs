using Disk.Api.Data;
using Disk.Common.DTO;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Disk.Api.Controllers
{
    [Route("v1/api/rest/file")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly ApplicationDbContext context;
        private readonly IContentTypeProvider contentTypeProvider;

        public FileController(IConfiguration config, ApplicationDbContext context, IContentTypeProvider contentTypeProvider)
        {
            this.config = config;
            this.context = context;
            this.contentTypeProvider = contentTypeProvider;
        }


        [HttpGet("{id}")]
        public IActionResult GetFile(Guid id)
        {
            Disk.Api.Data.File? file = this.context.Files.SingleOrDefault(s => s.Id == id);

            if (file == null)
            {
                return this.NotFound();
            }

            string ext = Path.GetExtension(file.Name);
            string name = Path.GetFileNameWithoutExtension(file.Name);

            try
            {
                FileStream fs = System.IO.File.OpenRead(Path.Combine(this.GetStorageDirectory(), file.Id.ToString() + ext));
                string mimeType = this.GetContentType(ext);
                var result = new FileStreamResult(fs, mimeType);
                this.Response.Headers.Add("Content-Disposition", "Inline; filename=" + HttpUtility.UrlEncode(file.Name));
                return result;
            }
            catch (FileNotFoundException)
            {
                return this.NotFound("No file in storage directory");
            }
        }

        [HttpGet("{id}/info")]
        public IActionResult GetFileInfo(Guid id)
        {
            Disk.Api.Data.File? file = this.context.Files.SingleOrDefault(s => s.Id == id);

            return file == null
                ? this.NotFound() as IActionResult
                : this.Ok(new FileResponse(file.Id, file.Name, file.Size));
        }

        [HttpPost("{directoryId}")]
        public async Task<IActionResult> Upload(Guid directoryId, IFormFile file)
        {
            if (await this.context.Directories.SingleOrDefaultAsync(directory => directory.Id == directoryId) == null)
            {
                return this.NotFound();
            }

            string fileName = file.FileName;

            for (int i = 2; this.context.Files.Any(f => f.DirectoryId == directoryId && f.Name == fileName); i++)
            {
                fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}({i}){Path.GetExtension(file.FileName)}";
            }

            var fileEntity = new Disk.Api.Data.File()
            {
                Id = Guid.NewGuid(),
                DirectoryId = directoryId,
                Name = fileName,
                Size = (int)file.Length,
            };

            this.context.Add(fileEntity);
            await this.context.SaveChangesAsync();

            FileStream fileStream = System.IO.File.Create(Path.Combine(
                this.GetStorageDirectory(),
                $"{fileEntity.Id}{Path.GetExtension(fileEntity.Name)}"
                ));
            await file.CopyToAsync(fileStream);
            fileStream.Close();

            return this.Ok();
        }

        private string GetContentType(string extension)
        {
            return this.contentTypeProvider.TryGetContentType(extension, out string result)
                ? result
                : "application/octet-stream";
        }


        private string GetStorageDirectory()
        {
            return this.config["App:StorageDirectory"];
        }
    }
}