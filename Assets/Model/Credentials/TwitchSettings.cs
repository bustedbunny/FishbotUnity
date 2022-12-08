using UnityEngine;

namespace Fishbot.Model.Credentials
{
    public class TwitchSettings : MonoBehaviour
    {
        private JsonFileLoader<Settings> _fileLoader;

        public Settings Settings { get; private set; }

        private void Awake()
        {
            _fileLoader = new JsonFileLoader<Settings>("Credentials");

            var loaded = _fileLoader.Read();
            Settings = loaded.Equals(default) ? new Settings() : loaded;
        }

        public void Save(Settings newSettings)
        {
            if (string.IsNullOrEmpty(newSettings.channel) || string.IsNullOrEmpty(newSettings.login) ||
                string.IsNullOrEmpty(newSettings.token))
            {
                return;
            }

            Settings = newSettings;
            _fileLoader.Write(Settings);
        }
    }
}