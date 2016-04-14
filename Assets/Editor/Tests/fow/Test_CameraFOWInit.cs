using NUnit.Framework;
using UnityEngine;

namespace fow
{
    [TestFixture]
    class Test_CameraFOWInit
    {
        [Test]
        public void SceneWithoutTerrain()
        {
            var obj = new GameObject();           
            var cam = obj.AddComponent<Camera>();
            Assert.NotNull(cam);
            var fow = obj.AddComponent<CameraFOW>();
            Assert.NotNull(fow);
            
            fow.OnPreRender();
            fow.OnPostRender();
        }
    }
}
