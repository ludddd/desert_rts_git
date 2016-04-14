using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ai.command
{
    static class GroupGoTo
    {
        public const float REMOVE_SEGMENTS_WITH_ANGLE_LESS_THAN_DEGREE = 5;

        public static void GoTo(IList<GameObject> objects, Vector3 dest)
        {
            if (!objects.Any()) {
                return;
            }
            var start = GetFormationCenter(objects);
            GotoFrom(objects, start, dest);
        }

        public static void GotoFrom(IList<GameObject> objects, Vector3 from, Vector3 to)
        {
            if (!objects.Any()) {
                return;
            }
            foreach (var obj in objects) {
                var cmdExec = obj.GetComponent<CommandExecutor>();
                if (!cmdExec) {
                    continue;
                }
                cmdExec.SetMovement(new behavior.movement.GoTo(cmdExec.NavMeshAgent, to + (obj.transform.position - from)));
            }
        }

        public static void FollowPath(IList<GameObject> objects, IList<Vector3> points)
        {
            if (!objects.Any()) {
                return;
            }
            var start = GetFormationCenter(objects);
            var straightPath = StraightenPathInXY(points, REMOVE_SEGMENTS_WITH_ANGLE_LESS_THAN_DEGREE);
            foreach (var obj in objects) {
                var cmdExec = obj.GetComponent<CommandExecutor>();
                if (!cmdExec) {
                    continue;
                }
                cmdExec.SetMovement(new behavior.movement.FollowPath(cmdExec.NavMeshAgent, straightPath, obj.transform.position - start));
            }
        }

        private static Vector3 GetFormationCenter(IList<GameObject> objects)
        {
            var start = new Vector3();
            foreach (var obj in objects) {
                start += obj.transform.position;
            }
            start /= objects.Count;
            return start;
        }

        private static IList<Vector3> StraightenPathInXY(IList<Vector3> path, float minAngleDegree)
        {
            IList<Vector3> rez = new List<Vector3>();
            rez.Add(path.First());
            for (int i = 1; i < path.Count - 1; i++) {
                var prevSeg = new Vector2(path[i].x - rez.Last().x, path[i].z - rez.Last().z);
                var nextSeg = new Vector2(path[i+1].x - path[i].x, path[i+1].z - path[i].z);
                if (Vector2.Angle(prevSeg, nextSeg) >= minAngleDegree) {
                    rez.Add(path[i]);
                }
            }
            rez.Add(path.Last());
            return rez;
        }
    }
}
