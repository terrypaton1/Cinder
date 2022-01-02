using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class BONUSManager : BaseObject
{
    [SerializeField]
    protected FallingLetter letterPrefab;

    private List<FallingLetter> fallingLettersPool = new List<FallingLetter>();
    private List<FallingLetter> fallingObjects = new List<FallingLetter>();
    private List<FallingLetter> collectedObjects = new List<FallingLetter>();

    [SerializeField]
    private List<BonusCharacter> letterAnimators;

    private IEnumerator coroutine;
    private const string word = "BRICK";
    private const string LetterCollectedAnimation = "LetterCollected";
    private const string LetterBonusCollectedAnimation = "LetterBonusCollected";
    private const string HideLetterAnimation = "HideLetter";

    public void RestartGame()
    {
        ResetFallingObjectsAvailable();
    }

    public void Setup()
    {
        fallingObjects = new List<FallingLetter>();
        collectedObjects = new List<FallingLetter>();
        fallingLettersPool = new List<FallingLetter>();

        for (var i = 0; i < 5; i++)
        {
            var fallingLetter = Instantiate(letterPrefab, transform, true);
            var letter = word.Substring(i, 1);

            fallingLetter.Setup(Points.fallingLetterCollectedPointValue, letter);
            fallingLettersPool.Add(fallingLetter);
        }

        HideAllLetters();
    }

    public void ResetFallingObjectsAvailable()
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
    }

    public bool BrickShouldDropPoints(Vector3 position)
    {
        if (fallingLettersPool.Count == 0)
        {
            return true;
        }

        // do a random check, if below x% then spawn a dropping letter
        if (Random.Range(0, 100) <= GameVariables.PercentChanceToDropBonusLetter)
        {
            var fallingLetter = GetRandomLetterFromPool();
            fallingLetter.StartFalling(position);
            return false;
        }

        //going to drop extra points … but lets only dpo that a percentage of the time
        if (Random.Range(0, 100) <= GameVariables.PercentChanceToDropPoints)
        {
            return true;
        }

        // not going to drop anything
        return false;
    }

    private FallingLetter GetRandomLetterFromPool()
    {
        var randomLetter = Random.Range(0, fallingLettersPool.Count);
        var fallingLetter = fallingLettersPool[randomLetter];
        fallingLettersPool.RemoveAt(randomLetter);
        fallingObjects.Add(fallingLetter);
        return fallingLetter;
    }

    public void LetterCollected(FallingLetter _fallingLetter)
    {
        collectedObjects.Add(_fallingLetter);
        fallingObjects.Remove(_fallingLetter);

        var letterValue = word.IndexOf(_fallingLetter.letter, StringComparison.Ordinal);

        Assert.IsTrue(letterAnimators.Count == 5,
            "letterAnimators must have references to the animators for b,o,n,u,s");
        var animatorRef = letterAnimators[letterValue];
        animatorRef.ShowCollected();

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

        yield return WaitCache.WaitForSeconds(0.5f);

        foreach (var letterAnimator in letterAnimators)
        {
            letterAnimator.PlayCollected();
        }

        yield return WaitCache.WaitForSeconds(1.5f);
        ResetFallingObjectsAvailable();
    }

    public void HideAllLetters()
    {
        foreach (var letterAnimator in letterAnimators)
        {
            letterAnimator.Hide();
        }
    }

    public void LetterWasNotCollected(FallingLetter _fallingLetter)
    {
        // THIS IS NOT WORKING CORRECTLY NEED TO DO IT A DIFFERENT WAY
        fallingLettersPool.Add(_fallingLetter);
        fallingObjects.Remove(_fallingLetter);
//		Debug.Log("adding the letter:" + _fallingLetter.letter + " back to the pool");
    }
}