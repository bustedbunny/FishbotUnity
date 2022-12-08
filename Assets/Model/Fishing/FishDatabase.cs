using System;
using System.Collections.ObjectModel;
using UnityEngine;
using Random = System.Random;

namespace Fishbot.Model.Fishing
{
    public class FishDatabase : MonoBehaviour
    {
        public ObservableCollection<FishRecord> FishRecords { get; private set; }

        private JsonFileLoader<ObservableCollection<FishRecord>> _fileLoader;

        private Random _random;

        private void Awake()
        {
            _random = new Random();
            _fileLoader = new JsonFileLoader<ObservableCollection<FishRecord>>("FishList");

            var loadedDatabase = _fileLoader.Read();
            FishRecords = loadedDatabase ?? new ObservableCollection<FishRecord>();

            FishRecords.CollectionChanged += (_, _) => { Recalculate(); };
            Recalculate();
        }

        private int _totalWeight;

        private void Recalculate()
        {
            _totalWeight = 0;
            foreach (var fishRecord in FishRecords)
            {
                _totalWeight += fishRecord.Probability;
            }

            _fileLoader.Write(FishRecords);
        }

        public string GetRandom()
        {
            var rnd = _random.Next(0, _totalWeight);
            foreach (var fishRecord in FishRecords)
            {
                if (rnd < fishRecord.Probability)
                {
                    return fishRecord.Message;
                }

                rnd -= fishRecord.Probability;
            }

            throw new Exception("Didn't get any random fish record");
        }

        public FishRecord this[int index]
        {
            get => FishRecords[index];
            set => FishRecords[index] = value;
        }

        public bool AddRecord(FishRecord record)
        {
            if (!CheckAvailability(record.Label))
            {
                return false;
            }

            FishRecords.Add(record);
            return true;
        }

        public bool CheckAvailability(string label)
        {
            foreach (var fishRecord in FishRecords)
            {
                if (fishRecord.Label == label)
                {
                    return false;
                }
            }

            return true;
        }

        public void RemoveRecord(int i) => FishRecords.RemoveAt(i);

        public void RemoveRecord(string label)
        {
            RemoveRecord(GetIndOf(label));
        }

        private int GetIndOf(string label)
        {
            for (var i = 0; i < FishRecords.Count; i++)
            {
                var fishRecord = FishRecords[i];
                if (fishRecord.Label == label)
                {
                    return i;
                }
            }

            throw new Exception($"No record with label: {label} exists.");
        }
    }
}