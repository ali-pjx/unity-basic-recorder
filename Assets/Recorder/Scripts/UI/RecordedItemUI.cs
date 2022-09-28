using TMPro;
using UnityEngine;

namespace Recorder.Scripts.UI
{
    public class RecordedItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _recordNameTxt;

        public void SetData(string nameString)
        {
            _recordNameTxt.text = nameString;
        }
    }
}
