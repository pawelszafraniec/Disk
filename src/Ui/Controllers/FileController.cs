using Disk.Common.DTO;
using Disk.Ui.Models;
using Disk.Ui.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disk.Ui.Controllers
{
    [Authorize]
    [Route("/file")]
    public class FileController : Controller
    {
        private static readonly Dictionary<string, FileType> fileTypes = new Dictionary<string, FileType>
        {
            { "", FileType.Unknown },
            { ".txt", FileType.Text },
            { ".png", FileType.Image },
            { ".jpg", FileType.Image },
            { ".jpeg", FileType.Image },
            { ".gif", FileType.Image },
            { ".mp4", FileType.Video },
        };

        private readonly ILogger logger;
        private readonly FileService fileService;

        public FileController(ILogger<FileController> logger, FileService fileService)
        {
            this.logger = logger;
            this.fileService = fileService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Index(Guid id)
        {
            FileResponse response = await this.fileService.GetFileInfo(id);

            string extension = System.IO.Path.GetExtension(response.Name);

            fileTypes.TryGetValue(extension, out FileType fileType);
            return this.View(new FileViewModel(response, fileType));
        }

        [HttpGet("/{directoryId}/upload-file")]
        public IActionResult Upload(Guid directoryId)
        {
            return this.View(new FileUploadViewModel(directoryId));
        }

        [HttpPost("{directoryId}")]
        public async Task<IActionResult> Upload(Guid directoryId, IFormFile file)
        {
            if (file == null)
            {
                return this.View(new FileUploadViewModel(directoryId));
            }

            await this.fileService.UploadFile(directoryId, file);
            return this.Redirect($"/{directoryId}");
        }
    }
}
