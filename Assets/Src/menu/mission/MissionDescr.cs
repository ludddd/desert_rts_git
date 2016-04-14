using UnityEngine;

namespace menu.mission
{
    public class MissionDescr:MonoBehaviour
    {
        [SerializeField]
        private string sceneName;
        [SerializeField]
        private string missionName;
        [SerializeField]
        private string missionText;

        public string SceneName
        {
            get { return sceneName; }
        }

        public string MissionName
        {
            get { return missionName.Length > 0 ? missionName : SceneName; }
        }

        public string MissionText
        {
            get { return missionText; }
        }

        public void Init(string sceneName, MissionDescr prevValue)
        {
            this.sceneName = sceneName;
            if (prevValue != null)
            {
                missionName = prevValue.missionName;
                missionText = prevValue.missionText;
            }
        }
    }
}