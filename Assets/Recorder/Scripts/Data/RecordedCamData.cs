using System;
using UnityEngine;

namespace Recorder.Scripts.Data
{
    [Serializable]
    public class RecordedCamData
    {
        [SerializeField] public Vector3 rPosition;
        [SerializeField] public float yAxis;
        [SerializeField] public float xAxis;

        public RecordedCamData(Vector3 pos, float yValue, float xValue)
        {
            rPosition = pos;
            yAxis = yValue;
            xAxis = xValue;
        }
    }
}