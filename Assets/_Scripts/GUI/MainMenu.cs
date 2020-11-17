using UnityEngine;

public class MainMenu : MonoBehaviour
{
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
        var maxLevelBeaten = PlayerPrefs.GetInt(DataVariables.maxLevelBeatenPrefix);
    }
}