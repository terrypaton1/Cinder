using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField]
    private GameObject thankyouMessage;

    [SerializeField]
    private GameObject donateButton;

    [SerializeField]
    private UILabel versionText;

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
//			Debug.Log("showing main menu");
        var hasPlayerDonated = PlayerPrefs.GetInt(DataVariables.hasPlayerAlreadyDonated);
//		hasPlayerDonated =0;
//		Debug.Log("hasPlayerDonated:" + hasPlayerDonated);
        if (hasPlayerDonated > 0)
        {
            // show thank you for supporting the developer
            thankyouMessage.SetActive(true);
            donateButton.SetActive(false);
        }
        else
        {
// COMMENTING OUT THE DONATE BUTTON
//			donateButton.SetActive(true);
            thankyouMessage.SetActive(false);
        }

        versionText.text = "version " + CurrentBundleVersion.version;
//		Debug.Log(	versionText.text );
    }
}