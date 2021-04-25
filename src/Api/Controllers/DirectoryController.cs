using Disk.Api.Data;

using Disk.Api.Data;
using Disk.Common.DTO;
using Disk.Search;
using Disk.Search.Query;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Disk.Api.Controllers
{
    [Route("v1/api/rest/directory")]
    [ApiController]
    [Authorize]
    public class DirectoryController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IExpressionParser expressionParser;
        private readonly ApplicationDbContext context;

        public DirectoryController(IConfiguration config, ApplicationDbContext context,
                                   IExpressionParser expressionParser)
        {
            this.configuration = config;
            this.context = context;
            this.expressionParser = expressionParser;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string? query = null,
                                 [FromQuery] bool? includeSubdirectories = false)
        {
            return this.Get(null, query, includeSubdirectories);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid? id,
                                      [FromQuery] string? query = null,
                                      [FromQuery] bool? includeSubdirectories = false)
        {
            Guid realId = id ?? this.GetRootDirectoryId();

            List<DirectoryBaseResponse>? directories = includeSubdirectories == true
                ? new List<DirectoryBaseResponse>()
                : this.context.Directories
                             .Where(directory => directory.ParentID == realId)
                             .Select(directory => new DirectoryBaseResponse(directory.Id, directory.Name))
                             .ToList();

            IQueryable<File> files;

            if (includeSubdirectories == true)
            {
                IEnumerable<Guid> subdirectories = this.GetSubdirectories(realId).Append(realId);

                files = this.context.Files
                 .Where(files => subdirectories.Contains(files.DirectoryId));
            }
            else
            {
                files = this.context.Files
                .Where(files => files.DirectoryId == realId);
            }

            bool invalidQuery = false;

            if (!string.IsNullOrWhiteSpace(query))
            {
                try
                {
                    var queryString = Encoding.UTF8.GetString(Convert.FromBase64String(query));
                    Expression<Func<File, bool>>? expression = this.expressionParser.ParseExpression<File>(queryString);
                    files = files.Where(expression);
                }
                catch (FormatException)
                {
                    return this.BadRequest("Query have wrong encoding");
                }
                catch (InvalidQueryException)
                {
                    invalidQuery = true;
                }
            }

            return this.Ok(new DirectoryResponse()
            {
                Directories = directories,
                Files = files
                    .Select(file => new FileResponse(file.Id, file.Name, file.Size))
                    .ToList(),
                InvalidQuery = invalidQuery,
                Id = realId,
            });
        }

        [HttpPost("{parentId}")]
        public async Task<IActionResult> CreateDirectory(Guid parentId, [FromBody] DirectoryRequest request)
        {
            Directory? parent = this.context.Directories.SingleOrDefault(directory => directory.Id == parentId);

            if (parent == null)
            {
                return this.NotFound();
            }

            if (this.context.Directories.Any(d => d.ParentID == parentId && d.Name == request.Name))
            {
                return this.BadRequest();
            }

            var newDirectory = new Directory()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                ParentID = parent.Id,
            };

            this.context.Add(newDirectory);

            await this.context.SaveChangesAsync();

            return this.CreatedAtAction(nameof(this.Get),
                                        new { id = newDirectory.Id },
                                        new DirectoryBaseResponse(
                                            newDirectory.Id,
                                            newDirectory.Name));
        }

        private Guid GetRootDirectoryId()
        {
            return this.context.Directories.Single(check => check.IsRoot == true).Id;
        }


        private List<Guid> GetSubdirectories(Guid id)
        {

            var listID = new List<Guid>();
            listID = this.context.Directories
                .Where(directory => directory.ParentID == id)
                .Select(directory => directory.Id)
                .ToList();

            for (int i = 0; i < listID.Count; i++)
            {
                var sub = new List<Guid>();
                sub = this.context.Directories
                    .Where(directory => directory.ParentID == listID[i])
                    .Select(directory => directory.Id)
                    .ToList();
                listID.AddRange(sub);
            }
            return listID;
        }
    }
}