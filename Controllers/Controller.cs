using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace webapi.Controllers
{
    [ApiController]
    public class Controller : ControllerBase
    {

        private readonly ILogger<Controller> _logger;

        public Controller(ILogger<Controller> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("/download")]
        public async Task FetchFile()
        {
            Response.ContentType = "video/mp4";
            string path = @"C:\stream.mp4";
            using FileStream fs = System.IO.File.OpenRead(path);
            Response.StatusCode = 200;
            await fs.CopyToAsync(Response.Body);
        }

        [HttpGet]
        [Route("/downloadx")]
        public async Task<FileStreamResult> FetchFilex()
        {
            string path = @"C:\stream.mp4";
            FileStream fs = System.IO.File.OpenRead(path);
            return new FileStreamResult(fs, "video/mp4");
        }

        [HttpGet]
        [Route("/downloada")]
        public FileResult getFileById(int fileId)
        {
            string path = @"C:\stream.mp4";
            return PhysicalFile(path, "video/mp4", enableRangeProcessing: true);
        }
    }
}
