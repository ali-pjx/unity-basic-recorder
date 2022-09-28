using System;
using System.Collections.Generic;
using Cinemachine;
using Recorder.Scripts.Data;
using Recorder.Scripts.Service.Observer;
using UnityEngine;

namespace Recorder.Scripts.Gameplay
{
    public class ReplayController : MonoBehaviour
    {
        [SerializeField] private CinemachineFreeLook _cineCam;
        [SerializeField] private float _currentReplayIndex;
        [SerializeField] private float _indexChangeRate;

        private bool _isPlaying;
        private bool _autoReplay;
        private List<RecordedCamData> _recordedData = new();
        private ObserverSystem _observer;

        private void Awake()
        {
            _observer = FindObjectOfType<ObserverSystem>();
            _recordedData = GameManager.Instance.SelectedRecordData.recordedCamData;
        }

        private void OnEnable()
        {
            _observer.ListenToEvent(EObserver.REPLAY_AUTO, OnAutoReplay);
            _observer.ListenToEvent(EObserver.REPLAY_KEYBOARD, OnKeyboardReplay);
            _observer.ListenToEvent(EObserver.REPLAY_STOP, OnStopReplay);
        }

        private void OnDisable()
        {
            _observer.RemoveEventListener(EObserver.REPLAY_AUTO, OnAutoReplay);
            _observer.RemoveEventListener(EObserver.REPLAY_KEYBOARD, OnKeyboardReplay);
            _observer.RemoveEventListener(EObserver.REPLAY_STOP, OnStopReplay);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ChangeReplayState();
            }

            if (_autoReplay) return;

            _indexChangeRate = 0;

            if (Input.GetKey(KeyCode.RightArrow))
            {
                _indexChangeRate = 1;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _indexChangeRate = -1;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                _indexChangeRate = .5f;
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                return;
            }
        }

        private void LateUpdate()
        {
            if (_isPlaying)
            {
                float nextIndex = _currentReplayIndex + _indexChangeRate;

                if (nextIndex < _recordedData.Count && nextIndex >= 0)
                {
                    SetCinemachineAxis(nextIndex);
                }
            }
        }

        private void ChangeReplayState()
        {
            _isPlaying = !_isPlaying;
            if (_isPlaying)
            {
                _cineCam.m_XAxis.m_MaxSpeed = 0;
                _cineCam.m_YAxis.m_MaxSpeed = 0;
                Debug.Log(_cineCam.m_YAxis.m_MaxSpeed + " " + _cineCam.m_XAxis.m_MaxSpeed);
                SetCinemachineAxis(0);
            }
        }

        private void SetCinemachineAxis(float index)
        {
            _currentReplayIndex = index;
            RecordedCamData targetRecordedCamData = _recordedData[(int) index];

            _cineCam.m_YAxis.Value = targetRecordedCamData.yAxis;
            _cineCam.m_XAxis.Value = targetRecordedCamData.xAxis;

            transform.position = targetRecordedCamData.rPosition;
        }

        private void OnAutoReplay()
        {
            _autoReplay = _isPlaying = true;
            _indexChangeRate = 1;
        }

        private void OnKeyboardReplay()
        {
            _isPlaying = true;
            _autoReplay = false;
        }

        private void OnStopReplay()
        {
            _isPlaying = false;
        }
    }
}