using @select;
using UnityEngine;

namespace unit.test
{
    class TestAlwaysSelected: MonoBehaviour, ISelectable
    {
        private void Start()
        {
            Selection.instance.Add(this);
        }

        public void OnSelected()
        {
            //do nothing
        }

        public void OnUnselect()
        {
            Selection.instance.Add(this);
        }
    }
}
