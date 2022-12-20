using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Fishbot.Model.VersionChecking.GithubReleaseData;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Fishbot.Model.VersionChecking
{
    public class VersionChecker : MonoBehaviour
    {
        private void Awake()
        {
            try
            {
                LoadVersion().Forget();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private const string URL = "https://api.github.com/repos/bustedbunny/FishbotUnity/releases/latest";

        public static async UniTask LoadVersion()
        {
            var request = new UnityWebRequest(URL, UnityWebRequest.kHttpVerbGET);
            request.downloadHandler = new DownloadHandlerBuffer();

            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", URL, request.error);
            }
            else
            {
                var text = request.downloadHandler.text;

                if (!text.StartsWith('['))
                {
                    text = $"[{text}]";
                }

                var data = JsonConvert.DeserializeObject<List<Root>>(text);

                var name = data.First().name;
                var curVersion = Application.version;

                Debug.Log("yes!");
            }

            request.Dispose();
        }
    }
}