using System;
using DG.Tweening;
using Recorder.Scripts.Gameplay;
using Recorder.Scripts.Service.Observer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Recorder.Scripts.UI
{
    public class ReplayUIHandler : MonoBehaviour
    {
        [SerializeField] private Transform _helpBox;
        [SerializeField] private Button _helpBtn;
        [SerializeField] private Button _exitBtn;
        [SerializeField] private Button _autoReplayBtn;
        [SerializeField] private Button _keyboardReplayBtn;
        [SerializeField] private Button _stopReplayBtn;
        [SerializeField] private TextMeshProUGUI _recName;
        [SerializeField] private Slider _progressSlider;
        
        private bool _showHelp;
        private ObserverSystem _observer;

        private void Awake()
        {
            _observer = FindObjectOfType<ObserverSystem>();
        }

        private void Start()
        {
            _recName.text = GameManager.Instance.SelectedRecordData.recName;
        }

        private void OnEnable()
        {
            _helpBtn.onClick.AddListener(ShowHelpBox);
            _exitBtn.onClick.AddListener(ExitToRecordScene);
            _autoReplayBtn.onClick.AddListener(AutoReplay);
            _keyboardReplayBtn.onClick.AddListener(KeyboardReplay);
            _stopReplayBtn.onClick.AddListener(StopReplay);
            
            _observer.ListenToEvent<float>(EObserver.REPLAY_SLIDER_VALUE, OnSetSliderValue);
        }

        private void OnDisable()
        {
            _helpBtn.onClick.RemoveAllListeners();
            _exitBtn.onClick.RemoveAllListeners();
            _autoReplayBtn.onClick.RemoveAllListeners();
            _keyboardReplayBtn.onClick.RemoveAllListeners();
            _stopReplayBtn.onClick.RemoveAllListeners();
            
            _observer.RemoveEventListener<float>(EObserver.REPLAY_SLIDER_VALUE, OnSetSliderValue);
        }

        private void ShowHelpBox()
        {
            _showHelp = !_showHelp;
            if (_showHelp)
            {
                _helpBox.DOMoveX(0, .5f);
                return;
            }

            _helpBox.DOMoveX(-400, .5f);
        }

        private void ExitToRecordScene()
        {
            GameManager.Instance.GoToLoadingScene(GameManager.EScenes.RECORD_SCENE);
        }

        private void AutoReplay()
        {
            _observer.BroadcastEvent(EObserver.REPLAY_AUTO);
        }

        private void KeyboardReplay()
        {
            _observer.BroadcastEvent(EObserver.REPLAY_KEYBOARD);
        }

        private void StopReplay()
        {
            _observer.BroadcastEvent(EObserver.REPLAY_STOP);
        }

        private void OnSetSliderValue(float value)
        {
            _progressSlider.value = value;
        }
    }
}