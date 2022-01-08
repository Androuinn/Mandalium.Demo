using AutoMapper;
using Mandalium.Core.Generic.Collections;
using Mandalium.Demo.Models.DomainModels;
using Mandalium.Demo.Models.Dtos;

namespace Mandalium.API.AutoMapperProfiles
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<Blog, BlogDto>().ForMember(x => x.TopicName, dest => dest.MapFrom(opt => opt.Topic.Name));
            CreateMap<BlogDto, Blog>().ConstructUsing(x => new Blog(x.TopicId));
            CreateMap<Comment, CommentDto>();
            CreateMap<CommentDto, Comment>().ConstructUsing(x => new Comment(x.BlogId));
            CreateMap<PagedCollection<Comment>, PagedCollection<CommentDto>>();
            CreateMap<PagedCollection<Blog>, PagedCollection<BlogDto>>();


        }



    }
}
