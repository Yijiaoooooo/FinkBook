﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Api.Dtos;

namespace User.Api.Controllers
{
    public class BaseController : Controller
    {
        protected UserIdentity userIdentity => new UserIdentity { UserId = 1, Name = "yijia" };


    }
}
