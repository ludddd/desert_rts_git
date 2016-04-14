using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ai.test
{
    [RequireComponent(typeof(NavMeshAgent))]
    class DrawPath: MonoBehaviour
    {
        [SerializeField]
        private float step = 0.1f;
        [SerializeField]
        private Color desiredPath = Color.green;
        [SerializeField]
        private Color plannedPath = Color.red;
        [SerializeField]
        private Color trajectory = Color.blue;
        private IList<Vector3> points = new List<Vector3>();
        private List<Vector3> path = new List<Vector3>();
        private IList<Vector3> destinations = new List<Vector3>();

        private void Start()
        {
            points.Add(transform.position);
            path.Add(transform.position);
            destinations.Add(transform.position);
        }

        private void Update()
        {
            if (Vector3.Distance(points.Last(), transform.position) >= step)
            {
                points.Add(transform.position);
            }
            var agent = GetComponent<NavMeshAgent>();
            if (agent.hasPath && Vector3.Distance(agent.destination, path.Last()) >= step)
            {
                path.AddRange(agent.path.corners);
            }
            if (agent.hasPath && Vector3.Distance(agent.destination, destinations.Last()) >= step)
            {
                destinations.Add(agent.destination);
            }
        }

        private void OnDrawGizmosSelected()
        {
            DrawPoints(points, trajectory);
            DrawPoints(path, plannedPath);
            DrawPoints(destinations, desiredPath);
        }

        private static void DrawPoints(IList<Vector3> points, Color color)
        {
            Gizmos.color = color;
            for (int i = 0; i < points.Count - 1; i++) {
                Gizmos.DrawLine(points[i], points[i + 1]);
            }
        }
    }
}
