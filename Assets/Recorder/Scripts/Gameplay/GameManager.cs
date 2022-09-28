using Recorder.Scripts.Data;
using Recorder.Scripts.Service;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Recorder.Scripts.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public enum EScenes
        {
            RECORD_SCENE = 0,
            LOADING_SCENE = 1,
            REPLAY_SCENE = 2
        }

        public static GameManager Instance { get; private set; }
        public RecordListData SelectedRecordData { get; set; }
        public int CurrentSceneIndex { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }

            CurrentSceneIndex = 0;
            SaveSystem.Initialize();
        }

        public void GoToLoadingScene(EScenes sceneEnum)
        {
            CurrentSceneIndex = (int) sceneEnum;
            SceneManager.LoadScene("LoadingScene");
        }
    }
}