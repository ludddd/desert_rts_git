using UnityEngine;
using System.Collections;

namespace ai.dbg
{

    [RequireComponent(typeof(NavMeshAgent))]
    public class PathRenderer : MonoBehaviour
    {

        void OnDrawGizmosSelected()
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            if (!agent.hasPath) {
                return;
            }
            Gizmos.color = Color.blue;
            for (int i = 1; i < agent.path.corners.Length; i++) {
                Gizmos.DrawLine(agent.path.corners[i - 1], agent.path.corners[i]);
            }
        }
    }


}