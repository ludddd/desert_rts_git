using ai.target;
using faction;
using UnityEngine;
using wpn;

namespace ai.behavior.movement
{
    public class PursueEnemies: Base
    {
        private readonly IShooter shooter;
        private readonly float visibilityRange;
        private readonly FactionData.FactionId myFaction;
        private ITarget pursueTarget = InvalidTarget.Invalid;
        private KeepInRange keepInRange;

        public PursueEnemies(NavMeshAgent agent, IShooter shooter, float visibilityRange, FactionData.FactionId myFaction) : base(agent)
        {
            this.shooter = shooter;
            this.visibilityRange = visibilityRange;
            this.myFaction = myFaction;
            keepInRange = new KeepInRange(Agent, InvalidTarget.Invalid, shooter.Range);
        }

        public override void Update()
        {
            UpdateTarget(GetTarget());
            keepInRange.Update();
        }

        private void UpdateTarget(ITarget target)
        {
            if (!keepInRange.Target.Equals(target)) {
                keepInRange = new KeepInRange(Agent, target, shooter.Range);
            }
        }

        private ITarget GetTarget()
        {
            if (shooter.Target != null && shooter.Target.IsValid)
            {
                return shooter.Target;
            }
            if (!pursueTarget.IsValid) {
                pursueTarget = FindNearest.Get(Agent.transform.position, myFaction, visibilityRange);
            }
            return pursueTarget;
        }
    }
}
