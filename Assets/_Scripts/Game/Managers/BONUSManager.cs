using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BONUSManager : BaseObject
{
    [SerializeField]
    FallingLetter letterPrefab;

    public List<FallingLetter> fallingLettersPool;

    public List<FallingLetter> fallingObjects;

    public List<FallingLetter> collectedObjects;

    [SerializeField]
    protected SpriteRenderer _sprite0;

    [SerializeField]
    protected SpriteRenderer _sprite1;

    [SerializeField]
    protected SpriteRenderer _sprite2;

    [SerializeField]
    protected SpriteRenderer _sprite3;

    [SerializeField]
    protected SpriteRenderer _sprite4;

    [SerializeField]
    Animator _sprite0Animator;

    [SerializeField]
    protected Animator _sprite1Animator;

    [SerializeField]
    protected Animator _sprite2Animator;

    [SerializeField]
    protected Animator _sprite3Animator;

    [SerializeField]
    protected Animator _sprite4Animator;

    public Color invisibleLetterColor = new Color(1, 1, 1, 0);

    private List<string> lettersAvailable;

    private List<string> lettersCollected;

    private string lettersCollectedForDebug = "";

    private readonly string word = "BRICK";
    private const string letterBonusCollected = "LetterBonusCollected";
    private const string letterCollected = "LetterCollected";
    private const string HideLetter = "HideLetter";

    protected void OnEnable()
    {
        //create pool of falling letters
        fallingLettersPool = new List<FallingLetter>();
        fallingObjects = new List<FallingLetter>();
        collectedObjects = new List<FallingLetter>();
        for (var i = 0; i < 5; i++)
        {
            fallingLettersPool.Add(Instantiate(letterPrefab));
            fallingLettersPool[i].Setup(GameVariables.fallingLetterCollectedPointValue, word.Substring(i, 1));
            fallingLettersPool[i].transform.parent = transform;
        }

        HideAllLetters();
    }

    private void ResetLettersAvailable()
    {
        // remove all the letters from the 
        for (var i = 0; i < fallingObjects.Count; i++)
        {
            fallingLettersPool.Add(fallingObjects[i]);
        }

        for (var i = 0; i < collectedObjects.Count; i++)
        {
            fallingLettersPool.Add(collectedObjects[i]);
        }

        fallingObjects = new List<FallingLetter>();
        collectedObjects = new List<FallingLetter>();
        Debug.Log("fallingLettersPool.Count:" + fallingLettersPool.Count);
    }

    public bool BrickShouldDropPoints(Vector3 position)
    {
        // no letters available
        if (fallingLettersPool.Count == 0)
            return true;
        // do a random check, if below x% then spawn a dropping letter
        if (Random.Range(0, 100) <= GameVariables.percentChanceToDropBONUSLetter)
        {
            //
            var randomLetter = Random.Range(0, fallingLettersPool.Count);

            // falling letters should fall faster
            var _fallingLetter = fallingLettersPool[randomLetter];
//		Debug.Log("letterToDrop:" + _fallingLetter.letter);

            fallingLettersPool.RemoveAt(randomLetter);
//		Debug.Log("fallingLettersPool remaining:" + fallingLettersPool.Count);
            fallingObjects.Add(_fallingLetter);
            _fallingLetter.StartFalling(position);
            return false;
        }

        //going to drop extra points … but lets only dpo that a percentage of the time
        if (Random.Range(0, 100) <= GameVariables.percentChanceToDropPoints)
        {
            return true;
        }

        // not going to drop anything
        return false;
    }

    public void LetterCollected(FallingLetter _fallingLetter)
    {
//		Debug.Log("LetterCollected:" + _fallingLetter.letter);
//		 check if the player has collected all the letters
        collectedObjects.Add(_fallingLetter);
        fallingObjects.Remove(_fallingLetter);
        lettersCollectedForDebug += _fallingLetter.letter;
//		Debug.Log("lettersCollectedForDebug:" + lettersCollectedForDebug);
        var letterValue = word.IndexOf(_fallingLetter.letter);

        switch (letterValue)
        {
            case 0:
                _sprite0Animator.Play(letterCollected);
                break;
            case 1:
                _sprite1Animator.Play(letterCollected);
                break;
            case 2:
                _sprite2Animator.Play(letterCollected);
                break;
            case 3:
                _sprite3Animator.Play(letterCollected);
                break;
            case 4:
                _sprite4Animator.Play(letterCollected);
                break;
        }

        if (collectedObjects.Count >= 5)
        {
            StartCoroutine(AllLettersCollected());
        }
        else
        {
            PlaySound(SoundList.BONUSLetterCollected);
            // animate the letter collected
        }
    }

    private IEnumerator AllLettersCollected()
    {
//		Debug.Log("COLLECTED ALL LETTERS!!");
        ShowInGameMessage("BONUS POINTS!!");
        PlaySound(SoundList.AllBONUSLettersCollected);
        Messenger<int>.Broadcast(GlobalEvents.PointsCollected, GameVariables.AllLettersOfBONUSCollectedPoints,
            MessengerMode.DONT_REQUIRE_LISTENER);
        // now we need to reset the objects for it to start over again

        yield return new WaitForSeconds(.5f);
        _sprite0Animator.Play(letterBonusCollected);
        _sprite1Animator.Play(letterBonusCollected);
        _sprite2Animator.Play(letterBonusCollected);
        _sprite3Animator.Play(letterBonusCollected);
        _sprite4Animator.Play(letterBonusCollected);

        yield return new WaitForSeconds(1.5f);
        ResetLettersAvailable();
        HideAllLetters();
    }

    private void HideAllLetters()
    {
        _sprite0Animator.Play(HideLetter);
        _sprite1Animator.Play(HideLetter);
        _sprite2Animator.Play(HideLetter);
        _sprite3Animator.Play(HideLetter);
        _sprite4Animator.Play(HideLetter);
    }

    public void LetterWasNotCollected(FallingLetter _fallingLetter)
    {
        // a dropping letter was not collected, it fell off the bottom of the screen. Add if back to the pool list
        // THIS IS NOT WORKING CORRECTLY NEED TO DO IT A DIFFERENT WAY
        fallingLettersPool.Add(_fallingLetter);
        fallingObjects.Remove(_fallingLetter);
//		Debug.Log("adding the letter:" + _fallingLetter.letter + " back to the pool");
    }

    protected void OnDestroy()
    {
        s_Instance = null;
    }

    private static BONUSManager s_Instance;

    public static BONUSManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(BONUSManager)) as BONUSManager;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
                Debug.LogError("Could not locate an BONUSManager object!");
                Debug.Break();
            }

            return s_Instance;
        }
    }

    protected void OnApplicationQuit()
    {
        s_Instance = null;
    }
}