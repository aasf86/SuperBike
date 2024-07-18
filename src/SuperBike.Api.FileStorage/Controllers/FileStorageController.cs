using Microsoft.AspNetCore.Mvc;
using SuperBike.Api.FileStorage.Dtos;
using SuperBike.Api.FileStorage.ServiceHandler;

namespace SuperBike.Api.FileStorage.Controllers
{
    [ApiController]
    [Route("api/file-storage")]
    public class FileStorageController : ControllerBase
    {
        private readonly ILogger<FileStorageController> _logger;
        private readonly string _localStorage;
        private readonly FileManager _fileManager;

        public FileStorageController(
            ILogger<FileStorageController> logger, 
            [FromServices] IConfigurationManager config,
            [FromServices] FileManager fileManager)
        {
            _logger = logger;
            _localStorage = config.GetSection("DiskFiles").Value;
            _fileManager = fileManager;
        }

        [HttpGet("download/{key}")]
        public async Task<ActionResult> Download(string key)
        {
            try
            {                
                var fileDiskDb = await _fileManager.GetFile(_localStorage, key);

                if (fileDiskDb is not null)
                {
                    return File(System.IO.File.ReadAllBytes(fileDiskDb.LocalPath), fileDiskDb.ContentType, fileDiskDb.FileName);
                }
                
                return NotFound("Item não localizadoa.");
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.Message);
                return NotFound("Item não localizadoa.");
            }
        }

        [HttpPost("upload")]
        public async Task<ActionResult> Upload([FromForm] FileUpload file)
        {
            try
            {
                var fileDiskDb = await _fileManager.GenerateLocal(_localStorage, file.File);
                return Ok(new { 
                    fileDiskDb.Key, 
                    fileDiskDb.FileName, 
                    fileDiskDb.ContentType, 
                    fileDiskDb.Length, 
                    fileDiskDb.Inserted 
                });
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.Message);
                return BadRequest("Houve um erro ao processar arquivo.");
            }
        }
    }
}

namespace SuperBike.Api.FileStorage.Dtos
{
    public class FileUpload
    {
        public IFormFile File { get; set; }
    }
}