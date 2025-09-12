using AppAPI.Application.Interfaces;
using AppAPI.Domain.Dto_s;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AppAPI.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class SbifController : ControllerBase
    {
        private readonly IGenerateSbifService _sbifService;
        private readonly IMemoryCache _memoryCache;

        public SbifController(IGenerateSbifService sbifService, IMemoryCache memoryCache)
        {
            _sbifService = sbifService;
            _memoryCache = memoryCache;
        }
        [HttpPost("set-generic-data")]
        public IActionResult SetGenericData([FromBody] GenericDataDto genericDataDto)
        {
            if (genericDataDto == null)
                return BadRequest("DTO boş olamaz.");

            // Basit örnek: MemoryCache ile saklayabiliriz
            _memoryCache.Set("genericData", genericDataDto, TimeSpan.FromMinutes(10));
            Console.WriteLine();
            return Ok(new { Message = "DTO alındı" });
        }
        [HttpPost("upload-files")]
        public async Task<IActionResult> UploadFiles([FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return BadRequest("En az bir dosya yüklemelisiniz.");


            // DTO'yu cache'den al
            if (!_memoryCache.TryGetValue("genericData", out GenericDataDto? genericDataDto))
                return BadRequest("DTO bulunamadı veya süresi dolmuş.");

            var fileDtos = new List<FileUploadDto>();
            foreach (var file in files)
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                fileDtos.Add(new FileUploadDto(file.FileName, ms.ToArray()));
            }

            // SBIF oluştur
            var memoryStream = await _sbifService.GenerateSbifAsync(fileDtos, genericDataDto);

            return File(memoryStream, "application/xml", "sbif_zengin.xml");
        }

    }
}
