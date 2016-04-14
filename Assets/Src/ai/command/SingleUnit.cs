using System.Collections.Generic;
using ai.behavior.movement;
using ai.behavior.targeting;
using wpn;
using UnityEngine;

namespace ai.command
{
    public static class SingleUnit
    {
        public static void Attack(CommandExecutor executor, ITarget target)
        {
            executor.SetTargeting(new TargetGiven(executor.Shooter, target));
            executor.SetMovement(new KeepInRange(executor.NavMeshAgent, target, executor.Shooter.Range));
        }

        public static void GoTo(CommandExecutor executor, Vector3 dest)
        {
            executor.SetMovement(new behavior.movement.GoTo(executor.NavMeshAgent, dest));
        }

        public static void FollowPath(CommandExecutor executor, IList<Vector3> path, Vector3 offset)
        {
            executor.SetMovement(new behavior.movement.FollowPath(executor.NavMeshAgent, path, offset));
        }
    }
}
