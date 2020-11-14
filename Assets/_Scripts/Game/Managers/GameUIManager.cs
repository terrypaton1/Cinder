#region

using UnityEngine;

#endregion

public class GameUIManager : MonoBehaviour {
	/// <summary> The buttons holder. </summary>
	[SerializeField]
	UIWidget buttonsHolder;

	/// <summary> The pause menu holder. </summary>
	[SerializeField]
	UIWidget pauseMenuHolder;

	/// <summary> The game over menu holder. </summary>
	[SerializeField]
	UIWidget GameOverMenuHolder;

	/// <summary> The level complete menu holder. </summary>
	[SerializeField]
	UIWidget LevelCompleteMenuHolder;

	/// <summary> The game complete menu holder. </summary>
	[SerializeField]
	UIWidget GameCompleteMenuHolder;

	/// <summary> The in game menu buttons animator. </summary>
	[SerializeField] Animator inGameMenuButtonsAnimator;

	void Awake() {
		HideAllMenus();
	}

	/// <summary>
	/// Add listeners
	/// </summary>
	void OnEnable() {
//		DontDestroyOnLoad(gameObject);
		Messenger<bool>.AddListener(MenuEvents.DisplayInGameButtons, DisplayInGameButtons);
		Messenger<bool>.AddListener(MenuEvents.DisplayPauseMenu, DisplayPauseMenu);
		Messenger<bool>.AddListener(MenuEvents.DisplayLevelComplete, DisplayLevelComplete);
		Messenger<bool>.AddListener(MenuEvents.DisplayGameComplete, DisplayGameComplete);
		Messenger<bool>.AddListener(MenuEvents.DisplayGameOver, DisplayGameOver);
		Messenger.AddListener(MenuEvents.RestartGame, RestartGame);
		Messenger.AddListener(GlobalEvents.PauseGame, PauseGame);
		Messenger.AddListener(GlobalEvents.ResumeGame, ResumeGame);
		Messenger.AddListener(GlobalEvents.HideAllMenus, HideAllMenus);
		Messenger<bool>.AddListener(GlobalEvents.LoginResult, LoginResult);



	}

	/// <summary>
	/// Remove listeners
	/// </summary>
	void OnDisable() {
		Messenger<bool>.RemoveListener(MenuEvents.DisplayInGameButtons, DisplayInGameButtons);
		Messenger<bool>.RemoveListener(MenuEvents.DisplayPauseMenu, DisplayPauseMenu);
		Messenger<bool>.RemoveListener(MenuEvents.DisplayLevelComplete, DisplayLevelComplete);
		Messenger<bool>.RemoveListener(MenuEvents.DisplayGameComplete, DisplayGameComplete);
		Messenger<bool>.RemoveListener(MenuEvents.DisplayGameOver, DisplayGameOver);
		Messenger.RemoveListener(MenuEvents.RestartGame, RestartGame);
		Messenger.RemoveListener(GlobalEvents.PauseGame, PauseGame);
		Messenger.RemoveListener(GlobalEvents.ResumeGame, ResumeGame);
		Messenger.RemoveListener(GlobalEvents.HideAllMenus, HideAllMenus);
		Messenger<bool>.RemoveListener(GlobalEvents.LoginResult, LoginResult);
	}

	void HideAllMenus() {
		LevelCompleteMenuHolder.gameObject.SetActive(false);		
		buttonsHolder.gameObject.SetActive(false);			
		pauseMenuHolder.gameObject.SetActive(false);
		GameOverMenuHolder.gameObject.SetActive(false);
		GameCompleteMenuHolder.gameObject.SetActive(false);
	}

	/// <summary>
	/// Displaies the game complete.
	/// </summary>
	void DisplayGameComplete(bool display){
		HideAllMenus();	
		if (display) {		
			GameCompleteMenuHolder.gameObject.SetActive(true);
		} 
	}

	/// <summary>
	/// Logins the result.
	/// </summary>
	/// <param name="success">If set to <c>true</c> success.</param>
	void LoginResult(bool success) {
		if (success) {
			// save game has been loaded, update any details we need to.
		}
	}

	/// <summary>
	/// Pauses the game.
	/// </summary>
	void PauseGame() {
		DisplayPauseMenu(true);
	}

	/// <summary>
	/// Resumes the game.
	/// </summary>
	void ResumeGame() {
		DisplayInGameButtons(true);
	}

	/// <summary>
	/// Restarts the game.
	/// </summary>
	void RestartGame() {
		#if UNITY_EDITOR
		Debug.Log("TODO: Restart level");
		#endif
//		StartCoroutine(LoadLevel());
	}

	/// <summary>
	/// Displays the level complete.
	/// </summary>
	/// <param name="display">If set to <c>true</c> display.</param>
	void DisplayLevelComplete(bool display) {
		HideAllMenus();	
		if (display) {		
			LevelCompleteMenuHolder.gameObject.SetActive(true);
		} 
	}

	/// <summary>
	/// Displaies the game over.
	/// </summary>
	/// <param name="display">If set to <c>true</c> display.</param>
	void DisplayGameOver(bool display) {
		HideAllMenus();	
		if (display) {		
			GameOverMenuHolder.gameObject.SetActive(true);
		} 
	}

	/// <summary>
	/// Displaies the pause button.
	/// </summary>
	/// <param name="display_it">If set to <c>true</c> display it.</param>
	void DisplayInGameButtons(bool displayButton) {

		HideAllMenus();	
//		if (pause_button==null)return;
		if (displayButton) {		
			buttonsHolder.gameObject.SetActive(true);
			inGameMenuButtonsAnimator.Play("AnimateInGameMenuButtonsIn");

		}
	}

	/// <summary>
	/// Displaies the pause menu.
	/// </summary>
	/// <param name="display">If set to <c>true</c> display.</param>
	void DisplayPauseMenu(bool display) {
		HideAllMenus();	
		if (display) {		
			pauseMenuHolder.gameObject.SetActive(true);
		} 
	}
}
