using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Pitch.Match.Api.Infrastructure.Repositories.Contexts
{
    public class MongoDbDataContext<T> : IDataContext<T> where T : IEntity
    {
        private readonly IMongoCollection<T> _collection;

        public MongoDbDataContext(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("match"); //TODO name
            _collection = database.GetCollection<T>("matches"); //TODO name
        }

        public async Task<T> FindOneAsync(Expression<Func<T, bool>> query)
        {
            var result = await _collection.FindAsync(query);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> query)
        {
            var result = await _collection.FindAsync(query);
            return await result.ToListAsync();
        }

        public async Task CreateAsync(T item)
        {
            await _collection.InsertOneAsync(item);
        }

        public async Task UpdateAsync(T item)
        {
            var version = item.Version;
            item.Version++;
            await _collection.ReplaceOneAsync(x => x.Id == item.Id && x.Version == version, item);
        }
    }
}
