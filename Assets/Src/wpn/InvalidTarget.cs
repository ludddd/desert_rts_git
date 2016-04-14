using UnityEngine;

namespace wpn
{
    class InvalidTarget: ITarget
    {
        public static readonly ITarget Invalid = new InvalidTarget();

        private InvalidTarget()
        {
        }

        public Vector3 Pos {
            get { Debug.Assert(false, "Should not be called. Check IsValid before"); return Vector3.zero;}
        }
        public bool IsValid { get { return false; } }
        public bool Equals(ITarget other)
        {
            return other == Invalid;
        }
    }
}
