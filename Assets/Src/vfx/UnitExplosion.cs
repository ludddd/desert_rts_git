using UnityEngine;

namespace vfx
{
    class UnitExplosion: MonoBehaviourEx
    {
        [editor.attr.Button]
        public void Play()
        {
            foreach (var ps in GetComponentsInChildren<ParticleSystem>())
            {
                ps.Play();
            }
        }
    }
}
