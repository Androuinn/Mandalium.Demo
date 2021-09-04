using AutoMapper;
using Mandalium.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mandalium.API.Controllers
{
    public abstract class BaseController : ControllerBase 
    {
        internal readonly IUnitOfWork _unitOfWork;
        internal readonly IMemoryCache _memoryCache;
        internal readonly IMapper _mapper;

        public BaseController()
        {
         
        }

        public BaseController(IUnitOfWork unitOfWork, IMemoryCache memoryCache = null, IMapper mapper = null)
        {
            _unitOfWork = unitOfWork;
            if (memoryCache != null)
                _memoryCache = memoryCache;

            if (mapper != null)
                _mapper = mapper;
        }


    }
}
