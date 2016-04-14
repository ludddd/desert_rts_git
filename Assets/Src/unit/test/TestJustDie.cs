using System.Collections;
using utils;
using UnityEngine;

namespace unit.test
{
    class TestJustDie: MonoBehaviour
    {
        [SerializeField]
        private GameObject unit;

        private void Start()
        {
            StartCoroutine(KillLater());
        }

        private void Kill()
        {
            var damagable = unit.GetComponentWithInterface<Damagable>();
            damagable.Damage(damagable.MaxHealth);
        }

        private IEnumerator KillLater()
        {
            yield return new WaitForEndOfFrame();
            Kill();
        }

    }
}
