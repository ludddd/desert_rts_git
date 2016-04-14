using faction;
using fow;
using utils;
using UnityEngine;

namespace ai
{
    public class CommandExecutor : MonoBehaviour
    {
        private behavior.targeting.Base targetingBehavior;
        private behavior.movement.Base movementBehavior;

        private void Start()
        {
            SetTargeting(GetDefaultTargeting());
            SetMovement(GetDefaultMovement());
        }

        // Update is called once per frame
        private void Update()
        {
            targetingBehavior.Update();
            if (targetingBehavior.IsFinished)
            {
                SetTargeting(GetDefaultTargeting());
            }
            movementBehavior.Update();
            if (movementBehavior.IsFinished)
            {
                SetMovement(GetDefaultMovement());
            }
        }

        public void SetTargeting(behavior.targeting.Base targeting)
        {
            Debug.Assert(targeting != null);
            targetingBehavior = targeting;
        }

        public void SetMovement(behavior.movement.Base movement)
        {
            Debug.Assert(movement != null);
            movementBehavior = movement;
        }

        public string GetCommandName()
        {
            var targetingName = targetingBehavior == null ? "none" : targetingBehavior.GetType().Name;
            var movementName = movementBehavior == null ? "none" : movementBehavior.GetType().Name;
            return string.Format("targeting: {0}, movement; {1}", targetingName, movementName);
        }

        private behavior.targeting.Base GetDefaultTargeting()
        {
            return Shooter != null
                ? new behavior.targeting.TargetNearest(Shooter, FactionId)
                : new behavior.targeting.Base();
        }

        public IShooter Shooter
        {
            get { return gameObject.GetComponentWithInterface<IShooter>(); }
        }

        public FactionData.FactionId FactionId
        {
            get { return Faction.GetFactionId(gameObject); }
        }

        private behavior.movement.Base GetDefaultMovement()
        {
            return FactionId.IsPlayerFaction() || Shooter == null
                ? new behavior.movement.Base(NavMeshAgent)
                : new behavior.movement.PursueEnemies(NavMeshAgent, Shooter, VisibilityRange, FactionId);
        }

        public NavMeshAgent NavMeshAgent
        {
            get
            {
                return gameObject.GetComponent<NavMeshAgent>();
            }
        }

        public float VisibilityRange
        {
            get
            {
                var vizor = gameObject.GetComponent<FOWVisor>();
                return vizor != null ? vizor.Range : float.PositiveInfinity;
            }
        }
    }

}
