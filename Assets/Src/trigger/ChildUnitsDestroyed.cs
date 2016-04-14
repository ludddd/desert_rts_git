using unit;
using utils;
using UnityEngine;
using UnityEngine.Events;

namespace trigger
{

    class ChildUnitsDestroyed: MonoBehaviourEx
    {
        [SerializeField]
        private UnityEvent OnEvent;

        private void Update()
        {
            if (!HasChildUnits()) {
                Signal();
            }
        }

        private void Signal()
        {
            OnEvent.Invoke();
            enabled = false;
        }

        private bool HasChildUnits()
        {
            foreach (var child in transform.IterateBreadthFirst())
            {
                if (child.gameObject.IsUnit())
                {
                    return true;
                }
            }
            return false;
        }

        [editor.attr.Button]
        public void Test()
        {
            Signal();
        }
    }
}
