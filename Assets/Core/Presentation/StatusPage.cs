using Cysharp.Threading.Tasks;
using Fishbot.Model.Twitch;
using UnityEngine.UIElements;
using Logger = Fishbot.Model.Logging.Logger;
using Object = UnityEngine.Object;

namespace Fishbot.Presentation
{
    public class StatusPage : Presenter
    {
        private bool _connected;
        private readonly Label _statusLabel;
        private readonly VisualElement _statusImage;
        private readonly Button _connectionButton;

        private bool Connected
        {
            get => _connected;
            set
            {
                if (_connected != value)
                {
                    _connected = value;
                    UpdateConnection();
                }
            }
        }

        public StatusPage(TemplateContainer view) : base(view)
        {
            var logger = Object.FindObjectOfType<Logger>();
            var client = Object.FindObjectOfType<ConnectionClient>();


            client.Client.OnConnected += (_, _) =>
            {
                Connected = true;
                Logger.Message("Connected.");
            };
            client.Client.OnDisconnected += (_, _) =>
            {
                Connected = false;
                Logger.Message("Disconnected.");
            };

            _connectionButton = view.Q<Button>("ConnectionBtn");

            _connectionButton.RegisterCallback<ClickEvent>(_ =>
            {
                if (!Connected)
                {
                    Connected = client.Connect();
                }
                else
                {
                    client.Disconnect();
                    Connected = false;
                }
            });


            _statusLabel = view.Q<Label>("StatusLabel");
            _statusImage = view.Q("StatusImage");

            UpdateConnection();


            var logList = view.Q<ListView>("LogList");
            var scroller = logList.Q<ScrollView>();
            scroller.verticalScrollerVisibility = ScrollerVisibility.Hidden;
            scroller.horizontalScrollerVisibility = ScrollerVisibility.Hidden;


            logList.itemsSource = logger.Messages;
            logger.Messages.CollectionChanged += (_, _) => { UpdateCollection().Forget(); };

            async UniTaskVoid UpdateCollection()
            {
                await UniTask.SwitchToMainThread();
                logList.RefreshItems();
            }
        }

        private void UpdateConnection()
        {
            var status = Connected ? "Connected" : "Disconnected";
            _statusLabel.text = $"Status: {status}";

            var buttonStatus = Connected ? "Disconnect" : "Connect";
            _connectionButton.text = buttonStatus;

            _statusImage.style.display =
                new StyleEnum<DisplayStyle>((Connected) ? DisplayStyle.Flex : DisplayStyle.None);
        }
    }
}