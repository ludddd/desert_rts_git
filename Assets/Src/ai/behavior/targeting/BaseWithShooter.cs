using UnityEngine;
using wpn;

namespace ai.behavior.targeting
{
    public class BaseWithShooter: Base
    {
        protected IShooter shooter;

        public BaseWithShooter(IShooter shooter)
        {
            Debug.Assert(shooter != null);
            this.shooter = shooter;
            this.shooter.Target = InvalidTarget.Invalid;
        }
    }
}
