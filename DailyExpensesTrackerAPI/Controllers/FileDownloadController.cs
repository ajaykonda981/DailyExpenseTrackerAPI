using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DailyExpensesTrackerAPI.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DailyExpensesTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileDownloadController : ControllerBase
    {
        // GET: api/<FileDownloadController>
        [HttpPost]
        public async Task<ActionResult> Get([FromBody] Fileuploader fileuploader)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), @"Resources\Images\" + fileuploader.GeneratedFileName );
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return File(memory, GetFileTypes()[ext], fileuploader.ActualFileName);
        }

        private Dictionary<string, string> GetFileTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt" , "text/plain" },
                {".pdf", "application/pdf" },
                { ".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                {".xls", "application/vnd.ms-excel" },
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                {".png", "image/png" },
                {".jpg", "image/jpeg" },
                {".jped", "image/jped" },
                {".gif", "image/gif" },
                {".csv", "image/csv" }
            };
        }
    }
}
