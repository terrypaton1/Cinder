#region

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#endregion

public class BONUSManager : BaseObject {
	/// <summary>
	/// The letter prefab.
	/// </summary>
	[SerializeField] FallingLetter letterPrefab;

	/// <summary>
	/// The falling letters pool.
	/// </summary>
	public List<FallingLetter> fallingLettersPool;

	/// <summary>
	/// The falling objects.
	/// </summary>
	public List<FallingLetter> fallingObjects;

	/// <summary>
	/// The collected objects.
	/// </summary>
	public 	List<FallingLetter> collectedObjects;

	/// <summary>
	/// The sprite0.
	/// </summary>
	[SerializeField]protected SpriteRenderer _sprite0;

	/// <summary>
	/// The sprite1.
	/// </summary>
	[SerializeField]protected SpriteRenderer _sprite1;

	/// <summary>
	/// The sprite2.
	/// </summary>
	[SerializeField]protected SpriteRenderer _sprite2;

	/// <summary>
	/// The sprite3.
	/// </summary>
	[SerializeField]protected SpriteRenderer _sprite3;

	/// <summary>
	/// The sprite4.
	/// </summary>
	[SerializeField]protected SpriteRenderer _sprite4;

	/// <summary>
	/// The sprite0 animator.
	/// </summary>
	[SerializeField] Animator _sprite0Animator;

	/// <summary>
	/// The sprite1 animator.
	/// </summary>
	[SerializeField]protected  Animator _sprite1Animator;

	/// <summary>
	/// The sprite2 animator.
	/// </summary>
	[SerializeField] protected Animator _sprite2Animator;

	/// <summary>
	/// The sprite3 animator.
	/// </summary>
	[SerializeField] Animator _sprite3Animator;

	/// <summary>
	/// The sprite4 animator.
	/// </summary>
	[SerializeField] Animator _sprite4Animator;

	/// <summary>
	/// The color of the invisible letter.
	/// </summary>
	public Color invisibleLetterColor = new Color(1, 1, 1, 0);

	/// <summary>
	/// The letters available.
	/// </summary>
	List<string> lettersAvailable;

	/// <summary>
	/// The letters collected.
	/// </summary>
	List<string> lettersCollected;

	/// <summary>
	/// The letters collected for debug.
	/// </summary>
	string lettersCollectedForDebug = "";

	/// <summary>
	/// The word.
	/// </summary>
	readonly string word = "BRICK";

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable() {
		//create pool of falling letters
		fallingLettersPool = new List<FallingLetter>();
		fallingObjects = new List<FallingLetter>();
		collectedObjects = new List<FallingLetter>();
		for (var i = 0; i < 5; i++) {
			fallingLettersPool.Add(Instantiate(letterPrefab));
			fallingLettersPool [i].Setup(GameVariables.fallingLetterCollectedPointValue, word.Substring(i, 1));
			fallingLettersPool [i].transform.parent = transform;
		}
//		ResetLettersAvailable();
		HideAllLetters();
	}

	/// <summary>
	/// Resets the letters available.
	/// </summary>
	void ResetLettersAvailable() {
		// remove all the letters from the 
		for (var i = 0; i < fallingObjects.Count; i++) {
			fallingLettersPool.Add(fallingObjects [i]);
		}
		for (var i = 0; i < collectedObjects.Count; i++) {
			fallingLettersPool.Add(collectedObjects [i]);
		}
		fallingObjects = new List<FallingLetter>();
		collectedObjects = new List<FallingLetter>();
		Debug.Log("fallingLettersPool.Count:" + fallingLettersPool.Count);

	}

	/// <summary>
	/// Bricks the should drop points. Returns true if it should, if false, this instance is dropping a letter
	/// </summary>
	/// <returns><c>true</c>, if should drop points was bricked, <c>false</c> otherwise.</returns>
	/// <param name="position">Position.</param>
	public bool BrickShouldDropPoints(Vector3 position) {
		// no letters available
		if (fallingLettersPool.Count == 0)
			return true;
		// do a random check, if below x% then spawn a dropping letter
		if (Random.Range(0, 100) <= GameVariables.percentChanceToDropBONUSLetter) {
			//
			var randomLetter = Random.Range(0, fallingLettersPool.Count);

			// falling letters should fall faster
			var _fallingLetter = fallingLettersPool [randomLetter];
//		Debug.Log("letterToDrop:" + _fallingLetter.letter);

			fallingLettersPool.RemoveAt(randomLetter);
//		Debug.Log("fallingLettersPool remaining:" + fallingLettersPool.Count);
			fallingObjects.Add(_fallingLetter);
			_fallingLetter.StartFalling(position);
			return false;
		}
		//going to drop extra points … but lets only dpo that a percentage of the time
		if (Random.Range(0, 100) <= GameVariables.percentChanceToDropPoints) {

		return true;
		}
		// not going to drop anything
		return false;
	}

