using System.Threading.Tasks;
using FileManager.Components.Models;
using FileManager.Components.Models.Requests;
using FileManager.Components.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FileManager.Controllers
{
    [ApiController]
    [Route("api/file")]
    public class FileManagerController : ControllerBase
    {        
        private readonly ILogger<FileManagerController> _logger;
        private readonly IFileManagerService _fileManagerService;

        public FileManagerController(ILogger<FileManagerController> logger, IFileManagerService fileManagerService)
        {
            _logger = logger;
            _fileManagerService = fileManagerService;
        }               

        [HttpPost]
        public async Task<IActionResult> SaveFile([FromForm] UploadFileRequest request)
        {
            await _fileManagerService.ImportFile(request);
            return Ok();
        }
    }
}
