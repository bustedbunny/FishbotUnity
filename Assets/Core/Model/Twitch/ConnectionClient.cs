using System;
using Fishbot.Model.Credentials;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using UnityEngine;
using Logger = Fishbot.Model.Logging.Logger;

namespace Fishbot.Model.Twitch
{
    public class ConnectionClient : MonoBehaviour
    {
        public TwitchClient Client { get; private set; }

        private TwitchSettings _twitchSettings;

        private void Awake()
        {
            _twitchSettings = FindObjectOfType<TwitchSettings>();
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            var customClient = new WebSocketClient(clientOptions);
            Client = new TwitchClient(customClient);
        }

        public bool Connect()
        {
            var settings = _twitchSettings.Settings;
            Client.Initialize(new ConnectionCredentials(settings.login, settings.token), settings.channel);

            if (!Client.IsInitialized)
            {
                Logger.Message("Failed to connect. Twitch client is not initialized properly.");
                return false;
            }

            return Client.Connect();
        }

        public void Disconnect()
        {
            Client.Disconnect();
        }
    }
}