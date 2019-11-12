using Lavoro.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Repositories
{
    public class BaseRepository: IRepository
    {
        internal LaboroContext context;
        public BaseRepository(LaboroContext context)
        {
            this.context = context;
        }
    }
}
