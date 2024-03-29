﻿using JobOfferBackend.Domain.Common;
using JobOfferBackend.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOfferBackend.DataAccess
{
    /// <summary>
    /// Intentionally it doesn't implements an Interface because it could create a fake abstraction
    /// See https://medium.com/@dcamacho31/foreword-224a02be04f8
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseRepository<T> where T : IIdentity<T>
    {
        protected IMongoDatabase _database;

        public BaseRepository(IMongoDatabase database)
        {
            _database = database;
            Collection = _database.GetCollection<T>(typeof(T).Name);
        }

        protected IMongoCollection<T> Collection { get; set; }

        public IClientSessionHandle GetTransactionalSession()
        {
            return _database.Client.StartSession();
        }

        public string CreateId()
        {
            return ObjectId.GenerateNewId(DateTime.UtcNow).ToString();
        }

        public virtual async Task<T> GetByIdAsync(string id)
        {
            return await Collection.Find(p => p.Id == id).SingleOrDefaultAsync();
        }

        public virtual async Task<bool> CheckEntityExistsAsync(string id)
        {
            return await Collection.CountDocumentsAsync(p => p.Id == id) == 1;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Collection.AsQueryable().ToListAsync();
        }

        public virtual async Task UpsertAsync(T entity)
        {
            if (!entity.HasIdCreated)
            {
                entity.Id = CreateId();
            }

            await Collection.ReplaceOneAsync(p => p.Id == entity.Id, entity, new ReplaceOptions() { IsUpsert = true });

        }

        public virtual async Task<DeleteResult> RemoveOneAsync(T entity)
        {
            return await Collection.DeleteOneAsync<T>(p => p.Id == entity.Id);
        }
    }
}
