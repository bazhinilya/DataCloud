using DbLayer.Conext;
using Microsoft.AspNetCore.Mvc;

namespace DataServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly DbLayerContext _context;
        public HomeController(DbLayerContext context) => _context = context;

        [HttpGet]
        [Route("downloadFile")]
        public IActionResult DownloadFile([FromQuery] string fileName)
        {
             var (name, extension, data) = MiddleWareLogic.DataConvertation.GetDataFileFromDb(_context, fileName);
            return File(data, $"application/{extension[1..]}", $"{name}{extension}");
        }

        [HttpPost]
        [Route("uploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile uploadedFile)
        {
            return Ok(await MiddleWareLogic.DataConvertation.SetDataFileFromDb(_context, uploadedFile));
        }

        //// PUT api/<DataProcessor>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<DataProcessor>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}