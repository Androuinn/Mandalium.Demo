using AutoMapper;
using Mandalium.API.App_Code;
using Mandalium.API.App_GlobalResources;
using Mandalium.Core.Abstractions.Interfaces;
using Mandalium.Core.Helpers;
using Mandalium.Infrastructure.Specifications;
using Mandalium.Models.DomainModels;
using Mandalium.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Mandalium.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : BaseController
    {

        //TODO DB defaults dont work. Also lengths doesnt apply
        private readonly IGenericRepository<Blog> _blogRepository;
        private readonly IGenericRepository<Topic> _topicRepository;
        private readonly IGenericRepository<Comment> _commentRepository;

        public BlogController(IUnitOfWork unitOfWork, IMemoryCache memoryCache, IMapper mapper) : base(unitOfWork, memoryCache, mapper)
        {
            _blogRepository = _unitOfWork.GetRepository<Blog>();
            _topicRepository = _unitOfWork.GetRepository<Topic>();
            _commentRepository = _unitOfWork.GetRepository<Comment>();

        }

        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BlogDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IEnumerable<Blog> blogs = await _memoryCache.GetOrCreateAsync(CacheKeys.GetAllBlogsKey, entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                    return _blogRepository.GetAll(new GenericSpecification<Blog>(true, (x => x.Topic),(x=> x.PublishStatus == Models.Enums.PublishStatus.Published)));
                });

                if (blogs == null || !blogs.Any())
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<IEnumerable<BlogDto>>(blogs));
            }
            catch (Exception ex)
            {
                Utility.ReportError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, LanguageResource.General_Error_Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var blog = await _blogRepository.Get(id);
                if (blog == null || blog.PublishStatus == Models.Enums.PublishStatus.Deleted)
                {
                    return NotFound();
                }

                blog.PublishStatus = Models.Enums.PublishStatus.Deleted;

                await _blogRepository.Update(blog);
                await _unitOfWork.Save();

                _memoryCache.Remove(CacheKeys.GetAllBlogsKey);

                return NoContent();

            }
            catch (Exception ex)
            {
                Utility.ReportError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, LanguageResource.General_Error_Message);
            }
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BlogDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(BlogDto blogDto)
        {
            try
            {
                Blog blog = _mapper.Map<Blog>(blogDto);
                Topic topic = await _topicRepository.Get(blogDto.TopicId);
                if (topic == null)
                {
                    return NotFound();
                }

                blog.Topic = topic;
                blog.TopicId = topic.Id;
                blog.CreatedOn = DateTime.Now;

                Utility.CleanXss<Blog>(blog);

                await _blogRepository.Save(blog);
                await _unitOfWork.Save();

                _memoryCache.Remove(CacheKeys.GetAllBlogsKey);

                return Ok(blogDto);
            }
            catch (Exception ex)
            {
                Utility.ReportError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, LanguageResource.General_Error_Message);
            }
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(BlogDto blogDto)
        {
            try
            {
                Blog blog = await _blogRepository.Get(blogDto.Id);
                Topic topic = await _topicRepository.Get(blogDto.TopicId);
                if (blog == null || topic == null)
                {
                    return NotFound();
                }
                Utility.CleanXss<BlogDto>(blogDto);

                blog.Headline = blogDto.Headline;
                blog.SubHeadline = blogDto.SubHeadline;
                blog.CodeArea = blogDto.CodeArea;
                blog.ModifiedOn = DateTime.Now;
                blog.ImageUrl = blogDto.ImageUrl;
                blog.Topic = topic;
                blog.TopicId = blogDto.TopicId;

                await _blogRepository.Update(blog);
                await _unitOfWork.Save();

                _memoryCache.Remove(CacheKeys.GetAllBlogsKey);

                return NoContent();
            }
            catch (Exception ex)
            {
                Utility.ReportError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, LanguageResource.General_Error_Message);
            }
        }



        [HttpGet("[action]", Name = "{blogId}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllComments(int blogId = 0)
        {
            try
            {
                Blog blog = await _blogRepository.Get(blogId);

                if (blogId != 0 && (blog == null || blog.PublishStatus == Models.Enums.PublishStatus.Deleted))
                {
                    return NotFound();
                }

                var comments = await _memoryCache.GetOrCreateAsync(string.Format(CacheKeys.CommentKey, blogId), entry =>
                  {
                      entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);

                      if (blogId != 0)
                      {
                          return _commentRepository.GetAll(new GenericSpecification<Comment>(x => x.BlogId == blogId && x.PublishStatus == Models.Enums.PublishStatus.Published));
                      }
                      else
                      {
                          return _commentRepository.GetAll(new GenericSpecification<Comment>(x=> x.PublishStatus == Models.Enums.PublishStatus.Published));
                      }
                  });

                return Ok(_mapper.Map<IEnumerable<CommentDto>>(comments));

            }
            catch (Exception ex)
            {
                Utility.ReportError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, LanguageResource.General_Error_Message);
            }
        }

        [HttpPost("[action]")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateComment(CommentDto commentDto)
        {
            try
            {
                Blog blog = await _blogRepository.Get(commentDto.BlogId);
                if (blog == null)
                {
                    return NotFound();
                }

                Comment comment = _mapper.Map<Comment>(commentDto);
                //TODO DB defaults dont work. Also lengths doesnt apply
                comment.CreatedOn = DateTime.Now;
                
                Utility.CleanXss<Comment>(comment);

                await _commentRepository.Save(comment);
                await _unitOfWork.Save();
                _memoryCache.Remove(string.Format(CacheKeys.CommentKey, commentDto.BlogId));

                return Ok(commentDto);
            }
            catch (Exception ex)
            {
                Utility.ReportError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, LanguageResource.General_Error_Message);
            }
        }


        [HttpDelete("[action]", Name = "{commentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            try
            {
                Comment comment = await _commentRepository.Get(commentId);
                if (comment == null || comment.PublishStatus == Models.Enums.PublishStatus.Deleted)
                {
                    return NotFound();
                }

                comment.PublishStatus = Models.Enums.PublishStatus.Deleted;

                await _commentRepository.Update(comment);
                await _unitOfWork.Save();

                _memoryCache.Remove(string.Format(CacheKeys.CommentKey, comment.BlogId));

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception ex)
            {
                Utility.ReportError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, LanguageResource.General_Error_Message);
            }
        }









#if DEBUG
        [HttpGet("[action]")]
        public async Task<IActionResult> ClearCache()
        {
            var blogs = await _blogRepository.GetAll();
            await Task.Run(() =>
            {
                foreach (var blog in blogs)
                {
                    _memoryCache.Remove(string.Format(CacheKeys.CommentKey, blog.Id));
                }
                _memoryCache.Remove(CacheKeys.GetAllBlogsKey);
            });

            return Ok();
        }

#endif


    }
}
