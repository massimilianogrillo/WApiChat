using Lavoro.Data;
using Lavoro.Domain;
using WebApi.Repositories;

namespace WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly LaboroContext context;
        public UserService(LaboroContext context)
        {
            this.context = context;
        }

        public bool ValidateCredentials(string email, string password, out User user)
        {
            user = null;
            var chunks = email.Split('@');
            if (chunks.Length > 0)
            {
                string username = chunks[0];
                var repository = new UserRepository(context);
                user = repository.GetUser(username);

            }

            return user != null;
        }
        public int Register(User user)
        {
            var repository = new UserRepository(context);
            return repository.AddUser(user);
        }
    }
}
