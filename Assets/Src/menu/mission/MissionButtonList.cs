using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace menu.mission
{
    public class MissionButtonList : MonoBehaviour
    {
        [SerializeField]
        private GameObject buttonProto = null;
        [SerializeField]
        private GameObject missionListProvider = null;
        [SerializeField]
        private float buttonAspectRatio = 0.25f;

        public void Start()
        {
            CreateButtons();
            SelectFirstMission();
        }

        private void SelectFirstMission()
        {
            if (MissionList.Any())
            {
                OnMissionSelected(MissionList.First());
            }           
        }

        private MissionList MissionList
        {
            get { return missionListProvider.GetComponent<MissionList>(); }
        }

        private void CreateButtons()
        {
            foreach (var item in MissionList)
            {
                CreateMissionButton(item);
            }
            HideProtoButton();
        }

        private void CreateMissionButton(MissionDescr mission)
        {
            var buttonObj = Instantiate(buttonProto);
            buttonObj.transform.SetParent(transform);
            buttonObj.GetComponentInChildren<Text>().text = mission.MissionName;
            var button = buttonObj.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => OnMissionSelected(mission));
            button.GetComponent<LayoutElement>().preferredHeight = ButtonHeight;
        }

        private float ButtonHeight
        {
            get
            {
                return ((RectTransform)transform).rect.width * buttonAspectRatio;
            }
        }

        private void HideProtoButton()
        {
            buttonProto.SetActive(false);
        }

        private void OnMissionSelected(MissionDescr mission)
        {
            MissionList.SelectMission(mission);
        }
    }
}
