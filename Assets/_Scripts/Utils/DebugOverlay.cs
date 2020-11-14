using UnityEngine;

public class DebugOverlay : MonoBehaviour
{
    [SerializeField]
    UILabel debugText;

    string textToDisplay = "";

    bool debugWasEnabled = false;

    protected void Awake()
    {
        if (GameVariables.displayDebugOverlay)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    protected void OnEnable()
    {
        if (GameVariables.displayDebugOverlay)
        {
            debugWasEnabled = true;
            Messenger<string>.AddListener(GlobalEvents.DebugString, DebugDisplay);
        }
    }

    protected void OnDisable()
    {
        if (debugWasEnabled)
            Messenger<string>.RemoveListener(GlobalEvents.DebugString, DebugDisplay);
    }

    void DebugDisplay(string _text)
    {
        textToDisplay += _text + "\n";
        debugText.text = textToDisplay;
    }
}