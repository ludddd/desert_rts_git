using System.Collections.Generic;
using UnityEngine;

namespace vfx
{
    class RandomParticleOnTerrain : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem proto;
        [SerializeField]
        private float periodSec;
        [SerializeField]
        [Range(0, 1)]
        private float periodDisp;
        private float timeToNext;
        [SerializeField]
        private float heightOverTerrain;
        private List<ParticleSystem> pool = new List<ParticleSystem>();

        private void scheduleNext()
        {
            timeToNext = periodSec * Random.Range(1.0f - periodDisp, 1.0f + periodDisp);
        }

        public void Start()
        {
            scheduleNext();
        }

        public void OnDestroy()
        {
            pool.Clear();
        }

        public void Update()
        {
            timeToNext -= Time.deltaTime;
            if (timeToNext <= 0) {
                AddEffect();
                scheduleNext();
            }
        }

        private void AddEffect()
        {
            var effect = GetOrCreate();
            effect.transform.position = GetRandomPos();
            effect.Play();
        }

        private ParticleSystem GetOrCreate()
        {
            var obj = pool.Find(item => item.isStopped);
            if (obj == null) {
                obj = Instantiate(proto);
                obj.transform.SetParent(utils.RuntimeObjectSceneFolder.Instance.Get("vfx"));
                pool.Add(obj);
            }
            return obj;
        }

        private Vector3 GetRandomPos()
        {
            var area = GetVisibleTerrainArea(Terrain.activeTerrain, Camera.main);
            //TODO: ? zero size area ?
            var pos = RandomPointInArea(area);
            pos.y = Terrain.activeTerrain.SampleHeight(pos) + heightOverTerrain;
            return pos;
        }

        private static Bounds GetVisibleTerrainArea(Terrain terrain, Camera camera)
        {
            var bottomLeft = utils.Projection.ProjectToTerrain(camera, terrain.GetComponent<TerrainCollider>(), new Vector3(0, 0));
            var topRight = utils.Projection.ProjectToTerrain(camera, terrain.GetComponent<TerrainCollider>(), new Vector3(1, 1));
            Bounds bounds = new Bounds(0.5f * (bottomLeft + topRight), Vector3Abs(topRight - bottomLeft));
            return IntersectBounds(bounds, terrain.GetComponent<TerrainCollider>().bounds);
        }

        private static Vector3 Vector3Abs(Vector3 v)
        {
            return Vector3.Max(v, -v);
        }

        private static Bounds IntersectBounds(Bounds a, Bounds b)
        {
            var rez = new Bounds();
            rez.SetMinMax(Vector3.Max(a.min, b.min), Vector3.Min(a.max, b.max));
            return rez;
        }

        private static Vector3 RandomPointInArea(Bounds area)
        {
            return new Vector3(Random.Range(area.min.x, area.max.x),
                                Random.Range(area.min.y, area.max.y),
                                Random.Range(area.min.z, area.max.z));
        }
    }
}
