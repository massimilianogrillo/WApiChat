using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lavoro.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        internal LaboroContext context;
        public BaseController(LaboroContext context)
        {
            this.context = context;
        }
    }
}