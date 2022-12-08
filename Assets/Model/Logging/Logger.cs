using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using UnityEngine;

namespace Fishbot.Model.Logging
{
    public class Logger : MonoBehaviour
    {
        private static Logger _instance;

        public ObservableCollection<string> Messages;

        private void Awake()
        {
            Messages = new ObservableCollection<string>();
            _instance = this;
        }

        public static void Message(string message)
        {
            if (message is null) return;

            var builder = new StringBuilder();
            builder.AppendLine($"{DateTime.Now.ToLongTimeString()}: ");
            builder.AppendLine(message);
            var msg = builder.ToString();
            Debug.Log(msg);
            _instance.Messages.Insert(0, msg);
        }

        public static void Clear()
        {
            _instance.Messages.Clear();
        }
    }
}