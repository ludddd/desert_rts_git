using System.IO;
using utils;
using UnityEditor;
using UnityEngine;

namespace editor.utils
{
    static class AssetUtils
    {
        public static Texture2D LoadSceneTexture(string scenePath)
        {
            var texturePath = SceneToTextureFileName(scenePath);
            var assetPath = PathUtils.GetRelativePath(ProjectPath, texturePath);
            assetPath = assetPath.Replace(Path.DirectorySeparatorChar, '/');    //unity accept only this separator
            var rez = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture2D)) as Texture2D;
            return rez;
        }

        private static string SceneToTextureFileName(string scenePath)
        {
            var texturePath = Path.GetFileNameWithoutExtension(scenePath) + ".png";
            var textureDir = Path.GetDirectoryName(scenePath);
            if (textureDir != null) texturePath = Path.Combine(textureDir, texturePath);
            return texturePath;
        }

        public static string ProjectPath
        {
            get { return Path.GetFullPath(Path.Combine(Application.dataPath, "..")); }
        }
    }
}
