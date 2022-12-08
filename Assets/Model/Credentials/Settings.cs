using System;
using UnityEngine.Serialization;

namespace Fishbot.Model.Credentials
{
    [Serializable]
    public struct Settings
    {
        public string login;
        public string token;
        public string channel;
    }
}