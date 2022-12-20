using Fishbot.Model.Fishing;
using UnityEngine.UIElements;

namespace Fishbot.Presentation.FishList
{
    public class FishRecordItem : VisualElement
    {
        private readonly Label _label;
        private readonly Label _probability;


        public void Update()
        {
            _label.text = Record.Label;
            _probability.text = Record.Probability.ToString("D3");
        }

        public FishRecord Record
        {
            get => _record;
            set
            {
                _record = value;
                Update();
            }
        }

        private FishRecord _record;

        public FishRecordItem()
        {
            style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);

            _label = new Label();

            var labelContainer = new VisualElement();
            labelContainer.style.flexGrow = 0.8f;
            labelContainer.Add(_label);
            Add(labelContainer);

            _probability = new Label();

            var probabilityContainer = new VisualElement();
            probabilityContainer.style.flexGrow = 0.2f;
            probabilityContainer.Add(_probability);
            Add(probabilityContainer);
        }


        public new class UxmlFactory : UxmlFactory<FishRecordItem, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
        }
    }
}