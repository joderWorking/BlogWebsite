using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO.BlogPost;
using CodePulse.API.Models.DTO.Category;
using CodePulse.API.Services.Constracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogPostsController : ControllerBase
{
    private readonly IBlogPostService _blogPostService;
    private readonly ICategoryService _categoryService;

    public BlogPostsController(IBlogPostService blogPostService, ICategoryService categoryService)
    {
        _blogPostService = blogPostService;
        _categoryService = categoryService;
    }

    [HttpPost]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> AddBlogPostAsync([FromBody] CreateBlogPostRequestDto request)
    {
        var result = await _blogPostService.AddAsync(request);

        if (result != null)
            return Ok(result);
        return BadRequest("Add failed");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBlogPost()
    {
        var blogPostList = await _blogPostService.GetAllUpdatedAsync();
        var response = new List<BlogPostDto>();
        foreach (var blogPost in blogPostList)
            response.Add(new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                IsVisible = blogPost.IsVisible,
                Categories = blogPost.Categories?.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            });


        return Ok(response);
    }

    [HttpGet]
    [Route("{Id:guid}")]
    public async Task<IActionResult> GetBlogPostById([FromRoute] string Id)
    {
        try
        {
            var blogPostId = Guid.Parse(Id);
            var blogPost = await _blogPostService.GetByIdAsyc(blogPostId);
            if (blogPost is null) return NotFound("Item not found");

            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                PublishedDate = blogPost.PublishedDate,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Title = blogPost.Title,
                IsVisible = blogPost.IsVisible,
                ShortDescription = blogPost.ShortDescription,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories?.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };
            return Ok(response);
        }
        catch (FormatException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [Route("{Id}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> UpdateBlogPostById([FromRoute] string Id,
        [FromBody] UpdateBlogPostRequestDto request)
    {
        try
        {
            // Parse the Id string to a Guid
            var blogPostId = Guid.Parse(Id);

            // Get the existing blog post by Id
            var existBlogPost = await _blogPostService.GetByIdAsyc(blogPostId);

            // Check if the blog post was found
            if (existBlogPost == null) return NotFound("Item not found");

            // Update the existing blog post with new values
            existBlogPost.Author = request.Author;
            existBlogPost.Content = request.Content;
            existBlogPost.FeaturedImageUrl = request.FeaturedImageUrl;
            existBlogPost.IsVisible = request.IsVisible;
            existBlogPost.PublishedDate = request.PublishedDate;
            existBlogPost.ShortDescription = request.ShortDescription;
            existBlogPost.Title = request.Title;
            existBlogPost.UrlHandle = request.UrlHandle;

            // Update categories if provided
            if (request.Categories is not null)
            {
                if (existBlogPost.Categories is not null)
                    existBlogPost.Categories.Clear(); // Clear existing categories
                else
                    existBlogPost.Categories = new List<Category>();

                foreach (var categoryId in request.Categories)
                {
                    var existCategory = await _categoryService.GetCategoryById(categoryId);
                    if (existCategory is not null) existBlogPost.Categories.Add(existCategory);
                }
            }

            // Update the blog post
            var result = await _blogPostService.UpdateById(existBlogPost);

            // Return the appropriate response
            if (result)
                return Ok(existBlogPost);
            return NotFound("Failed to update the item");
        }
        catch (FormatException ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpDelete]
    [Route("{reqId}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> DeleteBlogPostAsync([FromRoute] string reqId)
    {
        try
        {
            var blogPostId = Guid.Parse(reqId);
            var blogPost = await _blogPostService.DeleteBlogPostAsync(blogPostId);
            if (blogPost is null) return NotFound("Item not found");
            return Ok(new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                PublishedDate = blogPost.PublishedDate,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Title = blogPost.Title,
                IsVisible = blogPost.IsVisible,
                ShortDescription = blogPost.ShortDescription,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories?.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            });
        }
        catch (FormatException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("{urlHandle}")]
    public async Task<IActionResult> GetBlogPostByUrl([FromRoute] string urlHandle)
    {
        var blogPost = await _blogPostService.GetByUrl(urlHandle);
        if (blogPost is not null)
        {
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                PublishedDate = blogPost.PublishedDate,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Title = blogPost.Title,
                IsVisible = blogPost.IsVisible,
                ShortDescription = blogPost.ShortDescription,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories?.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };
            return Ok(response);
        }

        return NotFound(blogPost);
    }
}