	/// <summary>
	/// Letters the collected.
	/// </summary>
	/// <param name="_fallingLetter">Falling letter.</param>
	public void LetterCollected(FallingLetter _fallingLetter) {

//		Debug.Log("LetterCollected:" + _fallingLetter.letter);
//		 check if the player has collected all the letters
		collectedObjects.Add(_fallingLetter);
		fallingObjects.Remove(_fallingLetter);
		lettersCollectedForDebug += _fallingLetter.letter;
//		Debug.Log("lettersCollectedForDebug:" + lettersCollectedForDebug);
		var letterValue = word.IndexOf(_fallingLetter.letter);

		switch (letterValue) {
			case 0:
				_sprite0Animator.Play("LetterCollected");
				break;
			case 1:
				_sprite1Animator.Play("LetterCollected");
				break;
			case 2:
				_sprite2Animator.Play("LetterCollected");
				break;
			case 3:
				_sprite3Animator.Play("LetterCollected");
				break;
			case 4:
				_sprite4Animator.Play("LetterCollected");
				break;
		}
		if (collectedObjects.Count >= 5) {
			StartCoroutine(AllLettersCollected());

		} else {
			PlaySound(SoundList.BONUSLetterCollected);
			// animate the letter collected

		}
	}

	/// <summary>
	/// All letters collected.
	/// </summary>
	/// <returns>The letters collected.</returns>
	IEnumerator AllLettersCollected() {
//		Debug.Log("COLLECTED ALL LETTERS!!");
		ShowInGameMessage("BONUS POINTS!!");
		PlaySound(SoundList.AllBONUSLettersCollected);
		Messenger<int>.Broadcast(GlobalEvents.PointsCollected, GameVariables.AllLettersOfBONUSCollectedPoints, MessengerMode.DONT_REQUIRE_LISTENER);
		// now we need to reset the objects for it to start over again

		yield return new WaitForSeconds(.5f);
		_sprite0Animator.Play("LetterBonusCollected");
		_sprite1Animator.Play("LetterBonusCollected");
		_sprite2Animator.Play("LetterBonusCollected");
		_sprite3Animator.Play("LetterBonusCollected");
		_sprite4Animator.Play("LetterBonusCollected");

		yield return new WaitForSeconds(1.5f);
		ResetLettersAvailable();
		HideAllLetters();
	}

	/// <summary>
	/// Hides all letters.
	/// </summary>
	void HideAllLetters() {
		_sprite0Animator.Play("HideLetter");
		_sprite1Animator.Play("HideLetter");
		_sprite2Animator.Play("HideLetter");
		_sprite3Animator.Play("HideLetter");
		_sprite4Animator.Play("HideLetter");
	}

	/// <summary>
	/// Letters the was not collected.
	/// </summary>
	/// <param name="letter">Letter.</param>
	public void LetterWasNotCollected(FallingLetter _fallingLetter) {
		// a dropping letter was not collected, it fell off the bottom of the screen. Add if back to the pool list
		// THIS IS NOT WORKING CORRECTLY NEED TO DO IT A DIFFERENT WAY
		fallingLettersPool.Add(_fallingLetter);
		fallingObjects.Remove(_fallingLetter);
//		Debug.Log("adding the letter:" + _fallingLetter.letter + " back to the pool");
	}

	#region instance

	void OnDestroy() {	
		s_Instance = null;
	}

	// ************************************
	// s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
	// ************************************
	private static BONUSManager s_Instance;

	// ************************************
	// This defines a static instance property that attempts to find the manager object in the scene and
	// returns it to the caller.
	// ************************************
	public static BONUSManager instance {
		get {
			if (s_Instance == null) {
				// This is where the magic happens.
				//  FindObjectOfType(...) returns the first AManager object in the scene.
				s_Instance = FindObjectOfType(typeof(BONUSManager)) as BONUSManager;
			}

			// If it is still null, create a new instance
			if (s_Instance == null) {
				Debug.LogError("Could not locate an BONUSManager object!");
				Debug.Break();
			}

			return s_Instance;
		}
	}

	// ************************************
	// Ensure that the instance is destroyed when the game is stopped in the editor.
	// ************************************
	void OnApplicationQuit() {
		s_Instance = null;
	}

	#endregion
}
