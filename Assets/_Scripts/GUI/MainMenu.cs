#region

using UnityEngine;

#endregion

public class MainMenu : MonoBehaviour
{
	/// The donate button.
	/// </summary>
	[SerializeField] GameObject donateButton;

	void OnEnable ()
	{
		Messenger.AddListener (GlobalEvents.PurchaseMade, EvaluateDisplay);
		EvaluateDisplay ();
	}

	void OnDisable ()
	{
		Messenger.RemoveListener (GlobalEvents.PurchaseMade, EvaluateDisplay);
	}

	void EvaluateDisplay ()
	{
//		Debug.Log("showing main menu");
		// has the player already donated to the developer?
		var hasPlayerDonated = PlayerPrefs.GetInt (DataVariables.hasPlayerAlreadyDonated);
//		hasPlayerDonated =0;
//		Debug.Log("hasPlayerDonated:"+hasPlayerDonated);
		if (hasPlayerDonated > 0) {
			// show thank you for supporting the developer
			donateButton.SetActive (false);
		} else {
			var maxLevelBeaten = PlayerPrefs.GetInt (DataVariables.maxLevelBeatenPrefix);
//			maxLevelBeaten = 5;
//			// has the game been played more than 5 times?
//			Debug.Log("maxLevelBeaten:"+maxLevelBeaten);
			if (maxLevelBeaten >= 10) {
				// only show the donate button 40% of the time
				if (Random.Range (0, 10) < 4) {
//					Debug.Log("Show donate button");
// COMMENTING OUT THE DONATE BUTTON
					//donateButton.SetActive(true);

				} else {
					donateButton.SetActive (false);
				}
			} else {
				donateButton.SetActive (false);
			}
		}
	}
}
