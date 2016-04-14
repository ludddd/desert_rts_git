using faction;
using UnityEngine;

namespace wpn
{
    public class MissileAmmo : MonoBehaviour, IAmmo
    {
        public float rotateToTargetSpeed;
        public float targetDetectionAngle;
        public float timeToWorkAfterTargetLost;

        private Weapon weapon;
        private FactionData.FactionId factionId;
        private ITarget target = InvalidTarget.Invalid;
        private float timeLeft;

        public void Awake()
        {
            enabled = false;
        }

        public void Setup(Vector3 from, Vector3 dir, Weapon weapon, FactionData.FactionId factionId, ITarget target)
        {
            gameObject.transform.position = from;
            gameObject.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
            this.weapon = weapon;
            this.factionId = factionId;
            Debug.Assert(target != null);
            this.target = target;
            timeLeft = weapon.shootDist / weapon.ammoSpeed;
            enabled = true;
        }

        public void Update()
        {
            if (target.IsValid) {
                Vector3 targetDir = (target.Pos - transform.position).normalized;
                if (Vector3.Angle(transform.forward, targetDir) > targetDetectionAngle) {
                    LoseTarget();
                } else {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDir, transform.up), rotateToTargetSpeed * Time.deltaTime);
                }
            }
            timeLeft -= Time.deltaTime;
            if (timeLeft >= 0) {
                GetComponent<Rigidbody>().velocity = transform.forward * weapon.ammoSpeed;
            } 
            //TODO: stop particle emitter when no fuel left
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (factionId.IsFoe(collision.collider.gameObject)) {
                ApplyDamage(collision.collider);
            }
            Destroy(gameObject);
        }

        private void ApplyDamage(Collider hitTarget)
        {
            if (hitTarget.gameObject.GetComponent<Damagable>()) {
                hitTarget.gameObject.GetComponent<Damagable>().Damage(weapon.damage);
            }
        }

        private void LoseTarget()
        {
            target = InvalidTarget.Invalid;
            timeLeft = Mathf.Min(timeLeft, timeToWorkAfterTargetLost);
        }
    }
}
