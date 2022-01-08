using Mandalium.Core.Generic.Collections;
using Mandalium.Demo.Core.Service.Services;
using Mandalium.Demo.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Mandalium.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : BaseController
    {
        private readonly BlogService _blogService;
        private readonly CommentService _commentService;

        public BlogController(BlogService blogService, CommentService commentService) : base()
        {
            _blogService = blogService;
            _commentService = commentService;
        }

        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BlogDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            PagedCollection<BlogDto> blogs = await _blogService.GetAllPaged();
            if (blogs.IsNull() || !blogs.Collection.Any())
                return NotFound();

            return Ok(blogs);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await _blogService.Delete(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BlogDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(BlogDto blogDto)
        {
            BlogDto result = await _blogService.Create(blogDto);
            if (result.IsNull())
            {
                return NotFound();
            }
            return Ok(blogDto);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(BlogDto blogDto)
        {
            var result = await _blogService.Update(blogDto);
            if (!result)
                return NotFound();

            return NoContent();
        }



        [HttpGet("[action]", Name = "{blogId}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllComments(int blogId = 0)
        {
            PagedCollection<CommentDto> result = await _commentService.GetCommentsPaged(blogId, 1, 20);
            if (result.IsNull())
                return NotFound();

            return Ok(result);
        }

        [HttpPost("[action]")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateComment(CommentDto commentDto)
        {
            var result = await _commentService.CreateComment(commentDto);
            if (result.IsNull())
                return NotFound();

            return Ok(commentDto);
        }


        [HttpDelete("[action]", Name = "{commentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var result = await _commentService.DeleteComment(commentId);
            if (!result)
                return NotFound();
            return StatusCode(StatusCodes.Status200OK);
        }


#if DEBUG
        [HttpGet("[action]")]
        protected async Task<IActionResult> ClearCache()
        {
            await _blogService.ClearCache();
            return Ok();
        }

#endif


    }
}
