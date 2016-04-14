using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fow
{
    public abstract class ApplyVisibility: MonoBehaviour
    {
        private IVisibilityChecker visibilityChecker;

        void Start()
        {
            visibilityChecker = TerrainVisibilityCheckerFactory.Create();
            enabled = visibilityChecker != null;
        }

        void Update()
        {
            StartCoroutine(UpdateVisibilityAfterTimer());
        }

        IEnumerator UpdateVisibilityAfterTimer()
        {
            yield return null;
            bool isVisible = visibilityChecker.IsVisible(transform.position);
            foreach (var c in Renderers) {
                c.enabled = isVisible;
            }
        }

        protected abstract IEnumerable<Renderer> Renderers { get; } 
    }
}
