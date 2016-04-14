using UnityEngine;

namespace rend.repaint
{
    [RequireComponent(typeof(Renderer))]
    public class RepaintTexture : MonoBehaviour, IRepaintable
    {

        public const string TEX_PARAM_NAME = "_MainTex";

        public void Repaint(IPaintData paintData)
        {
            var prop = new MaterialPropertyBlock();
            prop.SetTexture(TEX_PARAM_NAME, paintData.Texture);
            var renderer = gameObject.GetComponent<Renderer>();
            renderer.SetPropertyBlock(prop);
        }
    }
}