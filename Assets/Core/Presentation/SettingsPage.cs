using Fishbot.Model.Credentials;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace Fishbot.Presentation
{
    public class SettingsPage : Presenter
    {
        private readonly TextField _loginEntry;
        private readonly TextField _tokenEntry;
        private readonly TextField _channelEntry;
        private readonly TwitchSettings _twitchSettings;

        public SettingsPage(TemplateContainer view) : base(view)
        {
            _loginEntry = view.Q<TextField>("LoginEntry");
            _tokenEntry = view.Q<TextField>("TokenEntry");
            _channelEntry = view.Q<TextField>("ChannelEntry");

            _twitchSettings = Object.FindObjectOfType<TwitchSettings>();


            _loginEntry.value = _twitchSettings.Settings.login;
            _tokenEntry.value = _twitchSettings.Settings.token;
            _channelEntry.value = _twitchSettings.Settings.channel;

            view.Q<Button>("SaveBtn").RegisterCallback<ClickEvent>(_ => Save());
        }

        private void Save()
        {
            var settings = new Settings
            {
                login = _loginEntry.value,
                token = _tokenEntry.value,
                channel = _channelEntry.value
            };
            _twitchSettings.Save(settings);
        }
    }
}