using Mandalium.Core.Generic.Collections;
using Mandalium.Demo.Core.Service.Services;
using Mandalium.Demo.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.Mime;
using System.Threading;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BlogDto))]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            PagedCollection<BlogDto> blogs = await _blogService.GetAllPaged(cancellationToken);
            if (blogs.IsNull() || !blogs.Collection.Any())
                return NotFound();

            return Ok(blogs);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            bool result = await _blogService.Delete(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BlogDto))]
        public async Task<IActionResult> Create(BlogDto blogDto, CancellationToken cancellationToken = default)
        {
            BlogDto result = await _blogService.Create(blogDto);
            if (result.IsNull())
            {
                return NotFound();
            }
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(BlogDto blogDto, CancellationToken cancellationToken = default)
        {
            var result = await _blogService.Update(blogDto);
            if (!result)
                return NotFound();

            return NoContent();
        }



        [HttpGet("[action]", Name = "{blogId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentDto))]
        public async Task<IActionResult> GetAllComments(int blogId = 0, CancellationToken cancellationToken = default)
        {
            PagedCollection<CommentDto> result = await _commentService.GetCommentsPaged(blogId, 1, 20);
            if (result.IsNull())
                return NotFound();

            return Ok(result);
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CommentDto))]
        public async Task<IActionResult> CreateComment(CommentDto commentDto, CancellationToken cancellationToken = default)
        {
            var result = await _commentService.CreateComment(commentDto);
            if (result.IsNull())
                return NotFound();

            return StatusCode(StatusCodes.Status201Created);
        }


        [HttpDelete("[action]", Name = "{commentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteComment(int commentId, CancellationToken cancellationToken = default)
        {
            var result = await _commentService.DeleteComment(commentId);
            if (!result)
                return NotFound();
            return NoContent();
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
