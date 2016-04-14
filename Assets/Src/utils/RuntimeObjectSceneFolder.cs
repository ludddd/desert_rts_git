using UnityEngine;

namespace utils
{
    class RuntimeObjectSceneFolder
    {
        private static RuntimeObjectSceneFolder instance;
        public static RuntimeObjectSceneFolder Instance
        {
            get
            {
                if (instance == null) {
                    instance = new RuntimeObjectSceneFolder();
                }
                return instance;
            }
        }

        private GameObject root;

        private RuntimeObjectSceneFolder()
        {
        }

        private void OnDestroy()
        {
            root = null;
        }

        private GameObject Root
        {
            get
            {
                if (root == null)   //TODO: is there a way to avoid double lazy initialization of instance and root?
                {
                    root = new GameObject("RuntimeObjects");
                }
                return root;
            }
        }

        public Transform Get(string category)
        {
            if (category.Equals("")) {
                return Root.transform;
            }
            Transform childTransform = Root.transform.FindChild(category);
            if (childTransform) {
                return childTransform;
            }
            var newChild = new GameObject(category);
            newChild.transform.SetParent(Root.transform);
            return newChild.transform;
        }
    }
}
