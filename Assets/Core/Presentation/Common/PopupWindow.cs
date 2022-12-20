using System;
using UnityEngine.UIElements;

namespace Fishbot.Presentation.Common
{
    public class PopupWindow
    {
        private readonly VisualTreeAsset _asset;
        private readonly UIDocument _uiDocument;

        private readonly TemplateContainer _popup;

        public PopupWindow(VisualTreeAsset windowAsset, UIDocument uiDocument)
        {
            _uiDocument = uiDocument;
            _asset = windowAsset;


            _popup = _asset.Instantiate();
            _popup.style.position = new StyleEnum<Position>(Position.Absolute);
           
        }

        // public void Pop(string message, params ButtonInstruction[] buttons)
        // {
        //     if (buttons.Length > 3)
        //     {
        //         throw new Exception("Can't have more than 3 buttons.");
        //     }
        // }
    }

    // public struct ButtonInstruction
    // {
    //     public string text;
    //     public
    // }
}