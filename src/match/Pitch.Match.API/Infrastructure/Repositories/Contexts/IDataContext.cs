using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pitch.Match.API.Infrastructure.Repositories.Contexts
{
    public interface IEntity
    {
        Guid Id { get; set; }
        int Version { get; set; }
    }

    public interface IDataContext<T> where T : IEntity
    {
        Task<T> FindOneAsync(Expression<Func<T, bool>> query);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> query);
        Task CreateAsync(T item);
        Task UpdateAsync(T item);
    }
}
