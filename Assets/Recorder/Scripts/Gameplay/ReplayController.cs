using System.Collections.Generic;
using Cinemachine;
using Recorder.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Recorder.Scripts.Gameplay
{
    public class ReplayController : MonoBehaviour
    {
        [SerializeField] private CinemachineFreeLook _cineCam;
        [SerializeField] private Button _recordBtn;
        [SerializeField] private Button _replayBtn;

        private bool _isReplay;
        private bool _isRecording;
        private List<RecordedCamData> _records = new();
        [SerializeField] private float _currentReplayIndex;
        [SerializeField] private float _indexChangeRate;

        private void OnEnable()
        {
            _recordBtn.onClick.AddListener(() => { _isRecording = !_isRecording; });
            _replayBtn.onClick.AddListener(ChangeReplayState);
        }

        private void OnDisable()
        {
            _recordBtn.onClick.RemoveAllListeners();
            _replayBtn.onClick.RemoveAllListeners();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ChangeReplayState();
            }

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

        private void ChangeReplayState()
        {
            _isReplay = !_isReplay;
            if (_isReplay)
            {
                _cineCam.m_XAxis.m_MaxSpeed = 0;
                _cineCam.m_YAxis.m_MaxSpeed = 0;
                Debug.Log(_cineCam.m_YAxis.m_MaxSpeed + " " + _cineCam.m_XAxis.m_MaxSpeed);
                SetCinemachineAxis(0);
            }
            else
            {
                _cineCam.m_XAxis.m_MaxSpeed = 300;
                _cineCam.m_YAxis.m_MaxSpeed = 2;
                SetCinemachineAxis(_records.Count - 1);
            }
        }

        private void LateUpdate()
        {
            if (!_isReplay && _isRecording)
            {
                // ReSharper disable once Unity.InefficientPropertyAccess
                _records.Add(new RecordedCamData(_cineCam.transform.position, _cineCam.m_YAxis.Value,
                    _cineCam.m_XAxis.Value));
            }
            else if (_isReplay)
            {
                float nextIndex = _currentReplayIndex + _indexChangeRate;

                if (nextIndex < _records.Count && nextIndex >= 0)
                {
                    SetCinemachineAxis(nextIndex);
                }
            }
        }

        private void SetCinemachineAxis(float index)
        {
            _currentReplayIndex = index;
            RecordedCamData targetRecordedCamData = _records[(int) index];

            _cineCam.m_YAxis.Value = targetRecordedCamData.yAxis;
            _cineCam.m_XAxis.Value = targetRecordedCamData.xAxis;

            transform.position = targetRecordedCamData.rPosition;
        }
    }
}