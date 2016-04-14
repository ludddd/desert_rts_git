using System.IO;
using editor.utils;
using UnityEditor;
using UnityEngine;

namespace editor.menu
{
    class TerrainReplaceSplatMap
    {
        [MenuItem("CONTEXT/Terrain/Replace SplatMap")]
        public static void ReplaceSplatMap(MenuCommand menuCommand)
        {
            ShowWindow(menuCommand);
        }

        [MenuItem("CONTEXT/Terrain/Replace SplatMap Debug")]
        public static void ReplaceSplatMapDebug(MenuCommand menuCommand)
        {
            ShowWindow(menuCommand, true);
        }

        private static void ShowWindow(MenuCommand menuCommand, bool debug = false)
        {
            var terrain = menuCommand.context as Terrain;
            var wnd = ScriptableObject.CreateInstance<TextureSelectWindow>();
            wnd.Terrain = terrain;
            wnd.debug = debug;
            wnd.ShowUtility();
        }
    }

    public class TextureSelectWindow : EditorWindow
    {
        private bool isObjectPickerShown = false;
        public Terrain Terrain { get; set; }
        public bool debug = false;

        public TextureSelectWindow()
        {
            titleContent = new GUIContent("Select Splat Map Texture");
        }

        void OnGUI()
        {
            if (!isObjectPickerShown) {
                isObjectPickerShown = true;
                EditorGUIUtility.ShowObjectPicker<Texture2D>(null, false, "", 0);
            }
            if (Event.current.commandName == "ObjectSelectorClosed") {
                var texture = (Texture2D)EditorGUIUtility.GetObjectPickerObject();
                if (texture != null) {
                    Replace(texture);
                }
                Close();
            }            
        }

        public void Replace(Texture2D splatMap)
        {
            //texture compression should be argb (no compression)
            //+ read/write enabled
            var terrainData = Terrain.terrainData;
            float[,,] map = CreateSplatMapFromTexture(splatMap, terrainData.alphamapLayers);
            terrainData.SetAlphamaps(0, 0, map);
            if (debug)
            {
                SaveTexture(map, DebugSavePath);
            }
        }

        private static float[,,] CreateSplatMapFromTexture(Texture2D splatMap, int numLayers)
        {
            Color[] colors = splatMap.GetPixels();
            float[,,] map = new float[splatMap.width, splatMap.height, numLayers];
            for (int x = 0; x < splatMap.width; x++) {
                for (int y = 0; y < splatMap.height; y++) {
                    int idx = (splatMap.height - 1 - x) * splatMap.height + y;
                    var fixedColor = FixColor(colors[idx], numLayers);
                    for (int i = 0; i < numLayers; i++) {
                        map[x, y, i] = fixedColor[i];
                    }                    
                }
            }

            return map;
        }

        public static Vector4 FixColor(Color color, int numLayers)
        {
            var rez = new Vector4();
            for (int i = 0; i < numLayers; i++) {   //Maybe it should be just argb to rgba conversion...
                rez[i] = color[numLayers - i - 1];
            }
            return NormalizeForSplatMap(rez);
        }

        private static Vector4 NormalizeForSplatMap(Vector4 rez)
        {
            //unity splatmap should be normalized that way: https://alastaira.wordpress.com/2013/11/14/procedural-terrain-splatmapping/
            var norm = rez[0] + rez[1] + rez[2] + rez[3];
            return norm > 0 ? rez / norm : Vector4.zero;
        }

        private void SaveTexture(float[,,] map, string path)
        {
            var texture = new Texture2D(map.GetLength(0), map.GetLength(1), TextureFormat.RGBA32, false);
            var colors = new Color[texture.width*texture.height];
            for (int i = 0; i < texture.width; i++)
            {
                for (int k = 0; k < texture.height; k++)
                {
                    var color = FloatArrayToColor(map, i, k);
                    colors[i*texture.width + k] = color;
                }
            }
            texture.SetPixels(colors);
            texture.Apply();
            File.WriteAllBytes(path, texture.EncodeToPNG());
            Debug.Log("resulting splatmap is saved to " + path);
        }

        private Color FloatArrayToColor(float[,,] map, int i, int k)
        {
            var rez = new Color();
            for (int l = 0; l < map.GetLength(2); l++)
            {
                rez[l] = map[i, k, l];
            }
            rez.a = 1;
            return rez;
        }

        private string DebugSavePath
        {
            get { return Path.Combine(AssetUtils.ProjectPath, "splatmap_debug.png"); }
        }
    }
}
