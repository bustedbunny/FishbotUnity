using System;
using System.Collections.Generic;
using Fishbot.Model.Fishing;
using Fishbot.Model.Fishing.Database;
using UnityEngine.UIElements;
using Logger = Fishbot.Model.Logging.Logger;
using Object = UnityEngine.Object;

namespace Fishbot.Presentation
{
    public class FishListPage : Presenter
    {
        private readonly RecordsAccessor _records;

        private readonly MultiColumnListView _treeView;
        private readonly TextField _newRecordField;
        private readonly VisualElement _previewContainer;
        private readonly TextField _previewLabel;
        private readonly IntegerField _previewProbability;
        private readonly TextField _previewMessage;


        private void UpdateList()
        {
            var data = _records.Query.ToList();
            _treeView.itemsSource = data;
        }

        public FishListPage(TemplateContainer view) : base(view)
        {
            _records = Object.FindObjectOfType<RecordsAccessor>();

            _treeView = view.Q<MultiColumnListView>();


            var labelColumn = _treeView.columns["label"];
            var probColumn = _treeView.columns["probability"];
            

            labelColumn.makeCell = static () =>
            {
                var label = new Label();
                label.style.whiteSpace = new StyleEnum<WhiteSpace>(WhiteSpace.Normal);
                return label;
            };
            probColumn.makeCell = static () => new Label();

            labelColumn.bindCell = (element, i) =>
            {
                var label = (Label)element;
                var record = (FishRecord)_treeView.itemsSource[i];
                label.text = record.Label;
            };

            probColumn.bindCell = (element, i) =>
            {
                var label = (Label)element;
                var record = (FishRecord)_treeView.itemsSource[i];
                label.text = record.Probability.ToString();
            };

            _treeView.columnSortingChanged += () =>
            {
                var sort = _treeView.sortedColumns;

                foreach (var columnDescription in sort)
                {
                    var direction = columnDescription.direction;
                    var list = (List<FishRecord>)_treeView.itemsSource;
                    if (columnDescription.column == labelColumn)
                    {
                        if (direction == SortDirection.Ascending)
                        {
                            list.Sort((x, y) => string.CompareOrdinal(x.Label, y.Label));
                        }
                        else
                        {
                            list.Sort((x, y) => string.CompareOrdinal(y.Label, x.Label));
                        }
                    }
                    else if (columnDescription.column == probColumn)
                    {
                        if (direction == SortDirection.Ascending)
                        {
                            list.Sort((x, y) => x.Probability - y.Probability);
                        }
                        else
                        {
                            list.Sort((x, y) => y.Probability - x.Probability);
                        }
                    }
                }

                _treeView.RefreshItems();
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

            _treeView.selectionChanged += ChangeSelection;

            Object.FindObjectOfType<Logger>();
        }


        private void ChangeSelection(IEnumerable<object> _)
        {
            var ind = _treeView.selectedIndex;
            if (ind < 0)
            {
                _previewContainer.visible = false;
                return;
            }

            var record = (FishRecord)_treeView.itemsSource[ind];

            _previewLabel.value = record.Label;
            _previewProbability.value = record.Probability;
            _previewMessage.value = record.Message ?? string.Empty;

            _previewContainer.visible = true;
        }

        private void ChangeMessage(ChangeEvent<string> evt)
        {
            var record = (FishRecord)_treeView.selectedItem;
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

            var record = (FishRecord)_treeView.selectedItem;
            var shouldUpdate = record.Probability != evt.newValue;

            if (shouldUpdate)
            {
                record.Probability = evt.newValue;
                _records.UpdateItem(record);
            }
        }

        private void RemoveRecord(ClickEvent _)
        {
            var curInd = _treeView.selectedIndex;

            var record = (FishRecord)_treeView.selectedItem;

            _treeView.itemsSource.RemoveAt(curInd);
            _records.Remove(record.ID);


            if (_treeView.itemsSource.Count > curInd)
            {
                _treeView.SetSelection(curInd);
            }
            else
            {
                _treeView.ClearSelection();
            }

            _treeView.RefreshItems();
            Logger.Message($"Removed record {record.Label}");
        }

        private void AddRecord(ClickEvent _)
        {
            if (_newRecordField.value == string.Empty)
            {
                return;
            }

            var record = new FishRecord(_newRecordField.value);
            _treeView.itemsSource.Add(record);
            _records.Add(record);

            _treeView.RefreshItems();

            Logger.Message($"Added record {_newRecordField.value}");
        }
    }
}