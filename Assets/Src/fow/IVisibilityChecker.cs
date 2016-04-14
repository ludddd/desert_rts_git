using UnityEngine;

namespace fow
{
    interface IVisibilityChecker
    {
        bool IsVisible(Vector3 pos);
    }
}
