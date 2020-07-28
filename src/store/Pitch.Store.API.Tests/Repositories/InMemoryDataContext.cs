using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Pitch.Store.API.Infrastructure;

namespace Pitch.Store.API.Tests.Repositories
{
    public class InMemoryDataContext<T> : IDataContext<T> where T : IEntity
    {
        public InMemoryDataContext(List<T> inMemoryList = null)
        {
            InMemoryList = inMemoryList ?? new List<T>();
        }

        private IList<T> InMemoryList { get; set; }

        public Task CreateAsync(T item)
        {
            InMemoryList.Add(item);
            return Task.CompletedTask;
        }

        public Task<T> FindOneAsync(Expression<Func<T, bool>> query)
        {
            return Task.FromResult(InMemoryList.FirstOrDefault(query.Compile()));
        }

        public Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> query)
        {
            return Task.FromResult(InMemoryList.Where(query.Compile()));
        }

        public Task UpdateAsync(T item)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Expression<Func<T, bool>> filter)
        {
            return Task.FromResult(InMemoryList.Except(InMemoryList.Where(filter.Compile())));
        }
    }
}