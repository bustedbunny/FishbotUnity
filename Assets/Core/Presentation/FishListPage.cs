using System;
using System.Collections.Generic;
using System.Linq;
using Fishbot.Model.Fishing;
using Fishbot.Model.Fishing.Database;
using Fishbot.Presentation.FishList;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = Fishbot.Model.Logging.Logger;
using Object = UnityEngine.Object;

namespace Fishbot.Presentation
{
    public class FishListPage : Presenter
    {
        private readonly RecordsAccessor _records;

        private readonly ListView _list;
        private readonly TextField _newRecordField;
        private readonly VisualElement _previewContainer;
        private readonly TextField _previewLabel;
        private readonly IntegerField _previewProbability;
        private readonly TextField _previewMessage;


        private void UpdateList()
        {
            _list.itemsSource = _records.Query.ToList();
        }

        public FishListPage(TemplateContainer view) : base(view)
        {
            _records = Object.FindObjectOfType<RecordsAccessor>();

            _list = view.Q<ListView>();


            _list.makeItem = () => new FishRecordItem();
            _list.bindItem = (element, i) =>
            {
                var item = (FishRecordItem)element;
                var record = (FishRecord)_list.itemsSource[i];
                item.Record = record;
            };

            UpdateList();


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

            Object.FindObjectOfType<Logger>();
        }

        private void ChangeSelection(IEnumerable<object> _)
        {
            if (_list.selectedIndex < 0)
            {
                _previewContainer.visible = false;
                return;
            }

            var record = (FishRecord)_list.itemsSource[_list.selectedIndex];

            _previewLabel.value = record.Label;
            _previewProbability.value = record.Probability;
            _previewMessage.value = record.Message ?? string.Empty;

            _previewContainer.visible = true;
        }

        private void ChangeMessage(ChangeEvent<string> evt)
        {
            var record = (FishRecord)_list.selectedItem;
            var shouldUpdate = record.Message != evt.newValue;

            if (shouldUpdate)
            {
                record.Message = evt.newValue;
                _records.UpdateItem(record);
            }
        }

        private void ChangeProbability(ChangeEvent<int> evt)
        {
            if (evt.newValue is < 0 or > 100)
            {
                _previewProbability.value = Math.Clamp(evt.newValue, 0, 100);
                return;
            }

            var record = (FishRecord)_list.selectedItem;
            var shouldUpdate = record.Probability != evt.newValue;

            if (shouldUpdate)
            {
                record.Probability = evt.newValue;
                _records.UpdateItem(record);
            }
        }

        private void RemoveRecord(ClickEvent _)
        {
            var curInd = _list.selectedIndex;

            var record = (FishRecord)_list.selectedItem;

            _list.itemsSource.RemoveAt(curInd);
            _records.Remove(record.ID);


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

            var record = new FishRecord(_newRecordField.value);
            _list.itemsSource.Add(record);
            _records.Add(record);

            _list.RefreshItems();

            Logger.Message($"Added record {_newRecordField.value}");
        }
    }
}