using DbLayer.Conext;
using Microsoft.AspNetCore.Mvc;

namespace DataServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly DbLayerContext _context;
        public HomeController(DbLayerContext context) => _context = context;

        [HttpGet]
        public FileContentResult Get([FromQuery] string fileName)
        {
             var (name, extension, data) = MiddleWareLogic.DataConvertation.GetDataFileFromDb(_context, fileName);
            return File(data, $"application/{extension}", $"{name}.{extension}");
        }

        // POST api/<DataProcessor>
        [HttpPost]
        public void Post([FromQuery] string value)
        {
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
