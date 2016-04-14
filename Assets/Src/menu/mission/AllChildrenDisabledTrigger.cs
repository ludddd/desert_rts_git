using UnityEngine;
using UnityEngine.Events;

namespace menu.mission
{
    class AllChildrenDisabledTrigger: MonoBehaviourEx
    {
        [SerializeField]
        private UnityEvent triggerEvent;

        private void Update()
        {
            if (!HasActiveChild()) {
                Signal();
            }
        }

        private void Signal()
        {
            triggerEvent.Invoke();
            enabled = false;
        }

        private bool HasActiveChild()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i).gameObject;
                if (child.activeSelf) return true;
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
