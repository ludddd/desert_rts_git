using UnityEngine;
using System.Collections.Generic;

namespace select
{
    public class Selection : MonoBehaviour
    {
        public delegate void ChangedSelectionHandler(IEnumerable<ISelectable> selected);

        public static Selection instance { get; private set; }

        private List<ISelectable> objects = new List<ISelectable>();
        public event ChangedSelectionHandler SelectionChanged = delegate { };

        void Awake()
        {
            instance = this;
        }

        public IEnumerable<ISelectable> Objects
        {
            get
            {
                return objects;
            }
        }

        public void Add(ISelectable obj)
        {
            if (!objects.Contains(obj)) {
                objects.Add(obj);
                obj.OnSelected();
                OnSelectionChanged();
            }
        }

        public void Remove(ISelectable obj)
        {
            if (objects.Contains(obj)) {
                obj.OnUnselect();
                objects.Remove(obj);
                OnSelectionChanged();
            }
        }

        public void Clear()
        {
            objects.ForEach(obj => obj.OnUnselect()); 
            objects.Clear();
            OnSelectionChanged();
        }

        public void AddMultiple(IEnumerable<ISelectable> items)
        {
            foreach(var item in items) {
                if (!objects.Contains(item)) {
                    objects.Add(item);
                    item.OnSelected();
                }
            }
            OnSelectionChanged();
        }

        private void OnSelectionChanged()
        {
            SelectionChanged(objects.ToArray());
        }
    }
}