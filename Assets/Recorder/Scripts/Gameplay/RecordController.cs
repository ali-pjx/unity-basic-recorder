using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Recorder.Scripts.Data;
using Recorder.Scripts.Service;
using Recorder.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Recorder.Scripts.Gameplay
{
    public class RecordController : MonoBehaviour
    {
        [SerializeField] private CinemachineFreeLook _cineCam;
        [SerializeField] private Button _recordBtn;
        [SerializeField] private Button _replayMenuBtn;
        [SerializeField] private Transform _replayMenu;
        [SerializeField] private Transform _replayMenuParent;
        [SerializeField] private GameObject _replayItemPrefab;

        private bool _isRecording;
        private bool _isReplayMenuActive = true;
        private List<RecordedCamData> _records = new();

        private void OnEnable()
        {
            _recordBtn.onClick.AddListener(ChangeRecordState);
            _replayMenuBtn.onClick.AddListener(OpenReplayMenu);
        }

        private void OnDisable()
        {
            _recordBtn.onClick.RemoveAllListeners();
            _replayMenuBtn.onClick.RemoveAllListeners();
        }

        private void LateUpdate()
        {
            if (_isRecording)
            {
                _records.Add(new RecordedCamData(_cineCam.transform.position, _cineCam.m_YAxis.Value,
                    _cineCam.m_XAxis.Value));
            }
        }

        private void ChangeRecordState()
        {
            _isRecording = !_isRecording;
            if (_isRecording)
            {
                _records.Clear();
                StartCoroutine(RedIconIllumination());
            }
            else
            {
                var newRecord = new RecordListData
                {
                    recordedCamData = _records
                };

                SaveSystem.SaveRecord(newRecord);
            }
        }

        private IEnumerator RedIconIllumination()
        {
            while (_isRecording)
            {
                _recordBtn.image.enabled = (false);
                yield return new WaitForSeconds(0.2f);
                _recordBtn.image.enabled = (true);
                yield return new WaitForSeconds(0.2f);
            }
        }

        private void OpenReplayMenu()
        {
            _isReplayMenuActive = !_isReplayMenuActive;
            if (_isReplayMenuActive)
            {
                // _replayMenu.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, 1f * Time.deltaTime);
                _replayMenu.DOScale(0,.4f).OnComplete(() =>
                {
                    _replayMenu.gameObject.SetActive(false);
                });
                return;
            }

            _replayMenu.gameObject.SetActive(true);
            _replayMenu.DOScale(1, .4f);

            foreach (Transform uiObject in _replayMenuParent)
            {
                Destroy(uiObject.gameObject);
            }
            
            List<RecordListData> recorderReplays = SaveSystem.LoadRecords();
            int recorderReplaysCount = recorderReplays.Count;
            
            for (var i = 0; i < recorderReplaysCount; i++)
            {
                var record = recorderReplays[i];
                var recordGameObject = Instantiate(_replayItemPrefab, _replayMenuParent);
                
                recordGameObject.GetComponent<RecordedItemUI>().SetData(record.recName);
                recordGameObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    GameManager.Instance.SelectedRecordData = record;
                    GameManager.Instance.GoToLoadingScene(GameManager.EScenes.REPLAY_SCENE);
                });
            }

            _replayMenu.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, 1f * Time.deltaTime);
        }
    }
}