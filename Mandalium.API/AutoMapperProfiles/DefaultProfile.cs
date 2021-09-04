using AutoMapper;
using Mandalium.Models.DomainModels;
using Mandalium.Models.Dtos;

namespace Mandalium.API.AutoMapperProfiles
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<Blog, BlogDto>().ForMember(x => x.TopicName, dest => dest.MapFrom(opt => opt.Topic.Name)).ReverseMap();
            CreateMap<Comment, CommentDto>().ReverseMap();
        }



    }
}
