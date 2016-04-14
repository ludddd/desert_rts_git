using System.IO;
using menu.mission;
using ui.minimap;
using utils;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace editor.menu.mission
{
    public class MissionDescrBuilder
    {
        private const int RESOLUTION = 1024;
        private Camera camera;
        private GameObject obj;

        [MenuItem("Tools/Create Mission Descr")]
        private static void CrearePrefab()
        {
            var prefabObj = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            MissionDescrBuilder prefabBuild;
            //TODO: don't like this if...
            if (prefabObj == null)
            {
                prefabBuild = new MissionDescrBuilder(null);
                prefabObj = PrefabUtility.CreatePrefab(PrefabPath, prefabBuild.GetRootObj());
                var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefabObj);
                instance.SetActive(false);
            }
            else
            {
                prefabBuild = new MissionDescrBuilder(prefabObj.GetComponent<MissionDescr>());
                PrefabUtility.ReplacePrefab(prefabBuild.GetRootObj(), prefabObj);
            }
            prefabBuild.Term();
        }

        private static string PrefabPath
        {
            get { return GetSavePath(".prefab"); }
        }

        private MissionDescrBuilder(MissionDescr oldDescr)
        {
            camera = CreateCamera();
            obj = CreateGameObject(oldDescr);
            SetupCamera(Terrain.activeTerrain.GetComponent<Collider>().bounds);
            CreateTerrainImage();
            CreateIcons();
        }

        private static Camera CreateCamera()
        {
            var camObj = new GameObject();
            return camObj.AddComponent<Camera>();
        }

        private static GameObject CreateGameObject(MissionDescr oldDescr)
        {
            var obj = new GameObject("MissionDescr");
            var descr = obj.AddComponent<MissionDescr>();
            descr.Init(Path.GetFileNameWithoutExtension(ScenePath), oldDescr);
            return obj;
        }

        private void SetupCamera(Bounds bbox)
        {
            camera.aspect = bbox.size.x / bbox.size.z;
            camera.backgroundColor = Color.black;
            camera.targetTexture = new RenderTexture(Mathf.FloorToInt(RESOLUTION * camera.aspect), RESOLUTION, 32);
            camera.cullingMask = LayerMask.GetMask(Layers.Terrain, "MissionMap");

            float camHeight = CalcCameraHeight(camera, bbox);
            camera.transform.position = bbox.center + camHeight * Vector3.up;   //not exactly correct - mountain on the edge may not fit...
            camera.transform.rotation = LookDown(); 
        }

        private static float CalcCameraHeight(Camera cam, Bounds bbox)
        {
            float horzTan = Mathf.Tan(0.5f * Mathf.Deg2Rad * cam.fieldOfView * cam.aspect);
            float vertTan = Mathf.Tan(0.5f * Mathf.Deg2Rad * cam.fieldOfView);
            float camHeight = Mathf.Max(bbox.extents.x / horzTan, bbox.extents.z / vertTan);
            return camHeight;
        }

        private static Quaternion LookDown()
        {
            return Quaternion.Euler(90, 0, 0);
        }

        private void CreateIcons()
        {            
            var iconCreator = obj.AddComponent<IconCreator>();
            iconCreator.CreateIcons(camera);
        }

        private void CreateTerrainImage()
        {
            var image = obj.AddComponent<RawImage>();
            image.texture = CreateTerrainTexture();
            var aspectFitter = obj.AddComponent<AspectRatioFitter>();
            aspectFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            aspectFitter.aspectRatio = GetAspectRatio(image);
        }

        private static int GetAspectRatio(RawImage image)
        {
            return image.texture.width / image.texture.height;
        }

        private Texture CreateTerrainTexture()
        {
            camera.Render();
            var texture = RenderTextureUtil.CreateTextureFromRT(camera.targetTexture);
            var bytes = texture.EncodeToPNG();
            var path = GetSavePath(".png");
            File.WriteAllBytes(GetSavePath(".png"), bytes);
            return AssetDatabase.LoadAssetAtPath<Texture>(path);  //if we just return texture it won't be saved to prefab
        }

        private static string GetSavePath(string extension)
        {
            var scenePath = ScenePath;
            var rez = Path.GetFileNameWithoutExtension(scenePath) + extension;
            var sceneFolder = Path.GetDirectoryName(scenePath);
            if (sceneFolder != null) {
                rez = Path.Combine(sceneFolder, rez);
            }
            return rez.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        private static string ScenePath
        {
            get
            {
                return UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
            }
        }

        private void Term()
        {
            Object.DestroyImmediate(camera.gameObject);
            camera = null;
            Object.DestroyImmediate(obj);
            obj = null;
        }

        public GameObject GetRootObj()
        {
            return obj;
        }
    }
}
