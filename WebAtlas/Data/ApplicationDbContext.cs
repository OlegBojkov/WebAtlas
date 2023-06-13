using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebAtlas.Models;

namespace WebAtlas.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Address> Adresses { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<News> News { get; set; }
    }
}
