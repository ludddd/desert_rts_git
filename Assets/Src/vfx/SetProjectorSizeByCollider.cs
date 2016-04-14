using UnityEngine;

namespace vfx
{
    [RequireComponent(typeof(Projector))]
    class SetProjectorSizeByCollider : MonoBehaviour
    {
        [SerializeField] private Collider source;
        public void Start()
        {
            if (source != null)
            {
                var projector = GetComponent<Projector>();
                projector.orthographicSize = source.bounds.extents.x;
            }
        }
    }
}
