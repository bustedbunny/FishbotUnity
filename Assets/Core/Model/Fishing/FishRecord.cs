using System;
using LiteDB;

namespace Fishbot.Model.Fishing
{
    public class FishRecord
    {
        public FishRecord(string label)
        {
            Label = label;
            Probability = default;
            Message = default;
        }

        public long ID { get; set; }
        public string Label { get; set; }
        public int Probability { get; set; }
        public string Message { get; set; }
    }

    public class FishCount
    {
        public ObjectId ID { get; set; }
        public FishRecord FishRecord { get; set; }

        public string TwitchUserID { get; set; }

        public int Count { get; set; }
    }
}