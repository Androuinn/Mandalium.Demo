using AutoMapper;
using Mandalium.API.App_Code;
using Mandalium.API.App_GlobalResources;
using Mandalium.Core.Helpers;
using Mandalium.Core.Interfaces;
using Mandalium.Infrastructure.Specifications;
using Mandalium.Models.DomainModels;
using Mandalium.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mandalium.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : BaseController
    {

        private readonly IGenericRepository<Blog> _blogRepository;

        public BlogController(IUnitOfWork unitOfWork, IMemoryCache memoryCache, IMapper mapper) : base(unitOfWork, memoryCache, mapper)
        {
            _blogRepository = _unitOfWork.GetRepository<Blog>();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IEnumerable<Blog> blogs = await _memoryCache.GetOrCreateAsync(CacheKeys.GetAllBlogsKey, entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                    return _blogRepository.GetAll(new GenericSpecification<Blog>(true,(x=> x.Topic), x=> x.Id == 2));
                });

                if (blogs == null || !blogs.Any())
                {
                    return NoContent();
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var blog = await _blogRepository.Get(id);
                if (blog == null || blog.PublishStatus == Models.Enums.PublishStatus.Deleted)
                {
                    return NoContent();
                }

                blog.PublishStatus = Models.Enums.PublishStatus.Deleted;

                await _blogRepository.Update(blog);
                await _unitOfWork.Save();

                _memoryCache.Remove(CacheKeys.GetAllBlogsKey);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception ex)
            {
                Utility.ReportError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, LanguageResource.General_Error_Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(BlogDto blogDto)
        {
            try
            {
                Blog blog = _mapper.Map<Blog>(blogDto);
                Topic topic = await _unitOfWork.GetRepository<Topic>().Get(blogDto.TopicId);
                if (topic == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent);
                }

                blog.Topic = topic;
                blog.TopicId = topic.Id;

                await _blogRepository.Save(blog);
                await _unitOfWork.Save();

                _memoryCache.Remove(CacheKeys.GetAllBlogsKey);

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                Utility.ReportError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, LanguageResource.General_Error_Message);
            }
        }


    }
}
