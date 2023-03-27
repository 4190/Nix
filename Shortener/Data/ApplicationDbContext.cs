using Microsoft.EntityFrameworkCore;
using Shortener.Models;

namespace Shortener.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<ShortenedLink> shortenedLinks { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}