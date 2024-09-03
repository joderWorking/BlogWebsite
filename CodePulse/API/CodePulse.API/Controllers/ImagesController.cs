using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO.BlogImage;
using CodePulse.API.Services.Constracts;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    private readonly IBlogImageService _blogImageService;

    public ImagesController(IBlogImageService blogImageService)
    {
        _blogImageService = blogImageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllImage()
    {
        var images = await _blogImageService.GetAllAsync();

        if (images == null) return NotFound("No images found.");

        var listImageDtos = images.Select(x => new BlogImageDto
        {
            Id = x.Id,
            DateCreated = x.DateCreated,
            FileExtension = x.FileExtension,
            FileName = x.FileName,
            Title = x.Title,
            Url = x.Url
        }).ToList();

        return Ok(listImageDtos);
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile file, [FromForm] string fileName, [FromForm] string title)
    {
        ValidateFileUpload(file);

        if (ModelState.IsValid)
        {
            if (file == null || file.Length == 0) return BadRequest("No file provided.");
            // file upload
            var blogImage = new BlogImage
            {
                FileExtension = Path.GetExtension(file.FileName).ToLower(),
                FileName = fileName,
                Title = title,
                DateCreated = DateTime.Now
            };
            var retBlogImage = await _blogImageService.UploadImage(file, blogImage);

            var blogImageDto = new BlogImageDto
            {
                Id = retBlogImage.Id,
                DateCreated = retBlogImage.DateCreated,
                FileName = retBlogImage.FileName,
                FileExtension = retBlogImage.FileExtension,
                Title = retBlogImage.Title,
                Url = retBlogImage.Url
            };

            return Ok(blogImageDto);
        }

        return BadRequest(ModelState);
    }

    private void ValidateFileUpload(IFormFile file)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
        {
            Console.WriteLine(Path.GetExtension(file.FileName).ToLower());
            ModelState.AddModelError("file", "Unsupported file format");
        }

        if (file.Length > 10485760) ModelState.AddModelError("file", "File size cannot be more than 10MB");
    }
}