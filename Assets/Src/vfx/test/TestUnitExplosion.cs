using UnityEngine;

namespace vfx.test
{
    class TestUnitExplosion: MonoBehaviourEx
    {
        [SerializeField] private GameObject unit = null;

        [editor.attr.Button]
        public void DestroyUnit()
        {
            if (unit == null)
            {
                Debug.LogWarning("no unit to destroy");
                return;
            }
            var damagable = unit.GetComponent<Damagable>();
            if (damagable == null)
            {
                Debug.LogError("unit is missing damagable component");
                return;
            }
            damagable.Damage(damagable.MaxHealth);
        }
    }
}
