using Fishbot.Model.Fishing.Database;
using Fishbot.Model.Twitch;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using UnityEngine;
using Logger = Fishbot.Model.Logging.Logger;

namespace Fishbot.Model.Fishing
{
    public class FishingController : MonoBehaviour
    {
        private RandomRecordAccessor _randomAccessor;
        private CountAccessor _countAccessor;
        private ConnectionClient _twitch;


        private void Awake()
        {
            _randomAccessor = FindObjectOfType<RandomRecordAccessor>();
            _countAccessor = FindObjectOfType<CountAccessor>();
            _twitch = FindObjectOfType<ConnectionClient>();
        }

        private void Start()
        {
            _twitch.Client.OnMessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            var chatMsg = e.ChatMessage.Message.Trim();
            if (chatMsg == "!fish")
            {
                var record = _randomAccessor.GetRandom();

                var message = record.Message.Replace("{user}", "@" + e.ChatMessage.DisplayName);

                Logger.Message(message);

                _twitch.Client.SendMessage(e.ChatMessage.Channel, message);

                _countAccessor.AddCount(record.ID, e.ChatMessage.UserId);
            }

            // else if (chatMsg == "!count")
            // {
            //     var countMessage = _countAccessor.DisplayCountForUser(e.ChatMessage.UserId);
            //
            //     Logger.Message(countMessage);
            //     _twitch.Client.SendMessage(e.ChatMessage.Channel, countMessage);
            // }
        }
    }
}