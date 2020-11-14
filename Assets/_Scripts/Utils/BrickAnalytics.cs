using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class BrickAnalytics : MonoBehaviour {
	public static string changeScene = "ChangeScene";
	public static string levelComplete = "LevelComplete";
	public static string gameOver = "GameOver";
	public static string gamePaused = "GamePaused";
	public static string gameRestarted = "GameRestarted";
	public static string gameQuit = "GameQuit";
	public static string donateButtonPressed = "DonateButtonPressed";
	public static string GameComplete = "GameComplete";

	void OnEnable() {
		Messenger<string,string>.AddListener(GlobalEvents.AnalyticsEvent, LogEvent);
	}

	void OnDisable() {
		Messenger<string,string>.RemoveListener(GlobalEvents.AnalyticsEvent, LogEvent);
	}

	void LogEvent(string customEventName, string value) {
//	Debug.Log("LOG EVENT");
		Analytics.CustomEvent(customEventName, new Dictionary<string, object> {
			{ customEventName, value }
		});
	}
}
