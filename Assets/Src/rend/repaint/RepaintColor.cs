using UnityEngine;

namespace rend.repaint
{
    public class RepaintColor : MonoBehaviour, IRepaintable
    {
        public const string COLOR_PARAM_NAME = "_Color";

        public void Repaint(IPaintData paintData)
        {
            var renderer = gameObject.GetComponent<Renderer>();
            var prop = new MaterialPropertyBlock();
            prop.SetColor(COLOR_PARAM_NAME, paintData.Color);
            renderer.SetPropertyBlock(prop);
        }
    }
}
