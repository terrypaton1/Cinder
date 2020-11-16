using UnityEngine;

public class SFXMuteButton : MonoBehaviour
{
    [SerializeField]
    protected GameObject onGraphic;

    [SerializeField]
    protected GameObject offGraphic;

    private Animator _animator;

    protected void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected void OnEnable()
    {
        EvaluateSFXState();
    }

    protected void OnClick()
    {
//		Debug.Log(_animator);
        if (_animator != null)
        {
            _animator.Play("ButtonPressAnimation");
        }

        if (GameVariables.instance.SFXEnabled == 0)
        {
            GameVariables.instance.SFXEnabled = 1;
        }
        else
        {
            GameVariables.instance.SFXEnabled = 0;
        }

        PlayerPrefs.SetInt(DataVariables.SFXEnabled, GameVariables.instance.SFXEnabled);
        EvaluateSFXState();
    }

    private void EvaluateSFXState()
    {
        if (GameVariables.instance.SFXEnabled == 0)
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