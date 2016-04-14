using System.Collections.Generic;
using UnityEngine;

namespace fow
{
    [RequireComponent(typeof(Renderer))]
    public class ApplyVisibilitySelf: ApplyVisibility
    {
        protected override IEnumerable<Renderer> Renderers
        {
            get { yield return GetComponent<Renderer>(); }
        }
    }
}
