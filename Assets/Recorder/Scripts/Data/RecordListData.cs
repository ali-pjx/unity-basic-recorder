using System;
using System.Collections.Generic;
using UnityEngine;

namespace Recorder.Scripts.Data
{
    [Serializable]
    public class RecordListData
    {
        [SerializeField] public string recName;
        [SerializeField] public List<RecordedCamData> recordedCamData = new();
    }
}