using UnityEngine;

namespace wpn
{
    class UnitTarget: GameObjectTarget
    {
        //TODO: hide it
        public UnitTarget(GameObject obj) : base(obj)
        {
            Debug.Assert(Obj.GetComponent<Damagable>() != null);
        }

        public override bool IsValid
        {
            get { return base.IsValid && Obj.GetComponent<Damagable>().IsAlive; }
        }
    }
}
