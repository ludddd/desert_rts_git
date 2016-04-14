using System.Collections.Generic;
using UnityEngine;

namespace ai.behavior.movement
{
    public class FollowPath: Base
    {
        private const float SHARP_ANGLE_DEGREES = 90;

        private readonly IList<Vector3> path;   //cannot use ienumerator cause it has no peek() method for autobraking
        private readonly Vector3 offset;
        private int pathIdx = -1;
        private NavMeshPath nextPath;

        public FollowPath(NavMeshAgent agent, IList<Vector3> path, Vector3 offset) : base(agent)
        {
            this.path = path;
            this.offset = offset;
            MoveNextPoint();
        }

        private void MoveNextPoint()
        {
            if (pathIdx + 1 >= path.Count) return;
            nextPath = null;
            Agent.destination = NextDestination;
            pathIdx++;            
            Agent.autoBraking = AutoBraking;
            Agent.Resume();
        }

        private Vector3 NextDestination
        {
            get
            {
                return path[pathIdx + 1] + offset;
            }
        }

        private bool AutoBraking
        {
            get
            {
                return !HasMorePointsInPath;
            }
        }

        private bool HasMorePointsInPath
        {
            get
            {
                return pathIdx < path.Count - 1;
            }
        }

        public override void Update()
        {
            if (ShouldSwitchToNextPath) {
                MoveNextPoint();
            }
        }

        public override bool IsFinished
        {
            get { return pathIdx >= path.Count && IsDestinationReached; }
        }

        private bool ShouldSwitchToNextPath
        {
            get
            {
                if (NextPath == null) return IsDestinationReached;
                var currentDirection = Agent.destination - Agent.transform.position;
                var nextDirection = nextPath.corners[1] - Agent.transform.position;
                var angleToCurrent = Vector3.Angle(currentDirection, Agent.transform.forward);
                var angleToNext = Vector3.Angle(nextDirection, Agent.transform.forward);
                if (angleToCurrent < SHARP_ANGLE_DEGREES || angleToNext > SHARP_ANGLE_DEGREES || angleToCurrent < angleToNext)
                    return IsDestinationReached;
                return true;
            }
        }

        private NavMeshPath NextPath
        {
            get
            {
                if (nextPath == null && HasMorePointsInPath)
                {
                    nextPath = new NavMeshPath();
                    if (!NavMesh.CalculatePath(Agent.destination, NextDestination, Agent.areaMask, nextPath))
                    {
                        nextPath = null;
                    }
                }
                return nextPath;
            }
        }
    }
}
