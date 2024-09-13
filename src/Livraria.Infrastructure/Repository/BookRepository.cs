using Livraria.Core.Infrastructure;
using Livraria.Domain.Models;
using Livraria.Domain.Repository.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Livraria.Infrastructure.Repository;

public class BookRepository(LivrariaDbContext dbContext) : IBookRepository
{
    public ValueTask<EntityEntry<Book>> AddAsync(Book value, CancellationToken cancellationToken = default)
    {
        return dbContext
            .Books
            .AddAsync(value, cancellationToken);
    }

    public Task<BookReadModel?> GetBookReadModelAsync(int bookId, CancellationToken cancellationToken = default)
    {
        return dbContext
            .Books
            .Where(c => c.Id == bookId)
            .Select(c => new BookReadModel(c.Id, c.Name, c.Publisher, c.PublicationDate))
            .FirstOrDefaultAsync(cancellationToken);
    }
}