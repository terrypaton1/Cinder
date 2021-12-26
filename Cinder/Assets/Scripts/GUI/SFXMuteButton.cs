using UnityEngine;

public class SFXMuteButton : MonoBehaviour
{
    [SerializeField]
    protected GameObject onGraphic;

    [SerializeField]
    protected GameObject offGraphic;

    public void AudioButtonPressed()
    {
        CoreConnector.GameManager.gameVariables.sfxEnabled =
            CoreConnector.GameManager.gameVariables.sfxEnabled == 0 ? 1 : 0;

        PlayerPrefs.SetInt(Constants.SfxEnabled, CoreConnector.GameManager.gameVariables.sfxEnabled);
        EvaluateSFXState();
    }

    private void EvaluateSFXState()
    {
        if (CoreConnector.GameManager.gameVariables.sfxEnabled == 0)
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