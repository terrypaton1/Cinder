using UnityEngine;

public class SFXMuteButton : MonoBehaviour
{
    [SerializeField]
    protected GameObject onGraphic;

    [SerializeField]
    protected GameObject offGraphic;

    public void AudioButtonPressed()
    {
        if (CoreConnector.GameManager.gameVariables.sfxEnabled != 0)
            CoreConnector.GameManager.gameVariables.sfxEnabled = 0;
        else
            CoreConnector.GameManager.gameVariables.sfxEnabled = 1;

        PlayerPrefs.SetInt(Constants.SfxEnabled, CoreConnector.GameManager.gameVariables.sfxEnabled);
        EvaluateSFXState();
    }

    private void EvaluateSFXState()
    {
        Debug.Log(CoreConnector.GameManager.gameVariables.sfxEnabled);
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