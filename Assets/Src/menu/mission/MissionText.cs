using UnityEngine;
using UnityEngine.UI;

namespace menu.mission
{
    [RequireComponent(typeof(Text))]
    class MissionText: MonoBehaviour
    {
        public void SetMission(MissionDescr mission)
        {
            GetComponent<Text>().text = mission.MissionText;
        }
    }
}
