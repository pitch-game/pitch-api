using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pitch.Match.API.Infrastructure.Repositories.Contexts
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }

    public interface IDataContext<T> where T : IEntity
    {
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> query);
        Task<IEnumerable<T>> ToListAsync(Expression<Func<T, bool>> query);
        Task CreateAsync(T item);
        Task UpdateAsync(T item);
    }
}
