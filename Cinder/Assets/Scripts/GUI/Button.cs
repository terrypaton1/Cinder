using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

public class Button : BaseObject
{
    [SerializeField]
    protected Animator _animator;

    [SerializeField, FormerlySerializedAs("this_button")]
    public Buttons
        thisButtonID;

    protected void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private IEnumerator EvaluateButtonPress()
    {
        if (_animator != null)
        {
            _animator.Play("ButtonPressAnimation");
        }

        yield return new WaitForSeconds(.3f);
        switch (thisButtonID)
        {
            case Buttons.QuitGame:
                Debug.Log("QuitGame");
#if !UNITY_EDITOR
				Application.Quit();
#endif
                break;
        }
    }

    private void OnClick()
    {
        StartCoroutine(EvaluateButtonPress());
    }
}

public enum Buttons
{
    MainMenu = 10,
    Credits = 20,
    LevelChooser = 30,
    Game = 40,
    RestartLevel = 50,
    PauseGame = 60,
    ResumeGame = 70,
    QuitGame = 80,
    NextLevel = 100,
}