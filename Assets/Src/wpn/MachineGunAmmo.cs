using UnityEngine;
using faction;
using System.Linq;

namespace wpn
{
    class MachineGunAmmo : MonoBehaviour, IAmmo
    {
        public float trailLength = 1.0f;

        private float distLeft;
        private Weapon weapon;
        private FactionData.FactionId factionId;
        private Vector3 dir;

        public void Setup(Vector3 from, Vector3 dir, Weapon weapon, FactionData.FactionId factionId, ITarget target)
        {
            transform.position = from;
            this.dir = dir.normalized;
            this.weapon = weapon;
            this.factionId = factionId;
            var lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetVertexCount(2);
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position);
            distLeft = weapon.shootDist;
        }

        public void Update()
        {
            if (distLeft <= 0) {
                Destroy(gameObject);
            }
            RaycastHit hitInfo;
            float frameDist = Mathf.Min(distLeft, weapon.ammoSpeed * Time.deltaTime);
            if (Raycast(transform.position, dir, out hitInfo, frameDist)) {
                ApplyDamage(hitInfo.collider);
                distLeft = 0;
                transform.position = hitInfo.point;
            } else {
                transform.Translate(dir * frameDist);
                distLeft -= frameDist;
            }

            float length = Mathf.Min(trailLength, DistPassed());
            var lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position - length * dir);
        }

        private float DistPassed()
        {
            return weapon.shootDist - distLeft;
        }

        private bool Raycast(Vector3 from, Vector3 dir, out RaycastHit hitInfo, float maxDist)
        {
            RaycastHit[] hits = Physics.RaycastAll(from, dir, maxDist, ~LayerMask.GetMask(Layers.Minimap));
            if (hits.Length > 0) {
                foreach(var hit in hits.OrderBy(v => v.distance)) { //RaycastAll does not guarante order
                    if (factionId.IsFoe(hit.collider.gameObject)) {
                        hitInfo = hit;
                        return true;
                    }
                }
            }
            hitInfo = new RaycastHit();
            return false;
        }

        private void ApplyDamage(Collider hitTarget)
        {
            if (hitTarget.gameObject.GetComponent<Damagable>()) {
                hitTarget.gameObject.GetComponent<Damagable>().Damage(weapon.damage);
            }
        }        
    }
}
