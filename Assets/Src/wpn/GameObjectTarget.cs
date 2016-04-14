using UnityEngine;

namespace wpn
{
    public class GameObjectTarget : ITarget
    {
        protected readonly GameObject Obj;
        private Vector3 lastPos;    //TODO: remove this. Last pos object should be used only for classes that needs it... (Missile)

        protected GameObjectTarget(GameObject obj)
        {
            Obj = obj;
        }

        public static ITarget FromGameObject(GameObject obj)
        {
            Debug.Assert(obj != null);
            var target = obj.GetComponent<Damagable>() ? new UnitTarget(obj) : new GameObjectTarget(obj);
            target.UpdateLastPos();
            return target;
        }

        public Vector3 Pos
        {
            get
            {
                //naive but fast implementation. To correctly keep track of gameobject position should add as monobehavoiur
                if (IsValid) {
                    UpdateLastPos();
                }
                return lastPos;
            }
        }

        public virtual bool IsValid {
            get { return Obj != null; }
        }


        private void UpdateLastPos()
        {
            var bbox = Obj.GetComponent<BoxCollider>();
            lastPos = bbox != null ? Obj.transform.TransformPoint(bbox.center) : Obj.transform.position;
        }

        public GameObject GameObject
        {
            get { return Obj; }
        }

        public static GameObject TargetToGameObject(ITarget target)
        {
            var gameObjTarget = target as GameObjectTarget;
            return gameObjTarget != null ? gameObjTarget.GameObject : null;
        }

        protected bool Equals(GameObjectTarget other)
        {
            return Equals(Obj, other.Obj);
        }

        public bool Equals(ITarget other)
        {
            return Equals((object) other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals(Obj, ((GameObjectTarget)obj).Obj);
        }

        public override int GetHashCode()
        {
            return (Obj != null ? Obj.GetHashCode() : 0);
        }
    }
}
