using UnityEngine;

namespace wpn
{
    class StaticTarget : ITarget
    {
        private readonly Vector3 pos;

        private StaticTarget(Vector3 pos)
        {
            this.pos = pos;
        }

        public Vector3 Pos
        {
            get { return pos; }
        }

        public bool IsValid
        {
            get { return true; }
        }

        public static ITarget FromITarget(ITarget target)
        {
            return new StaticTarget(target.Pos);
        }

        public static ITarget FromPos(Vector3 pos)
        {
            return new StaticTarget(pos);
        }

        public bool Equals(ITarget other)
        {
            return this == other;   //no idea how to compare static targets...
        }
    }
}
