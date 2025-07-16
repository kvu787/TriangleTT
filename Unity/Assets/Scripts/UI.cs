using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DrivingGameV2 {
    public class UI : MonoBehaviour {
        void OnEnable() {
            string debugLogFilePath = $"{Application.persistentDataPath}/Player.log".Replace("/", "\\");
            string prefix = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff");
            string lapTimesFilePath = $"{Application.persistentDataPath}/{prefix} LapTimes.json".Replace("/", "\\");
            string text =
                $"Debug log:\n" +
                $"{debugLogFilePath}\n" +
                $"\n" +
                $"Lap times:\n" +
                $"{lapTimesFilePath}";
            this.GetComponent<UIDocument>().rootVisualElement.Q<Label>("topLeftText").text = text;
        }
    }
}
