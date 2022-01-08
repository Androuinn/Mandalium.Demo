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

        protected ServiceBase(IUnitOfWork unitOfWork, IMemoryCache memoryCache = null, IMapper mapper = null)
        {
            _unitOfWork = unitOfWork;
            if (memoryCache != null)
                _memoryCache = memoryCache;

            if (mapper != null)
                _mapper = mapper;
        }




    }
}
