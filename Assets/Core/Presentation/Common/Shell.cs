using UIToolkitMVP.ViewGeneration;
using UnityEngine;
using UnityEngine.UIElements;

namespace Fishbot.Presentation.Common
{
    public partial class Shell : MonoBehaviour
    {
        [GenerateView(typeof(StatusPage)), SerializeField]
        private VisualTreeAsset statusPageAsset;

        [GenerateView(typeof(FishListPage)), SerializeField]
        private VisualTreeAsset fishListPageAsset;

        [GenerateView(typeof(SettingsPage)), SerializeField]
        private VisualTreeAsset settingsPageAsset;

        private UIDocument _uiDocument;
        private VisualElement _content;

        public void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
        }

        private void Start()
        {
            InstantiateViews();

            var root = _uiDocument.rootVisualElement;
            _content = root.Q("Content");
            OpenPage(StatusPageView);

            root.Q<Button>("StatusBtn").RegisterCallback<MouseUpEvent>(_ => OpenPage(StatusPageView));
            root.Q<Button>("FishListBtn").RegisterCallback<MouseUpEvent>(_ => OpenPage(FishListPageView));
            root.Q<Button>("SettingsBtn").RegisterCallback<MouseUpEvent>(_ => OpenPage(SettingsPageView));
        }

        private void OpenPage(VisualElement page)
        {
            if (_content.Contains(page))
            {
                return;
            }

            _content.Clear();
            _content.Add(page);
            page.style.flexGrow = 1f;
        }
    }
}