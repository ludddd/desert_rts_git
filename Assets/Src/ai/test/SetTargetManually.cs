using utils;
using UnityEngine;
using wpn;

namespace ai.test
{
    class SetTargetManually: MonoBehaviour
    {
        [SerializeField]
        private GameObject unit = null;
        [SerializeField]
        private GameObject target = null;

        private void Start()
        {
            if (unit == null || target == null) return;
            var shooter = unit.GetComponentWithInterface<IShooter>();
            if (shooter == null) return;
            shooter.Target = GameObjectTarget.FromGameObject(target);
        }
    }
}
