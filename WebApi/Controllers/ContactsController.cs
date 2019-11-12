using Lavoro.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApi.Repositories;

namespace WebApi.Controllers
{

    [Route("api/[controller]-[action]")]
    [ApiController]
    public class ContactsController : BaseController
    {

        public ContactsController(LaboroContext context) : base(context)
        {

        }
        

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<object>> Contacts(int id)
        {
            var repository = new ContactsRepository(context);
            var result = repository.GetDetailedContactsForUser(id);
            return result;
        }


        [HttpGet("{id}")]
        public new ActionResult<object> User(int id)
        {
            var repository = new UserRepository(context);
            var result = repository.GetUserWithStarredFrequentContacts(id);
            return result;

        }



    }
}
