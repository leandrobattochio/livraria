using Livraria.Core.Domain.Entity;
using Livraria.Domain.Core;

namespace Livraria.Domain.Models;

public class Book : BaseEntity, IAggregateRoot
{
    protected Book()
    {
        
    }
    
    public Book(string name, string publisher, DateTime publicationDate)
    {
        Name = name;
        Publisher = publisher;
        PublicationDate = publicationDate;
    }

    public string Name { get; init; }
    public string Publisher { get; init; }
    public DateTime PublicationDate { get; init; }
}