using UnityEngine;

namespace ai.behavior.movement
{
    public class GoTo: Base
    {
        public GoTo(NavMeshAgent agent, Vector3 dest) : base(agent)
        {
            agent.destination = dest;
            agent.Resume();
        }

        public override bool IsFinished
        {
            get { return IsDestinationReached; }
        }        
    }
}
