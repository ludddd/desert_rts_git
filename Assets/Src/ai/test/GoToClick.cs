using UnityEngine;
using UnityEngine.EventSystems;

namespace ai.test
{
    class GoToClick: MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private NavMeshAgent agent;

        public void OnPointerClick(PointerEventData eventData)
        {
            agent.destination = eventData.pointerCurrentRaycast.worldPosition;
            agent.Resume();
        }
    }
}
