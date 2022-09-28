using System.Collections.Generic;
using System.IO;
using Recorder.Scripts.Data;
using UnityEngine;

namespace Recorder.Scripts.Service
{
    public static class SaveSystem
    {
        private static readonly string RecordDataPath = Application.streamingAssetsPath + "/Rec/";

        public static void Initialize()
        {
            if (!Directory.Exists(RecordDataPath)) Directory.CreateDirectory(RecordDataPath);
        }

        public static void SaveRecord(RecordListData data)
        {
            if (!File.Exists(RecordDataPath + $"{data.recName}.json"))
            {
                int dataIndex = 1;
                while (File.Exists(RecordDataPath + $"rec_{dataIndex}.json"))
                {
                    dataIndex++;
                }

                data.recName = $"rec_{dataIndex}";
            }

            string saveData = JsonUtility.ToJson(data);
            File.WriteAllText(RecordDataPath + $"{data.recName}.json", saveData);
        }

        public static List<RecordListData> LoadRecords()
        {
            List<RecordListData> allRecs = new();
            foreach (var datum in Directory.EnumerateFiles(RecordDataPath, "*.json"))
            {
                allRecs.Add(JsonUtility.FromJson<RecordListData>(File.ReadAllText(datum)));
            }

            return allRecs;
        }
    }
}
