using UnityEngine;

namespace ai
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentMover : MonoBehaviour
    {
        private NavMeshAgent agent;
        private phys.align.GroundAligner aligner;
        [SerializeField]
        private unit.Vehicle vehicleData = null;

        public void Start()
        {
            agent = GetComponent<NavMeshAgent>();            
            agent.updatePosition = false;
            vehicleData.SetupAgent(agent);
            AlignWithTerrain();
        }

        public void AlignWithTerrain()
        {
            if (aligner == null) {
                aligner = new phys.align.AlignByThreeLowest(gameObject.transform, GetComponent<BoxCollider>());
            }
            aligner.Align(Terrain.activeTerrain);
        }

        public void Update()
        {
            transform.position = agent.nextPosition;
            AlignWithTerrain();
        }
    }
}
