using UnityEngine;

namespace select
{
    public class SelectableUnit : MonoBehaviour, ISelectable
    {
        private const float CIRCLE_SIZE_MULTIPLIER = 1.4f;  //depends on size of circle image in texture
        private static GameObject prefab;
        private GameObject selectionCircle;

        private GameObject SelectionCircle
        {
            get
            {
                if (selectionCircle == null)
                {
                    CreateSelectionCircle();
                }
                return selectionCircle;
            }
        }

        private void CreateSelectionCircle()
        {
            selectionCircle = Instantiate(Prefab);
            selectionCircle.transform.SetParent(gameObject.transform, false);
            selectionCircle.SetActive(false);
            SetupCircleSize();
        }

        private static GameObject Prefab
        {
            get
            {
                if (prefab == null)
                {
                    prefab = Resources.Load("UnitSelection") as GameObject;
                }
                return prefab;
            }
        }

        private void SetupCircleSize()
        {
            var bbox = GetComponent<BoxCollider>();
            if (bbox == null) return;
            float diameter = new Vector3(bbox.size.x, 0, bbox.size.z).magnitude;    //won't work with unit scaling...
            var projector = selectionCircle.GetComponent<Projector>();
            projector.orthographicSize = 0.5f * diameter * CIRCLE_SIZE_MULTIPLIER;
        }

        public void OnSelected()
        {
            if (!enabled) return;
            SelectionCircle.SetActive(true);
        }

        public void OnUnselect()
        {
            SelectionCircle.SetActive(false);
        }

        public void OnDestroy()
        {
            ClearFromSelection();
        }

        private void ClearFromSelection()
        {
            //TODO: this seems not to clear icon list
            if (Selection.instance)
            {
                Selection.instance.Remove(this);
            }
        }
    }
}