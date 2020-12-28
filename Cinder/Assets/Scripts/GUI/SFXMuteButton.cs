using UnityEngine;

public class SFXMuteButton : MonoBehaviour
{
    [SerializeField]
    protected GameObject onGraphic;

    [SerializeField]
    protected GameObject offGraphic;

    public void AudioButtonPressed()
    {
        CoreConnector.GameManager.gameVariables.SFXEnabled =
            CoreConnector.GameManager.gameVariables.SFXEnabled == 0 ? 1 : 0;

        PlayerPrefs.SetInt(DataVariables.SFXEnabled, CoreConnector.GameManager.gameVariables.SFXEnabled);
        EvaluateSFXState();
    }

    private void EvaluateSFXState()
    {
        if (CoreConnector.GameManager.gameVariables.SFXEnabled == 0)
        {
            onGraphic.SetActive(false);
            offGraphic.SetActive(true);
        }
        else
        {
            onGraphic.SetActive(true);
            offGraphic.SetActive(false);
        }
    }
}