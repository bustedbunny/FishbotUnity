using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using LiteDB;
using TwitchLib.Api.Core.Models.Undocumented.ChannelPanels;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fishbot.Model.Fishing.Database
{
    public class FishDatabaseLiteDB : MonoBehaviour
    {
        [SerializeField] private string dbFilename = @"FishDatabase.db";
        public LiteDatabase Database { get; private set; }


        private void Awake()
        {
            var dbPath = Path.Combine(Application.persistentDataPath, dbFilename);

            Database = new LiteDatabase(new ConnectionString
            {
                Filename = dbPath
            });
        }

        private void OnDestroy()
        {
            Database.Dispose();
        }
    }
}