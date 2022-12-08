using System;
using Fishbot.Model.Twitch;
using TwitchLib.Client.Events;
using UnityEngine;
using Logger = Fishbot.Model.Logging.Logger;
using Object = UnityEngine.Object;

namespace Fishbot.Model.Fishing
{
    public class FishingController : MonoBehaviour
    {
        private FishDatabase _fishDatabase;
        private ConnectionClient _twitch;

        private void Awake()
        {
            _fishDatabase = FindObjectOfType<FishDatabase>();
            _twitch = FindObjectOfType<ConnectionClient>();
        }

        private void Start()
        {
            _twitch.Client.OnMessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message.Trim() == "!fish")
            {
                var message = _fishDatabase.GetRandom();

                message = message.Replace("{user}", "@" + e.ChatMessage.DisplayName);

                Logger.Message(message);

                _twitch.Client.SendMessage(e.ChatMessage.Channel, message);
            }
        }
    }
}