using AutoMapper;
using Mandalium.Core.Abstractions.Interfaces;
using Mandalium.Core.Generic.Collections;
using Mandalium.Core.Persistence.Specifications;
using Mandalium.Demo.Core.Helpers;
using Mandalium.Demo.Models.DomainModels;
using Mandalium.Demo.Models.Dtos;
using Microsoft.Extensions.Caching.Memory;

namespace Mandalium.Demo.Core.Service.Services
{
    public class BlogService : ServiceBase
    {
        private readonly IGenericRepository<Blog> _blogRepository;
        private readonly IGenericRepository<Topic> _topicRepository;
        private readonly IGenericRepository<Comment> _commentRepository;

        public BlogService(IUnitOfWork unitOfWork, IMemoryCache memoryCache, IMapper mapper) : base(unitOfWork, memoryCache, mapper)
        {
            _blogRepository = _unitOfWork.GetRepository<Blog>();
            _topicRepository = _unitOfWork.GetRepository<Topic>();
            _commentRepository = _unitOfWork.GetRepository<Comment>();
        }

        public async Task<PagedCollection<BlogDto>> GetAllPaged(CancellationToken cancellationToken = default)
        {
            PagedCollection<BlogDto> dtos = new PagedCollection<BlogDto>();
            try
            {
                var blogs = await _memoryCache.GetOrCreateAsync(CacheKeys.GetAllBlogsKey, entry =>
                {
                    int pageIndex = 1;
                    int pageSize = 20;
                    var specification = new PagedSpecification<Blog>(pageIndex, pageSize);

                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                    return _blogRepository.GetAllPaged(specification, cancellationToken);
                });

                return _mapper.Map<PagedCollection<BlogDto>>(blogs);
            }
            catch (Exception ex)
            {
                Utility.ReportError(ex);
            }
            return dtos;
        }

        public async Task<BlogDto> Create(BlogDto blogDto, CancellationToken cancellationToken = default)
        {
            try
            {
                if (blogDto.IsNull())
                    return null;

                Topic topic = await _topicRepository.Get(blogDto.TopicId, cancellationToken);
                if (topic.IsNull())
                    return null;

                Blog blog = _mapper.Map<Blog>(blogDto);

                Utility.CleanXss<Blog>(blog);

                await _blogRepository.Save(blog, cancellationToken);
                await _unitOfWork.Save(cancellationToken);

                _memoryCache.Remove(CacheKeys.GetAllBlogsKey);

            }
            catch (Exception ex)
            {
                Utility.ReportError(ex);
                return null;
            }
           
            return blogDto;
        }


        public async Task<bool> Update(BlogDto blogDto , CancellationToken cancellationToken = default)
        {
            try
            {
                if (blogDto.IsNull())
                    return false;

                Blog blog = await _blogRepository.Get(blogDto.Id, cancellationToken);
                Topic topic = await _topicRepository.Get(blogDto.TopicId, cancellationToken);
                if (blog.IsNull() || topic.IsNull())
                    return false;

                Utility.CleanXss<BlogDto>(blogDto);

                blog = _mapper.Map<Blog>(blogDto);

                await _blogRepository.Detach(blog, cancellationToken);
                await _blogRepository.Update(blog, cancellationToken);
                await _unitOfWork.Save(cancellationToken);

                _memoryCache.Remove(CacheKeys.GetAllBlogsKey);
            }
            catch (Exception ex)
            {
                Utility.ReportError(ex);
                return false;
            }
            return true;
        }


        public async Task<bool> Delete(int id , CancellationToken cancellationToken = default)
        {
            try
            {
                var blog = await _blogRepository.Get(id, cancellationToken);
                if (blog == null || blog.PublishStatus == Models.Enums.PublishStatus.Deleted)
                    return false;

                blog.PublishStatus = Models.Enums.PublishStatus.Deleted;

                await _blogRepository.Update(blog, cancellationToken);
                await _unitOfWork.Save(cancellationToken);

                _memoryCache.Remove(CacheKeys.GetAllBlogsKey);
            }
            catch (Exception ex)
            {
                Utility.ReportError(ex);
                return false;
            }
          
            return true;
        }


        public async Task ClearCache()
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
        }



    }
}
