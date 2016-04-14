using System;
using System.ComponentModel;
using faction;
using UnityEngine;
using Random = UnityEngine.Random;

namespace wpn
{
    [CreateAssetMenu(fileName = "weapon", menuName = "Create Weapon Type")]
    public class Weapon: ScriptableObject
    {
        public float fireRate;
        public GameObject ammoPrefab;
        public float ammoSpeed;
        public float shootDist;
        public float damage;
        public float angleDisp;
        public bool drawHitLine;

        public void Shoot(Vector3 from, Vector3 to, FactionData.FactionId factionId, ITarget target)
        {
            GameObject ammoObj = Instantiate(ammoPrefab);
            ammoObj.transform.SetParent(utils.RuntimeObjectSceneFolder.Instance.Get("ammo"));
            var ammo = ammoObj.GetComponent<IAmmo>();
            Vector3 dir = RandomizeDir((to - from).normalized, angleDisp);
            ammo.Setup(from, dir, this, factionId, target);
        }

        private static Vector3 RandomizeDir(Vector3 dir, float angleDisp)
        {
            return Quaternion.AngleAxis(Random.Range(0, 360), dir) * Quaternion.AngleAxis(Random.Range(0, angleDisp), Vector3.up) * dir;
        }

        public float ShootDist
        {
            get
            {
                return shootDist;
            }
        }

        public bool CanHit(Vector3 from, Vector3 to)
        {
            if (Vector3.Distance(from, to) > shootDist) {
                return false;
            }
            var obstacle = ShootCast(from, to);
            return obstacle == null;
        }

        private static int WeaponHitMask
        {
            get { return LayerMask.GetMask(Layers.Terrain); }
        }

        public RaycastHit? ShootCast(Vector3 from, Vector3 to)
        {
            RaycastHit hitInfo;
            float dist = Mathf.Min(shootDist, Vector3.Distance(from, to));
            bool isHit = Physics.Raycast(from, (to - from).normalized, out hitInfo, dist, WeaponHitMask);
            return isHit ? new RaycastHit?(hitInfo) : null;
        }
    }
}
