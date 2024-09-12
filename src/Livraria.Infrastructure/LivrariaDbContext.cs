using Livraria.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Infrastructure;

public class LivrariaDbContext : DbContext
{
    protected LivrariaDbContext()
    {
        
    }
    
    public LivrariaDbContext(DbContextOptions<LivrariaDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();
}