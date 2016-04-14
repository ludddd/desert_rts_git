using UnityEngine;

namespace utils
{
    public static class TransformUtil
    {
        public static bool IsAncestorOf(this Transform self, Transform transform)
        {
            for (; transform != null; transform = transform.parent)
            {
                if (transform.IsChildOf(self)) return true;
            }
            return false;
        }

        public static void DestroyImmediateAllChildren(this Transform self)
        {
            for (int i = self.childCount - 1; i >= 0; i--)
            {
                var obj = self.GetChild(i).gameObject;
                Object.DestroyImmediate(obj);
            }
        }
    }
}
