using System.Collections.Generic;
using UnityEngine;

namespace fow
{
    public class ApplyVisibilityFOWChildren : ApplyVisibility
    {
        protected override IEnumerable<Renderer> Renderers
        {
            get
            {
                foreach (var renderer in GetComponentsInChildren<Renderer>())
                {
                    if (renderer.material.shader.name == "Custom/MobileDiffuseFOW")
                    {
                        yield return renderer;
                    }
                }
            }
        }
    }
}

