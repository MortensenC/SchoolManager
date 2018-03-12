using SchoolManager.Domain.Entities;
using System.Data.Entity;

namespace SchoolManager.Domain
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<RegistrationRequest> RegistrationRequests { get; set; }
    }
}