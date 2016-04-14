using UnityEngine;

namespace trigger.render
{
    [RequireComponent(typeof(Collider))]
    class MinimapIcon : MonoBehaviour
    {
        [SerializeField] private GameObject proto = null;

        protected void Start()
        {
            var obj = Instantiate(proto);
            obj.transform.SetParent(gameObject.transform, false);
            obj.transform.localRotation = Quaternion.Euler(90, 0, 0);
            obj.transform.localScale = Size*Vector3.one;
        }

        private float Size
        {
            get { return 0.5f * GetComponent<Collider>().bounds.size.x; }
        }
    }
}
