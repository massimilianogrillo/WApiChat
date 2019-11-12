using Lavoro.Domain;

namespace WebApi.Services
{
    public interface IUserService
    {
        /// <summary>
        /// change the imlementation of this method to authenticate the user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        bool ValidateCredentials(string email, string password, out User user);
        int Register(User user);
    }
}
