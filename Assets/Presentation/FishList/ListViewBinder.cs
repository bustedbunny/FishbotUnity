using System;
using System.Collections;
using System.Collections.Generic;
using Fishbot.Model.Fishing;
using UnityEngine.UIElements;

namespace Fishbot.Presentation.FishList
{
    public static class ListViewBinder
    {
        public static void Bind(ListView view, VisualTreeAsset template, IList<FishRecord> itemSource)
        {
            view.makeItem = template.Instantiate;
            view.bindItem = (element, i) =>
            {
                var label = element.Q<Label>("RecordName");
                var probability = element.Q<Label>("Probability");

                var record = itemSource[i];
                label.text = record.Label;
                probability.text = record.Probability.ToString("D3");
            };
            view.itemsSource = itemSource as IList;
        }
    }
}