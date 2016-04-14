using UnityEditor;
using UnityEngine;

namespace tests
{
    class TestAssetPath: MonoBehaviourEx
    {
        public Object asset;

        [editor.attr.Button]
        public void PrintPath()
        {
            Debug.Log(AssetDatabase.GetAssetPath(asset));
        }
    }
}
