using UnityEngine;

namespace fow.test
{
    class TestTexture: MonoBehaviour
    {
        [SerializeField] private Texture texture;

        private void Start()
        {
            Shader.SetGlobalTexture(FOWRenderer.TEXTURE_PARAM_NAME, texture);
        }
    }
}
