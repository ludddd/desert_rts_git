using UnityEngine;
using UnityEngine.UI;

namespace menu.mission
{
    [RequireComponent(typeof(Image))]
    public class MissionMap: MonoBehaviour
    {
        public void SetMission(MissionDescr mission)
        {
            DisableAllChildren();
            mission.transform.SetParent(transform, false);
            mission.gameObject.SetActive(true);
        }

        private void DisableAllChildren()
        {
            for (int i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
