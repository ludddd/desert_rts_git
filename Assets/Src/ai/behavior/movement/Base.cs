using UnityEngine;

namespace ai.behavior.movement
{
    public class Base
    {
        protected NavMeshAgent Agent;

        public Base(NavMeshAgent agent)
        {
            Debug.Assert(agent != null);
            Agent = agent;
            Agent.Stop();
        }

        public virtual void Update()
        {
            //do nothing
        }

        protected bool IsDestinationReached
        {
            get
            {
                return !Agent.hasPath || Agent.remainingDistance <= Agent.stoppingDistance;
            }
        }

        public virtual bool IsFinished
        {
            get { return false; }
        }
    }
}
