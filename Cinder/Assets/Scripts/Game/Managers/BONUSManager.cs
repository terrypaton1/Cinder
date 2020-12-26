using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BONUSManager : BaseObject
{
    [SerializeField]
    protected FallingLetter letterPrefab;

    private List<string> lettersAvailable;
    private List<string> lettersCollected;
    public List<FallingLetter> fallingLettersPool;
    public List<FallingLetter> fallingObjects;
    public List<FallingLetter> collectedObjects;

    [SerializeField]
    protected Animator _sprite0Animator;

    [SerializeField]
    protected Animator _sprite1Animator;

    [SerializeField]
    protected Animator _sprite2Animator;

    [SerializeField]
    protected Animator _sprite3Animator;

    [SerializeField]
    protected Animator _sprite4Animator;

    private IEnumerator coroutine;
    private readonly string word = "BRICK";
    private readonly string LetterCollectedAnimation = "LetterCollected";
    private readonly string LetterBonusCollectedAnimation = "LetterBonusCollected";
    private readonly string HideLetterAnimation = "HideLetter";

    public void RestartGame()
    {
        ResetFallingObjectsAvailable();
    }

    public void Setup()
    {
        fallingObjects = new List<FallingLetter>();
        collectedObjects = new List<FallingLetter>();
        fallingLettersPool = new List<FallingLetter>();
        for (int i = 0; i < 5; i++)
        {
            var fallingLetter = Instantiate(letterPrefab, transform, true);
            fallingLettersPool.Add(fallingLetter);

            var letter = word.Substring(i, 1);
            fallingLetter.Setup(Points.fallingLetterCollectedPointValue, letter);
        }

        HideAllLetters();
    }

    private void ResetFallingObjectsAvailable()
    {
        foreach (var fallingLetter in fallingObjects)
        {
            fallingLettersPool.Add(fallingLetter);
        }

        foreach (var fallingLetter in collectedObjects)
        {
            fallingLettersPool.Add(fallingLetter);
        }

        fallingObjects = new List<FallingLetter>();
        collectedObjects = new List<FallingLetter>();

        HideAllLetters();
    }

    public bool BrickShouldDropPoints(Vector3 position)
    {
        if (fallingLettersPool.Count == 0)
        {
            return true;
        }

        // do a random check, if below x% then spawn a dropping letter
        if (Random.Range(0, 100) <= GameVariables.percentChanceToDropBONUSLetter)
        {
            var fallingLetter = GetRandomLetterFromPool();
            fallingLetter.StartFalling(position);
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

    private FallingLetter GetRandomLetterFromPool()
    {
        int randomLetter = Random.Range(0, fallingLettersPool.Count);
        FallingLetter fallingLetter = fallingLettersPool[randomLetter];
        fallingLettersPool.RemoveAt(randomLetter);
        fallingObjects.Add(fallingLetter);
        return fallingLetter;
    }

    public void LetterCollected(FallingLetter _fallingLetter)
    {
        collectedObjects.Add(_fallingLetter);
        fallingObjects.Remove(_fallingLetter);
        
        var letterValue = word.IndexOf(_fallingLetter.letter, StringComparison.Ordinal);

        switch (letterValue)
        {
            case 0:
                _sprite0Animator.Play(LetterCollectedAnimation);
                break;
            case 1:
                _sprite1Animator.Play(LetterCollectedAnimation);
                break;
            case 2:
                _sprite2Animator.Play(LetterCollectedAnimation);
                break;
            case 3:
                _sprite3Animator.Play(LetterCollectedAnimation);
                break;
            case 4:
                _sprite4Animator.Play(LetterCollectedAnimation);
                break;
        }

        if (collectedObjects.Count >= 5)
        {
            StartAllLettersCollected();
        }
        else
        {
            PlaySound(SoundList.BONUSLetterCollected);
            // animate the letter collected
        }
    }

    private void StartAllLettersCollected()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = AllLettersCollected();
        StartCoroutine(coroutine);
    }

    private IEnumerator AllLettersCollected()
    {
        ShowInGameMessage(Message.BonusPoints);
        PlaySound(SoundList.AllBONUSLettersCollected);

        CoreConnector.GameManager.scoreManager.PointsCollected(Points.AllLettersOfBONUSCollectedPoints);
        // now we need to reset the objects for it to start over again

        yield return new WaitForSeconds(0.5f);
        _sprite0Animator.Play(LetterBonusCollectedAnimation);
        _sprite1Animator.Play(LetterBonusCollectedAnimation);
        _sprite2Animator.Play(LetterBonusCollectedAnimation);
        _sprite3Animator.Play(LetterBonusCollectedAnimation);
        _sprite4Animator.Play(LetterBonusCollectedAnimation);

        yield return new WaitForSeconds(1.5f);
        ResetFallingObjectsAvailable();
        HideAllLetters();
    }

    private void HideAllLetters()
    {
        _sprite0Animator.Play(HideLetterAnimation);
        _sprite1Animator.Play(HideLetterAnimation);
        _sprite2Animator.Play(HideLetterAnimation);
        _sprite3Animator.Play(HideLetterAnimation);
        _sprite4Animator.Play(HideLetterAnimation);
    }

    public void LetterWasNotCollected(FallingLetter _fallingLetter)
    {
        // THIS IS NOT WORKING CORRECTLY NEED TO DO IT A DIFFERENT WAY
        fallingLettersPool.Add(_fallingLetter);
        fallingObjects.Remove(_fallingLetter);
//		Debug.Log("adding the letter:" + _fallingLetter.letter + " back to the pool");
    }
}