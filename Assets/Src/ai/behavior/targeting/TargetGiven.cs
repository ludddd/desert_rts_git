using UnityEngine;
using wpn;

namespace ai.behavior.targeting
{
    public class TargetGiven: BaseWithShooter
    {
        public TargetGiven(IShooter shooter, ITarget target) : base(shooter)
        {
            Debug.Assert(target != null);
            shooter.Target = target;
        }

        public override bool IsFinished
        {
            get { return !shooter.Target.IsValid; }
        }
    }
}
