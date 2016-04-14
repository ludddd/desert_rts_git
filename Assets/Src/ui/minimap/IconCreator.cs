using System.Collections.Generic;
using utils;
using UnityEngine;

namespace ui.minimap
{
    public class IconCreator: MonoBehaviour
    {
        [SerializeField]
        private Camera cam;
        private IEnumerable<IObjectWithIcon> objects;

        public interface IObjectWithIcon
        {
            void CreateIcon(RectTransform rectTransform, Camera minimapCamera);
            void UpdatePos(Camera minimapCamera);
        }

        private void Start()
        {
            if (cam != null)
            {
                CreateIcons(cam);
            }
        }

        public void CreateIcons(Camera cam)
        {
            foreach (var obj in GetAllIcons())
            {
                obj.CreateIcon((RectTransform)transform, cam);
            }
        }

        public IEnumerable<IObjectWithIcon> GetAllIcons()
        {
            if (objects == null)
            {
                objects = GameObjectHelper.GetAllComponentsWithInterface<IObjectWithIcon>();
            }
            return objects;
        }

        private void Update()
        {
            if (cam == null) return;
            foreach (var obj in GetAllIcons())
            {
                obj.UpdatePos(cam);
            }
        }
    }
}
