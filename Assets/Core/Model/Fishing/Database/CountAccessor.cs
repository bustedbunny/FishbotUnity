using System;
using System.Text;
using LiteDB;

namespace Fishbot.Model.Fishing.Database
{
    public class CountAccessor : DatabaseAccessor<FishCount>
    {
        protected override string TableName => "FishCounts";

        private RecordsAccessor _records;

        private void Awake()
        {
            _records = FindObjectOfType<RecordsAccessor>();

            var mapper = BsonMapper.Global;
            mapper.Entity<FishCount>().DbRef(x => x.FishRecord, "FishRecords");
        }

        public string DisplayCountForUser(string user)
        {
            var counts = Query.Where(x => x.TwitchUserID == user).ToEnumerable();
            var sb = new StringBuilder();

            foreach (var fishCount in counts)
            {
                var fishRecord = _records.Query.Where(x => x.ID == fishCount.FishRecord.ID).First();
                sb.Append($"{fishRecord.Label}: {fishCount.Count}; ");
            }

            return sb.ToString();
        }

        public void AddCount(long recordID, string user)
        {
            var count = Collection.FindOne(x => x.FishRecord.ID == recordID && x.TwitchUserID == user);
            if (count is null)
            {
                var record = _records.Query.Where(x => x.ID == recordID).FirstOrDefault();

                if (record is null) throw new NullReferenceException();

                Add(new FishCount
                {
                    FishRecord = record,
                    TwitchUserID = user,
                    Count = 1
                });
            }
            else
            {
                count.Count++;
                Collection.Update(count);
            }
        }
    }
}