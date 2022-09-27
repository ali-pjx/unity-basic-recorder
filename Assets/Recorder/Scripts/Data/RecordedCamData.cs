using UnityEngine;

namespace Recorder.Scripts.Data
{
    public class RecordedCamData
    {
        public Vector3 rPosition;
        public float yAxis;
        public float xAxis;

        public RecordedCamData(Vector3 pos, float yValue, float xValue)
        {
            rPosition = pos;
            yAxis = yValue;
            xAxis = xValue;
        }
    }
}