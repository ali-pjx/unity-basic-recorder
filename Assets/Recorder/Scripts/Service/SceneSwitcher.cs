using System.Collections;
using Recorder.Scripts.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Recorder.Scripts.Service
{
    public class SceneSwitcher : MonoBehaviour
    {
        [SerializeField] private Slider _loadingSlider;

        private void Start()
        {
            StartCoroutine(SwitchToGameScene());
        }

        private IEnumerator SwitchToGameScene()
        {
            // Bug: Visible test version
            {
                float loadingTime = 0;
                while (loadingTime <= 1)
                {
                    loadingTime += Time.deltaTime;
                    float progress = Mathf.Clamp01(loadingTime / .8f);

                    _loadingSlider.value = progress;
                    yield return null;
                }

                SceneManager.LoadScene(GameManager.Instance.CurrentSceneIndex);
            }

            // todo: correct version
            {
                /*AsyncOperation loadOp = SceneManager.LoadSceneAsync(GameManager.Instance.CurrentSceneIndex);
                while (!loadOp.isDone)
                {
                    float progress = Mathf.Clamp01(loadOp.progress / .8f);
    
                    _loadingSlider.value = progress;
                    yield return null;
                }*/
            }
        }
    }
}