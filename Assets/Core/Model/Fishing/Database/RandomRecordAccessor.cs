using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Fishbot.Model.Fishing.Database
{
    public class RandomRecordAccessor : MonoBehaviour
    {
        private Random _rng;
        private RecordsAccessor _records;

        private IEnumerable<FishRecord> _cache;
        private int _cacheVersion;

        private void Awake()
        {
            _rng = new Random();
            _records = FindObjectOfType<RecordsAccessor>();
        }

        private int _totalWeight;

        private void UpdateCache()
        {
            _cache = _records.Query.ToEnumerable();

            _totalWeight = 0;
            foreach (var record in _cache)
            {
                _totalWeight += record.Probability;
            }
        }

        public FishRecord GetRandom()
        {
            if (_records.Version > _cacheVersion)
            {
                UpdateCache();
            }

            var rnd = _rng.Next(0, _totalWeight);
            foreach (var fishRecord in _cache)
            {
                if (rnd < fishRecord.Probability)
                {
                    return fishRecord;
                }

                rnd -= fishRecord.Probability;
            }

            throw new Exception("Didn't get any random fish record");
        }
    }
}