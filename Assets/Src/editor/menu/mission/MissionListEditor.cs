using System.IO;
using System.Linq;
using editor.utils;
using menu.mission;
using utils;
using UnityEditor;
using UnityEngine;

namespace editor.menu.mission
{
    [CustomEditor(typeof(MissionList))]
    class MissionListEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawUpdateButton();
        }

        private void DrawUpdateButton()
        {
            if (GUILayout.Button("UpdateMissionList"))
            {
                var missionList = (MissionList) target;
                missionList.transform.DestroyImmediateAllChildren();
                foreach (var item in GetMissionsInFolder(MissionPath))
                {
                    item.transform.SetParent(missionList.transform, false);
                }
            }
        }

        private GameObject[] GetMissionsInFolder(string path)
        {
            var rez = from item in Directory.GetFiles(path, "*.unity") select GetMissionDescr(item);
            return rez.ToArray();
        }

        private string MissionPath
        {
            get { return Path.Combine(Application.dataPath, ((MissionList)target).MissionFolder); }
        }

        public GameObject GetMissionDescr(string path)
        {
            var localPath = PathUtils.GetRelativePath(AssetUtils.ProjectPath, path);
            var asset = AssetDatabase.LoadAssetAtPath<GameObject>(Path.ChangeExtension(localPath, ".prefab"));
            var rez = (GameObject) PrefabUtility.InstantiatePrefab(asset);
            Debug.Assert(rez.GetComponent<MissionDescr>() != null);
            return rez;
        }
    }
}
