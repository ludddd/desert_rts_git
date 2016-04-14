using System;
using System.Collections;
using System.Collections.Generic;
using utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace menu.mission
{
    public class MissionList: MonoBehaviour, IEnumerable<MissionDescr>
    {
        [SerializeField]
        private string missionFolder = "test_scenes";
        private IEnumerable<MissionDescr> missions;
        private MissionDescr current;   

        [Serializable]
        public class MissionSelectedEvent : UnityEvent<MissionDescr>
        {
        }

        public MissionSelectedEvent MissionSelected;
        public UnityEventWithString MissionStarted;

        private IEnumerable<MissionDescr> Missions
        {
            get
            {
                if (missions == null)
                {
                    missions = GetComponentsInChildren<MissionDescr>();
                }
                return missions;
            }
        }

        public IEnumerator<MissionDescr> GetEnumerator()
        {
            return Missions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Missions.GetEnumerator();
        }

        public void SelectMission(MissionDescr mission)
        {
            Debug.Log("mission " + mission.SceneName + " is selected");
            current = mission;
            MissionSelected.Invoke(mission);
        }

        //let this function be here for a while...
        public void StartCurrentMission()
        {
            MissionStarted.Invoke(current.SceneName);
        }

        public string MissionFolder
        {
            get { return missionFolder; }
        }
    }
}
