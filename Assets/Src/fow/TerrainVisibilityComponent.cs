using System.Collections;
using grid;
using UnityEngine;

namespace fow
{
    [RequireComponent(typeof(TerrainCollider))] //TODO: we do not need terrain here, just bbox...
    class TerrainVisibilityComponent : MonoBehaviour, IVisibilityChecker
    {
        [SerializeField]
        private float cellSize = 1.0f;
        [SerializeField]
        private GridFactory.Type gridType = GridFactory.Type.Square;
        private IGrid grid;
        private BoolGrid visible;
        private BoolGrid explored;

        public float CellSize { get { return cellSize; } }
        public IGrid Grid { get { return grid; } }

        public BoolGrid VisibleArea
        {
            get
            {
                InitFieldIfNotYet();
                return visible;
            }
        }

        public BoolGrid ExploredArea
        {
            get
            {
                InitFieldIfNotYet();
                return explored;
            }
        }

        void Start()
        {
            InitFieldIfNotYet();
        }

        private void InitFieldIfNotYet()
        {
            if (visible == null || explored == null) {
                Debug.Assert(visible == null && explored == null);
                grid = GridFactory.CreateForArea(gridType, GetComponent<TerrainCollider>().bounds, cellSize);
                visible = new BoolGrid(grid);
                explored = new BoolGrid(grid);
            }
        }

        void Update()
        {
            StartCoroutine(ClearVisibilityOnFrameEnd());
        }

        IEnumerator ClearVisibilityOnFrameEnd()
        {
            yield return new WaitForEndOfFrame();
            visible.Clear();
        }

        public void markAreaVisible(Vector3 center, float radius)
        {
            visible.MarkArea(center, radius);
            explored.Or(visible);
        }

        public bool IsVisible(Vector3 pos)
        {
            return visible.Is(pos);
        }        
    }
}
