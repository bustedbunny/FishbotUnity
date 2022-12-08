using Fishbot.Presentation;
using UnityEngine;
using UnityEngine.UIElements;
using Shell = Fishbot.Presentation.Common.Shell;

namespace Fishbot
{
    public abstract class Presenter
    {
        protected readonly Shell Shell;

        protected Presenter(TemplateContainer view)
        {
            Shell = Object.FindObjectOfType<Shell>();
        }
    }
}