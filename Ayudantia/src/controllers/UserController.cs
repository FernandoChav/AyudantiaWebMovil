using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Data;
using Ayudantia.Src.Dtos;
using Ayudantia.Src.Models;

using Microsoft.AspNetCore.Mvc;

namespace Ayudantia.Src.Controllers
{
    public class UserController(ILogger<UserController> logger, UnitOfWork unitOfWork) : BaseController
    {
        private readonly ILogger<UserController> _logger = logger;
        private readonly UnitOfWork _context = unitOfWork;



    }
}