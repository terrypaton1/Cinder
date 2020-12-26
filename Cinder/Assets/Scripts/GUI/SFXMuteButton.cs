using UnityEngine;

public class SFXMuteButton : MonoBehaviour
{
    //todo convert these to sprite renderers
    [SerializeField]
    protected Animator _animator;

    [SerializeField]
    protected GameObject onGraphic;

    [SerializeField]
    protected GameObject offGraphic;

    public void AudioButtonPressed()
    {
        _animator.Play("ButtonPressAnimation");

        if (CoreConnector.GameManager.gameVariables.SFXEnabled == 0)
        {
            CoreConnector.GameManager.gameVariables.SFXEnabled = 1;
        }
        else
        {
            CoreConnector.GameManager.gameVariables.SFXEnabled = 0;
        }

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