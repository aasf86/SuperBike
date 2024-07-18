using Microsoft.AspNetCore.Mvc;
using SuperBike.Api.FileStorage.ServiceHandler;
using SuperBike.Business.Dtos.Renter;

namespace SuperBike.Api.FileStorage.Controllers
{
    public static class Routes
    {
        public const string BaseUrlApi = "api/file-storage";
        public static class Relative
        {
            public const string Download = "download/{key}";
            public const string Upload = "upload";
        }        
    }

    [ApiController]
    [Route(Routes.BaseUrlApi)]
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

        [HttpGet(Routes.Relative.Download)]
        public async Task<ActionResult> Download(string key)
        {
            try
            {                
                var fileDiskDb = await _fileManager.GetFile(_localStorage, key);

                if (fileDiskDb is not null)
                {
                    return File(System.IO.File.ReadAllBytes(fileDiskDb.LocalPath), fileDiskDb.ContentType, fileDiskDb.FileName);
                }
                
                return NotFound("Item não localizado.");
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.Message);
                return NotFound("Item não localizadoa.");
            }
        }

        [HttpPost(Routes.Relative.Upload)]
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
                    fileDiskDb.Inserted,
                    UrlPath = $"{Routes.BaseUrlApi}/{Routes.Relative.Download.Replace("{key}", fileDiskDb.Key)}",
                    Host = $"{Request.Scheme}//{Request.Host.Value}"  
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