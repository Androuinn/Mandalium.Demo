using AutoMapper;
using Mandalium.Core.Abstractions.Interfaces;
using Mandalium.Core.Generic.Collections;
using Mandalium.Core.Persistence.Specifications;
using Mandalium.Demo.Core.Helpers;
using Mandalium.Demo.Models.DomainModels;
using Mandalium.Demo.Models.Dtos;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandalium.Demo.Core.Service.Services
{
    public class CommentService : ServiceBase
    {
        private readonly IGenericRepository<Blog> _blogRepository;
        private readonly IGenericRepository<Topic> _topicRepository;
        private readonly IGenericRepository<Comment> _commentRepository;

        public CommentService(IUnitOfWork unitOfWork, IMemoryCache memoryCache, IMapper mapper) : base(unitOfWork, memoryCache, mapper)
        {
            _blogRepository = _unitOfWork.GetRepository<Blog>();
            _topicRepository = _unitOfWork.GetRepository<Topic>();
            _commentRepository = _unitOfWork.GetRepository<Comment>();
        }

        public async Task<PagedCollection<CommentDto>> GetCommentsPaged(int blogId, int pageIndex, int pageSize)
        {
            PagedCollection<CommentDto> dtos = new PagedCollection<CommentDto>();
            try
            {
                Blog blog = await _blogRepository.Get(blogId);

                if (blog.IsNull() || blogId == 0 || blog.PublishStatus == Models.Enums.PublishStatus.Deleted)
                    return dtos;

                var comments = await _memoryCache.GetOrCreateAsync(string.Format(CacheKeys.CommentKey, blogId), entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                    return _commentRepository.GetAllPaged(new PagedSpecification<Comment>(pageIndex, pageSize, x => x.BlogId == blogId && x.PublishStatus == Models.Enums.PublishStatus.Published));
                });
                dtos = _mapper.Map<PagedCollection<CommentDto>>(comments);
            }
            catch (Exception ex)
            {
                Utility.ReportError(ex);
                return dtos;
            }
            return dtos;
        }

        public async Task<CommentDto> CreateComment(CommentDto commentDto)
        {
            try
            {
                Blog blog = await _blogRepository.Get(commentDto.BlogId);
                if (blog.IsNull())
                    return null;

                Comment comment = _mapper.Map<Comment>(commentDto);
                Utility.CleanXss<Comment>(comment);

                await _commentRepository.Save(comment);
                await _unitOfWork.Save();

                _memoryCache.Remove(string.Format(CacheKeys.CommentKey, commentDto.BlogId));

            }
            catch (Exception ex)
            {
                Utility.ReportError(ex);
                return null;
            }
            return commentDto;
        }

        public async Task<bool> DeleteComment(int id)
        {
            try
            {
                Comment comment = await _commentRepository.Get(id);
                if (comment.IsNull() || comment.PublishStatus == Models.Enums.PublishStatus.Deleted)
                    return false;

                comment.PublishStatus = Models.Enums.PublishStatus.Deleted;

                await _commentRepository.Detach(comment);
                await _commentRepository.Update(comment);
                await _unitOfWork.Save();

                _memoryCache.Remove(string.Format(CacheKeys.CommentKey, comment.BlogId));
            }
            catch (Exception ex)
            {
                Utility.ReportError(ex);
                return false;
            }
            return true;
        }
    }
}