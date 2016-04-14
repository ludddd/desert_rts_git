using UnityEngine;
using UnityEngine.SceneManagement;

namespace menu
{
    class SceneLoader: MonoBehaviour
    {
        private const string MAIN_MENU_SCENE_NAME = "MainMenu";

        public void LoadMission(string missionName)
        {
            SceneManager.LoadScene(missionName, LoadSceneMode.Single);
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene(MAIN_MENU_SCENE_NAME, LoadSceneMode.Single);
        }
    }
}
