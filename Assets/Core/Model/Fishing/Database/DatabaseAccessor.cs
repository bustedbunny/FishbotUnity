using System;
using LiteDB;
using UnityEngine;

namespace Fishbot.Model.Fishing.Database
{
    public abstract class DatabaseAccessor<T> : MonoBehaviour
    {
        [SerializeField] private FishDatabaseLiteDB database;
        [SerializeField] private BsonAutoId autoId = BsonAutoId.ObjectId;

        public int Version { get; private set; } = 1;

        protected abstract string TableName { get; }
        private ILiteDatabase _db;

        protected ILiteCollection<T> Collection { get; private set; }

        protected virtual void Start()
        {
            _db = database.Database;
            Collection = _db.GetCollection<T>(TableName, autoId);
        }

        public ILiteQueryable<T> Query => Collection.Query();

        public bool UpdateItem(T obj)
        {
            Version++;
            return Collection.Update(obj);
        }

        public bool Remove(long id)
        {
            Version++;
            return Collection.Delete(id);
        }

        public void Add(T obj)
        {
            Version++;
            Collection.Insert(obj);
        }
    }
}