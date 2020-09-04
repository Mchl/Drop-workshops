using System;
using System.Threading.Tasks;
using Drop.Core.Entities;
using Drop.Core.Repositories;
using Drop.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Drop.Infrastructure.Mongo.Repositories
{
    internal sealed class MongoParcelsRepository : IParcelsRepository
    {
        private readonly IMongoDatabase _database;

        public MongoParcelsRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Parcel> GetAsync(Guid id)
        {
            var parcel = await Collection
                .AsQueryable()
                .SingleOrDefaultAsync(d => d.Id == id);

            return parcel is null ? null : new Parcel(parcel.Id, parcel.Size, parcel.Address, parcel.State);
        }

        public Task AddAsync(Parcel parcel)
            => Collection.InsertOneAsync(new ParcelDocument(parcel));

        public Task UpdateAsync(Parcel parcel)
            => Collection.ReplaceOneAsync(p => p.Id == parcel.Id, new ParcelDocument(parcel));

        private IMongoCollection<ParcelDocument> Collection => _database.GetCollection<ParcelDocument>("parcels");
    }
}