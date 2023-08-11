using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTask.Entities.Enums;
using TestTask.Interfaces;
using TestTask.Services;

namespace TestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUploadService uploadService;
        private readonly IUserService userService;

        public UserController(IUploadService uploadService, IUserService userService)
        {
            this.uploadService = uploadService;
            this.userService = userService;
        }

        //Endpoint 1
        [HttpPost("upload")]
        public async Task<IActionResult> UploadCsvFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length <= 0)
                {
                    return BadRequest("No file uploaded.");
                }

                string uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadsDirectory))
                {
                    Directory.CreateDirectory(uploadsDirectory);
                }

                // Generate a unique file name
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadsDirectory, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                var userCsvDataList = await uploadService.ParseCsvDataAsync(filePath);

                foreach (var userCsvData in userCsvDataList)
                {
                    if (await userService.UserExistsAsync(userCsvData.UserIdentifier))
                    {
                        await userService.UpdateOrAddUserAsync(userCsvData);
                    }
                    else
                    {
                        await userService.AddUserAsync(userCsvData);
                    }
                }

                return Ok("CSV data uploaded and processed successfully.");
            }
            catch (Exception ex)
            {
                Exception innerException = ex.InnerException;

                Console.WriteLine("Original Exception Message: " + ex.Message);
                Console.WriteLine("Original Exception Stack Trace: " + ex.StackTrace);

                if (innerException != null)
                {
                    Console.WriteLine("Inner Exception Message: " + innerException.Message);
                    Console.WriteLine("Inner Exception Stack Trace: " + innerException.StackTrace);
                }

                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // Endpoint 2
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers(
        string sortBy = "username",
        SortDirection sortDirection = SortDirection.Ascending,
        int? limit = null)
        {
            try
            {
                if (limit.HasValue && limit <= 0)
                {
                    return BadRequest("Invalid limit value.");
                }

                var users = await userService.GetUsersAsync(sortBy, sortDirection, limit);

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
