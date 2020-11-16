using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    protected GameObject donateButton;

    protected void OnEnable()
    {
        Messenger.AddListener(GlobalEvents.PurchaseMade, EvaluateDisplay);
        EvaluateDisplay();
    }

    protected void OnDisable()
    {
        Messenger.RemoveListener(GlobalEvents.PurchaseMade, EvaluateDisplay);
    }

    private void EvaluateDisplay()
    {
//		Debug.Log("showing main menu");
        // has the player already donated to the developer?
        var hasPlayerDonated = PlayerPrefs.GetInt(DataVariables.hasPlayerAlreadyDonated);
//		hasPlayerDonated =0;
//		Debug.Log("hasPlayerDonated:"+hasPlayerDonated);
        if (hasPlayerDonated > 0)
        {
            // show thank you for supporting the developer
            donateButton.SetActive(false);
        }
        else
        {
            var maxLevelBeaten = PlayerPrefs.GetInt(DataVariables.maxLevelBeatenPrefix);
//			maxLevelBeaten = 5;
//			// has the game been played more than 5 times?
//			Debug.Log("maxLevelBeaten:"+maxLevelBeaten);
            if (maxLevelBeaten >= 10)
            {
                // only show the donate button 40% of the time
                if (Random.Range(0, 10) < 4)
                {
//					Debug.Log("Show donate button");
// COMMENTING OUT THE DONATE BUTTON
                    //donateButton.SetActive(true);
                }
                else
                {
                    donateButton.SetActive(false);
                }
            }
            else
            {
                donateButton.SetActive(false);
            }
        }
    }
}