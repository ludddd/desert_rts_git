using UnityEngine;

namespace ui.minimap
{
    class IconComponent: MonoBehaviour, IconCreator.IObjectWithIcon
    {
        [SerializeField]
        private GameObject proto = null;
        private GameObject iconObj;

        public void CreateIcon(RectTransform rectTransform, Camera minimapCamera)
        {
            Debug.Assert(proto.transform is RectTransform);
            Debug.Assert(iconObj == null);
            iconObj = Instantiate(proto);
            iconObj.transform.SetParent(rectTransform, false);
            iconObj.layer = rectTransform.gameObject.layer; //TODO: ba-a-a-d
            iconObj.SetActive(gameObject.activeInHierarchy);

            UpdatePos(minimapCamera);
        }

        public void UpdatePos(Camera minimapCamera)
        {
            var viewPos = minimapCamera.WorldToViewportPoint(transform.position);
            SetPosition((RectTransform)iconObj.transform, viewPos);
        }

        private static void SetPosition(RectTransform rectTransform, Vector3 viewPos)
        {
            rectTransform.anchorMin = rectTransform.anchorMax = viewPos;
        }

        private void OnEnable()
        {
            if (iconObj != null)
            {
                iconObj.SetActive(true);
            }
        }

        private void OnDisable()
        {
            if (iconObj != null)
            {
                iconObj.SetActive(false);
            }
        }
    }
}
