using Microsoft.EntityFrameworkCore;

namespace ent_server.Models
{
    public class UserContext: DbContext
    {
        public UserContext(DbContextOptions<UserContext> options): base(options) { }

        public DbSet<User> Users { get; set; }


    }
}
