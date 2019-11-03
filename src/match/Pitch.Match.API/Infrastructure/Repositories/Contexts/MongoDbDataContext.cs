using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Pitch.Match.API.Infrastructure.Repositories.Contexts
{
    public class MongoDbDataContext<T> : IDataContext<T> where T : IEntity
    {
        private readonly IMongoCollection<T> _collection;

        public MongoDbDataContext(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("match"); //TODO name
            _collection = database.GetCollection<T>("matches"); //TODO name
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> query)
        {
            return await _collection.AsQueryable().Where(query).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> ToListAsync(Expression<Func<T, bool>> query)
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
    }
}
