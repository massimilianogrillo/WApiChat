using Extensions;
using Lavoro.Data;
using Lavoro.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Repositories;
using WebApi.Services;

namespace WebApi.Controllers
{

    [Route("api/[controller]-[action]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        private IUserService authenticationService;
        public AuthenticationController(LaboroContext context, IUserService service) : base(context) {
            this.authenticationService = service;
        }

        [HttpPost]
        public ActionResult<object> Login(object userData)
        {
            var values = userData as JObject;
            authenticationService.ValidateCredentials(values["email"].ToString(), values["password"].ToString(), out User user);
            return user == null? null: new { id = user.Id };
            
        }
        [HttpPost]
        public ActionResult<int> AddUser(User userData)
        {
            return authenticationService.Register(userData);
        }


    }
}
