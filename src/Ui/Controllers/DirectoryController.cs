using Disk.Common.DTO;
using Disk.Ui.Models;
using Disk.Ui.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Disk.Ui.Controllers
{
    [Authorize]
    [Route("/")]
    public class DirectoryController : Controller
    {
        private readonly ILogger logger;
        private readonly DirectoryService directoryService;

        public DirectoryController(ILogger<DirectoryController> logger, DirectoryService directoryService)
        {
            this.logger = logger;
            this.directoryService = directoryService;
        }

        [HttpGet("{directoryId?}")]
        public async Task<IActionResult> Index(Guid? directoryId = null,
                                               [FromQuery] string? query = null,
                                               [FromQuery] string? includeSubdirectories = null)
        {
            bool include = includeSubdirectories == "on";

            DirectoryResponse? response = await this.directoryService.GetDirectoryAsync(directoryId, query, include);

            var model = new DirectoryViewModel(
                includeSubdirectories: include,
                query: query,
                directories: response.Directories,
                files: response.Files,
                invalidQuery: response.InvalidQuery,
                directoryId: response.Id
                );

            return this.View(model);
        }

        [HttpGet("{parentId}/create-directory")]
        public async Task<IActionResult> Create(Guid? parentId)
        {
            return this.View(new DirectoryCreateViewModel());
        }

        [HttpPost("{parentId}/create-directory")]
        public async Task<IActionResult> Create(Guid? parentId, [FromForm] DirectoryCreateViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            DirectoryResponse d = await this.directoryService.GetDirectoryAsync(parentId, null, null);

            if (d.Directories.Any(directory => directory.Name == model.Name))
            {
                this.ModelState.AddModelError("Name", "Directory with the given name exists");
                return this.View(model);
            }

            var newDirectory = await this.directoryService.CreateDirectoryAsync(parentId, model.Name!);
            return this.Redirect($"/{parentId}");
        }
    }
}
