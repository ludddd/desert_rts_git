using UnityEngine;

namespace ui
{
    class DisableObjectWhenActive : MonoBehaviour
    {
        [SerializeField]
        private GameObject target = null;

        public void OnDisable()
        {
            target.SetActive(true);
        }

        public void OnEnable()
        {
            target.SetActive(false);
        }
    }
}
