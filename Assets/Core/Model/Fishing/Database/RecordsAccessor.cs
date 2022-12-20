using System;
using System.Collections.ObjectModel;
using System.Linq;
using LiteDB;

namespace Fishbot.Model.Fishing.Database
{
    public class RecordsAccessor : DatabaseAccessor<FishRecord>
    {
        protected override string TableName => "FishRecords";
        protected override void Start()
        {
            base.Start();
            Collection.EnsureIndex(x => x.Label, true);

            Migrate();
        }

        private void Migrate()
        {
            var fileLoader = new JsonFileLoader<ObservableCollection<FishRecord>>("FishList");
            var loadedDatabase = fileLoader.Read();

            if (loadedDatabase != null)
            {
                var existing = Collection.FindAll();
                var filtered = loadedDatabase.Where(x => existing.All(y => y.Label != x.Label));
                Collection.InsertBulk(filtered);

                loadedDatabase.Clear();
                fileLoader.Write(loadedDatabase);
            }
        }
    }
}