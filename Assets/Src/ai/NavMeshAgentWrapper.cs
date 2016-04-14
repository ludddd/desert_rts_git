using UnityEngine;

namespace ai
{
    [RequireComponent(typeof(NavMeshAgent))]
    class NavMeshAgentWrapper : MonoBehaviour
    {
        public unit.Vehicle vehicleData = null;

        public void Start()
        {
            SetAgentData();
        }

        public void SetAgentData()
        {
            var agent = GetComponent<NavMeshAgent>();
            agent.speed = vehicleData.speed;
            agent.angularSpeed = vehicleData.angularSpeed;
            agent.acceleration = vehicleData.acceleration;
        }
    }
}
