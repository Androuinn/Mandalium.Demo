using AutoMapper;
using Mandalium.Core.Abstractions.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Mandalium.Demo.Core.Service.Services
{
    public abstract class ServiceBase
    {
        internal readonly IUnitOfWork _unitOfWork;
        internal readonly IMemoryCache _memoryCache;
        internal readonly IMapper _mapper;

        protected ServiceBase() { }

        protected ServiceBase(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;


        protected ServiceBase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        protected ServiceBase(IUnitOfWork unitOfWork, IMemoryCache memoryCache, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
            _mapper = mapper;
        }




    }
}
