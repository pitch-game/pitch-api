using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Pitch.Store.API.Infrastructure
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }


    public interface IDataContext<T> where T : IEntity
    {
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> query);
        Task<IList<T>> WhereAsync(Expression<Func<T, bool>> query);
        Task CreateAsync(T item);
        Task UpdateAsync(T item);
        Task DeleteAsync(T item);
    }

    public class MongoDbDataContext<T> : IDataContext<T> where T : IEntity
    {
        private readonly IMongoCollection<T> _collection;

        public MongoDbDataContext(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("store"); //TODO name
            _collection = database.GetCollection<T>("packs"); //TODO name
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> query)
        {
            return await _collection.AsQueryable().Where(query).FirstOrDefaultAsync();
        }

        public async Task<IList<T>> WhereAsync(Expression<Func<T, bool>> query)
        {
            return await _collection.AsQueryable().Where(query).ToListAsync();
        }

        public async Task CreateAsync(T item)
        {
            await _collection.InsertOneAsync(item);
        }

        public async Task UpdateAsync(T item)
        {
            await _collection.ReplaceOneAsync(x => x.Id == item.Id, item);
        }

        public async Task DeleteAsync(T item)
        {
            await _collection.DeleteOneAsync(x => x.Id == item.Id);
        }
    }
}
