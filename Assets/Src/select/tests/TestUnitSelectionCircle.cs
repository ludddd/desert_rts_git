using System.Linq;
using UnityEngine;

namespace select.tests
{
    class TestUnitSelectionCircle : MonoBehaviourEx
    {
        private void Start()
        {
            var units = GameObject.FindGameObjectsWithTag(Tags.Unit);
            var selectable = from unit in units select (ISelectable)unit.GetComponent<SelectableUnit>();
            Selection.instance.AddMultiple(selectable);
        }
    }
}
