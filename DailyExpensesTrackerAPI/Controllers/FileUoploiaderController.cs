using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
//using System.Web.Http;
using DailyExpensesTrackerAPI.ServiceModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DailyExpensesTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUoploiaderController : ControllerBase
    {
        // GET: api/<FileUoploiaderController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<FileUoploiaderController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FileUoploiaderController>
        [HttpPost, DisableRequestSizeLimit]
        public ActionResult<List<FileUploader>> Upload()
        {
            List<FileUploader> filesList = new List<FileUploader>();
            try
            {
                var files = Request.Form.Files;
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (files.Any(f => f.Length == 0))
                {
                    return BadRequest();
                }

                foreach (var file in files)
                {
                    var fileType = file.FileName.Split(".")[1];
                    var fileName = Convert.ToString(DateTimeOffset.Now.ToUnixTimeSeconds()) + "." + fileType;
                    //var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName); //you can add this path to a list and then return all dbPaths to the client if require

                    filesList.Add(
                        new FileUploader() { ActualFileName = file.FileName, GeneratedFileName = fileName }
                        ); ;
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                return filesList;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FileUoploiaderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

       // [HttpGet("download")]//http get as it return file 

        //[FromQuery]
        //public HttpResponseMessage GetTestFile([System.Web.Http.FromUri] string FileName)
        //{
        //    //below code locate physical file on server 
        //    //	string fileName = "NCH-974-073-022-459-800132294371051615057_ajay_FebrauryWork15th.xlsx";

        //    FileName = "1638716546.png";
        //    var folderName = Path.Combine("Resources", "Images");
        //    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        //    var localFilePath = Path.Combine(pathToSave, FileName);
        //    HttpResponseMessage response = null;


        //    //if file present than read file 
        //    var fStream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read);
        //    //compose response and include file as content in it
        //    response = new HttpResponseMessage
        //    {
        //        //StatusCode = return OK(),
        //        Content = new StreamContent(fStream)
        //    };
        //    //set content header of reponse as file attached in reponse
        //    response.Content.Headers.ContentDisposition =
        //    new ContentDispositionHeaderValue("attachment")
        //    {
        //        FileName = Path.GetFileName(fStream.Name)
        //    };
        //    //set the content header content type as application/octet-stream as it      
        //    //returning file as reponse 
        //    response.Content.Headers.ContentType = new
        //                  MediaTypeHeaderValue("application/octet-stream");

        //    return response;
        //}

        [HttpGet("download")]
        public async Task<IActionResult> Download([FromQuery] string FileName)
        {
        //E:\DailyExpensesTracker\DailyExpensesTrackerAPI\DailyExpensesTrackerAPI\Resources\Images\1638716546.png
            var currentDirectory = Directory.GetCurrentDirectory();
            FileName = "Resources\\Images\\" + FileName;
            //string getCurrentFileName = "Resources" + ""
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), FileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();
            var memory = new MemoryStream();
            await using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filePath), filePath);
        }

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;

            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }

    }
}
