using System;
using utils;
using UnityEngine;
using vfx;
using faction;

namespace unit
{
    class DestroyedUnit : MonoBehaviour
    {
        public static GameObject Create(GameObject unit)
        {
            var proto = Resources.Load<GameObject>("DestroyedUnit");
            var newObj = Instantiate(proto);
            CopyTransform(newObj.transform, unit.transform);
            CopyChildrenWithFilter(newObj, unit, MeshWithDefaultLayer);
            SetupTurretBlowUp(newObj, unit);
            RepaintDestroyed(newObj, unit);
            CreateNavObstacle(newObj, unit);
            return newObj;
        }

        private static void RepaintDestroyed(GameObject target, GameObject source)
        {
            var data = Faction.GetFactionDataIfAny(source);
            if (data.HasValue) UnitRepaint.Repaint(target, data.Value.AsDestroyedPaintData());
        }

        private static void SetupTurretBlowUp(GameObject target, GameObject source)
        {
            var turretBlowUp = target.GetComponent<TurretBlowUp>();
            if (!turretBlowUp) return;
            var turretComp = source.GetComponent<Turret>();
            if (!turretComp) return;
            var turretObj = turretComp.turretObj;   
            if (turretObj == null) return;
            Debug.Assert(target.transform.IsAncestorOf(turretObj.transform));
            turretBlowUp.Play(turretObj);
        }

        private static void CopyTransform(Transform target, Transform source)
        {
            target.SetParent(RuntimeObjectSceneFolder.Instance.Get("wreckage"));
            target.position = source.position;
            target.rotation = source.rotation;
            Debug.Assert(Vector3.Distance(source.transform.lossyScale, source.transform.localScale) < 0.001f,
                "this function will work wrong if unit parent scale will be used");
            target.localScale = source.localScale;
        }

        private static void CopyChildrenWithFilter(GameObject target, GameObject source, Func<GameObject, bool> filter)
        {
            for (int i = source.transform.childCount - 1; i >= 0; i--)
            {
                var child = source.transform.GetChild(i);
                if (!filter(child.gameObject)) continue;
                child.SetParent(target.transform); //TODO: keep order of children
            }
        }

        private static bool MeshWithDefaultLayer(GameObject obj)
        {
            if (obj.layer != LayersInt.Default) return false;
            if (obj.GetComponentInChildren<MeshRenderer>() == null) return false;
            return true;
        }

        private static void CreateNavObstacle(GameObject newObj, GameObject unit)
        {
            var obstacle = newObj.AddComponent<NavMeshObstacle>();
            obstacle.carving = false;
            obstacle.shape = NavMeshObstacleShape.Box;
            var unitBounds = unit.GetComponent<BoxCollider>().bounds;
            obstacle.center = obstacle.transform.InverseTransformPoint(unitBounds.center);
            obstacle.size = obstacle.transform.InverseTransformVector(unitBounds.size);
        }
    }
}
