using System;
using System.Collections.Generic;
using Fishbot.Model.Fishing;
using Fishbot.Presentation.FishList;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = Fishbot.Model.Logging.Logger;
using Object = UnityEngine.Object;

namespace Fishbot.Presentation
{
    public class FishListPage : Presenter
    {
        private readonly FishDatabase _dataBase;
        private readonly ListView _list;
        private readonly TextField _newRecordField;
        private readonly VisualElement _previewContainer;
        private readonly TextField _previewLabel;
        private readonly IntegerField _previewProbability;
        private readonly TextField _previewMessage;
        private readonly Logger _logger;

        public FishListPage(TemplateContainer view) : base(view)
        {
            var template = Resources.Load<VisualTreeAsset>("FishListTemplate");

            _dataBase = Object.FindObjectOfType<FishDatabase>();

            _list = view.Q<ListView>();

            ListViewBinder.Bind(_list, template, _dataBase.FishRecords);


            _newRecordField = view.Q<TextField>("NewRecordField");
            var addRecordButton = view.Q<Button>("AddBtn");

            addRecordButton.RegisterCallback<ClickEvent>(AddRecord);

            _previewContainer = view.Q("PreviewContainer");
            _previewLabel = _previewContainer.Q<TextField>("PreviewLabel");
            _previewProbability = _previewContainer.Q<IntegerField>("PreviewProbability");
            _previewMessage = _previewContainer.Q<TextField>("PreviewMessage");
            var previewRemove = _previewContainer.Q<Button>("PreviewRemove");

            previewRemove.RegisterCallback<ClickEvent>(RemoveRecord);


            _previewProbability.RegisterValueChangedCallback(ChangeProbability);

            _previewMessage.RegisterValueChangedCallback(ChangeMessage);

            _previewContainer.visible = false;

            _list.selectionChanged += ChangeSelection;

            _logger = Object.FindObjectOfType<Logger>();
        }

        private void ChangeSelection(IEnumerable<object> _)
        {
            if (_list.selectedIndex < 0)
            {
                _previewContainer.visible = false;
                return;
            }

            var record = _dataBase.FishRecords[_list.selectedIndex];

            _previewLabel.value = record.Label;
            _previewProbability.value = record.Probability;
            _previewMessage.value = record.Message ?? string.Empty;

            _previewContainer.visible = true;
        }

        private void ChangeMessage(ChangeEvent<string> evt)
        {
            var curInd = _list.selectedIndex;

            var record = _dataBase[curInd];
            record.Message = evt.newValue;
            _dataBase[curInd] = record;

            _list.RefreshItem(curInd);
        }

        private void ChangeProbability(ChangeEvent<int> evt)
        {
            var curInd = _list.selectedIndex;

            if (evt.newValue is < 0 or > 100)
            {
                _previewProbability.value = Math.Clamp(evt.newValue, 0, 100);
                return;
            }

            var record = _dataBase[curInd];
            record.Probability = evt.newValue;
            _dataBase[curInd] = record;

            _list.RefreshItem(curInd);
        }

        private void RemoveRecord(ClickEvent _)
        {
            var curInd = _list.selectedIndex;

            var record = _dataBase[curInd];

            _dataBase.RemoveRecord(curInd);

            if (_list.itemsSource.Count > curInd)
            {
                _list.SetSelection(curInd);
            }
            else
            {
                _list.ClearSelection();
            }

            _list.RefreshItems();
            Logger.Message($"Removed record {record.Label}");
        }

        private void AddRecord(ClickEvent _)
        {
            if (_newRecordField.value == string.Empty)
            {
                return;
            }

            _dataBase.AddRecord(new FishRecord(_newRecordField.value));
            _list.RefreshItems();

            Logger.Message($"Added record {_newRecordField.value}");
        }
    }
}