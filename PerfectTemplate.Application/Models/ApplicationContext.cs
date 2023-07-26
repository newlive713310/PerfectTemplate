using Microsoft.EntityFrameworkCore;
using PerfectTemplate.Application.Models.Entities;

namespace PerfectTemplate.Application.Models
{
    public class ApplicationContext : DbContext
    {
        public virtual DbSet<User> Groups { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    }
}
