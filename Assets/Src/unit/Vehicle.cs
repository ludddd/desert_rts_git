using UnityEngine;

namespace unit
{
    [CreateAssetMenu(fileName = "vehicle", menuName = "Create Vehicle Type")]
    class Vehicle: ScriptableObject
    {
        public float speed = 0;
        public float angularSpeed = 0;
        public float acceleration = 0;

        public void SetupAgent(NavMeshAgent agent)
        {
            agent.acceleration = acceleration;
            agent.angularSpeed = angularSpeed;
            agent.speed = speed;
        }
    }
}
