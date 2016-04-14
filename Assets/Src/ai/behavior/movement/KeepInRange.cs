using UnityEngine;
using wpn;

namespace ai.behavior.movement
{
    public class KeepInRange: Base
    {
        private const float PATH_UPDATE_RANGE = 1.0f;

        private readonly ITarget target;
        private readonly float range;

        public KeepInRange(NavMeshAgent agent, ITarget target, float range) : base(agent)
        {
            this.target = target;
            this.range = range;
            if (target.IsValid)
            {
                UpdateDestination();
                Agent.Resume();
            }
        }

        public override void Update()
        {
            if (!target.IsValid) {
                return;
            }
            if (IsAgentMoving) {
                if (Vector3.Distance(target.Pos, Agent.destination) >= PATH_UPDATE_RANGE) {
                    UpdateDestination();
                }
            } else {
                if (Vector3.Distance(target.Pos, Agent.transform.position) > StoppingDistance) {
                    UpdateDestination();
                }
            }
        }

        private bool IsAgentMoving
        {
            get
            {
                return Agent.remainingDistance > 0;
            }
        }

        private float StoppingDistance
        {
            get
            {
                return range - PATH_UPDATE_RANGE;
            }
        }

        private void UpdateDestination()
        {
            Agent.destination = target.Pos;
            Agent.stoppingDistance = StoppingDistance;
        }

        public override bool IsFinished
        {
            get { return !target.IsValid; }
        }

        public ITarget Target
        {
            get { return target; }
        }
    }
}
