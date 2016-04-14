using NUnit.Framework;
using UnityEngine;

namespace editor.menu
{
    [TestFixture]
    class Test_TerrainReplaceSplatMap
    {
        [Test]
        public void FixColor()
        {
            Assert.AreEqual(Vector4.zero, TextureSelectWindow.FixColor(Color.black, 1));
            Assert.AreEqual(Vector4.zero, TextureSelectWindow.FixColor(Color.black, 2));
            Assert.AreEqual(Vector4.zero, TextureSelectWindow.FixColor(Color.black, 3));
            Assert.AreEqual(new Vector4(1, 0, 0, 0), TextureSelectWindow.FixColor(Color.white, 1));
            Assert.AreEqual(new Vector4(1, 1, 0, 0) / 2, TextureSelectWindow.FixColor(Color.white, 2));
            Assert.AreEqual(new Vector4(1, 1, 1, 0) / 3, TextureSelectWindow.FixColor(Color.white, 3));
            Assert.AreEqual(new Vector4(0, 0, 1, 0), TextureSelectWindow.FixColor(Color.red, 3));
            Assert.AreEqual(new Vector4(0, 1, 0, 0), TextureSelectWindow.FixColor(Color.green, 3));
            Assert.AreEqual(new Vector4(1, 0, 0, 0), TextureSelectWindow.FixColor(Color.blue, 3));
            Assert.AreEqual(new Vector4(1, 0, 0, 0), TextureSelectWindow.FixColor(Color.grey, 1));
            Assert.AreEqual(new Vector4(1, 1, 0, 0) / 2, TextureSelectWindow.FixColor(Color.grey, 2));
            Assert.AreEqual(new Vector4(1, 1, 1, 0) / 3, TextureSelectWindow.FixColor(Color.grey, 3));
        }
    }
}
