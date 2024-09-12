using Livraria.Core.Infrastructure;
using Livraria.Domain.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Livraria.Domain.Repository.Books;

public interface IBookRepository : IRepository<Book>
{
    ValueTask<EntityEntry<Book>> AddAsync(Book value, CancellationToken cancellationToken = default);

    Task<BookReadModel?> GetBookReadModelAsync(int bookId, CancellationToken cancellationToken = default);
}