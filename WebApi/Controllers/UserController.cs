using Lavoro.Data;
using Lavoro.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Hubs;
using WebApi.Repositories;

namespace WebApi.Controllers
{

    [Route("api/[controller]-[action]")]
    [ApiController]
    public class UserController : BaseController
    {

        public UserController(LaboroContext context) : base(context){
        }
        
        [HttpGet("{id}")]
        public new ActionResult<object> User(int id)
        {
            //Class.FillDB(context);
            var repository = new UserRepository(context);
            var result = repository.GetUser(id);
            return result;
        }


    }
}
