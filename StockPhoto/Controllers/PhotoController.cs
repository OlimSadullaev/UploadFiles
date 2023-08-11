using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockPhoto.Data;
using StockPhoto.Entities;
using StockPhoto.Services;
using System.Reflection.PortableExecutable;

namespace StockPhoto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly StockPhotoDbContext _context;
        private readonly IAltPhotoService  _pService;

        public PhotoController(StockPhotoDbContext context, IAltPhotoService photoService)
        {
            _context = context;
            _pService = photoService;

        }

        /*[HttpGet]
        public async Task<IActionResult> GetAltPhotosAsync()
        {
            var photos = await _pService.GetAltPhotosAsync();

            if(photos == default || photos.Count() < 1)
            {
                return BadRequest("Are missing something");
            }

            return Ok(photos.Select(p => new
            {
                Id = p.Id,
                Name = p.Name,
                Link = p.Link,
                Original_size = p.Original_Size,
                Date_Of_Creation = p.Date_Of_Creation,
                Author = p.Author,
                Cost = p.Cost,
                Num_of_Sales = p.Num_of_Sales,
                Photo_Rating = p.Photo_Rating,
            })) ;

        }*/


        [HttpGet("id")]
        public async Task<ActionResult<List<AltPhoto>>> GetPhotoById(int id)
        {
            var characters = await _context.Photos
                .Where(c => c.Id == id)
                .ToListAsync();

            if (characters.Count < 0)
            {
                return BadRequest($"You do not have {id} id");
            }
            else
            {
                return Ok(characters);
            }
        }

        

            
        
    }
}
