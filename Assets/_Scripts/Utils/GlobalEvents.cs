public static class AchievementIDs {
	public const string SupportedDeveloper = "CgkI0P-w_8QBEAIQCQ";
	public const string BeatLevel5Boss = "CgkI0P-w_8QBEAIQAQ";
	public const string BeatLevel10Boss = "CgkI0P-w_8QBEAIQAg";
	public const string BeatLevel20Boss = "CgkI0P-w_8QBEAIQAw";
	public const string BeatLevel30Boss = "CgkI0P-w_8QBEAIQBA";
	public const string BeatLevel40Boss = "CgkI0P-w_8QBEAIQBQ";
	public const string BeatLevel50Boss = "CgkI0P-w_8QBEAIQBg";
	public const string BeatLevel60Boss = "CgkI0P-w_8QBEAIQBw";
	public const string BeatLevel66Boss = "CgkI0P-w_8QBEAIQCA";
	public const string Break100Bricks = "CgkI0P-w_8QBEAIQCg";
	public const string Break1000Bricks = "CgkI0P-w_8QBEAIQCw";
	public const string Break10000Bricks = "CgkI0P-w_8QBEAIQDA";
}
//	Messenger<string,float>.Broadcast(SocialEvents.ReportAchievementProgress, 	AchievementIDs.SupportedDeveloper, 100);

public static class SocialEvents {
	/// <summary> The signout. </summary>
	public const string Signout = "Signout";
	/// <summary> The login user. </summary>
	public const string LoginUser = "LoginUser";
	/// <summary> The show high scores. </summary>
	public const string ShowHighScores = "ShowHighScores";
	/// <summary> The show achievements. </summary>
	public const string ShowAchievements = "ShowAchievments";
	/// <summary> The save the players game. </summary>
	public const string SaveThePlayersGame = "SaveThePlayersGame";
	/// <summary> The report achievement progress. </summary>
	public const string ReportAchievementProgress = "ReportAchievementProgress";
	/// <summary> The report score. </summary>
	public const string ReportScore = "ReportScore";
	/// <summary> The confirm signout of social. </summary>
	public const string ConfirmSignoutOfSocial = "ConfirmSignoutOfSocial";
	/// <summary> The attempting login. </summary>
	public const string AttemptingLogin = "AttemptingLogin";
}
public static class GlobalEvents {
	/// <summary>
	/// The display boss health bar.
	/// </summary>
	public const string DisplayBossHealthBar = "DisplayBossHealthBar";
	/// <summary>
	/// The hide boss health instantly.
	/// </summary>
	public const string HideBossHealthInstantly = "HideBossHealthInstantly";
	/// <summary>
	/// The hide boss health bar.
	/// </summary>
	public const string HideBossHealthBar = "HideBossHealthBar";


	public const string ShakeGame = "ShakeGame";
	/// <summary>
	/// The purchase made.
	/// </summary>
	public const string PurchaseMade = "PurchaseMade";

	/// <summary> The start level timer. </summary>
	public const string StartLevelTimer = "StartLevelTimer";
	/// <summary> The resume level timer. </summary>
	public const string ResumeLevelTimer = "ResumeLevelTimer";
	/// <summary> The game variables have been updated from server. </summary>
	public const string GameVariablesHaveBeenUpdatedFromServer = "GameVariablesHaveBeenUpdatedFromServer";
	/// <summary> The debug string. </summary>
	public const string DebugString = "DebugString";
	/// <summary> The reload game data. </summary>
	public const string ReloadGameData = "ReloadGameData";
	/// <summary> The data updated. </summary>
	public const string DataUpdated = "DataUpdated";
	/// <summary> The display user interface screen.</summary>
	public const string DisplayUIScreen = "DisplayUIScreen";
	/// <summary> The refresh data. </summary>
	public const string RefreshData = "RefreshData";
	/// <summary> The play sound FX </summary>
	public const string PlaySoundFX = "PlaySoundFX";
	/// <summary> The spawn particle effect. </summary>
	public const string SpawnParticleEffect = "SpawnParticleEffect";
	/// <summary>  pause The game. </summary>
	public const string PauseGame = "PauseGame";
	/// <summary> The resume game. </summary>
	public const string ResumeGame = "ResumeGame";
	/// <summary> quits The game. </summary>
	public const string QuitGame = "QuitGame";
	/// <summary> The game over. </summary>
	public const string GameOver = "GameOver";
	/// <summary> analytics event. </summary>
	public const string AnalyticsEvent = "AnalyticsEvent";
	/// <summary>	The activate powerup. </summary>
	public const string ActivatePowerup = "ActivatePowerup";
	/// <summary> The points collected </summary>
	public const string PointsCollected = "PointsCollected";
	/// <summary> life lost. </summary>
	public const string LifeLost = "LifeLost";
	/// <summary> The display in game message. </summary>
	public const string DisplayInGameMessage = "DisplayInGameMessage";
	/// <summary> The hide in game message instantly. </summary>
	public const string HideInGameMessageInstantly = "HideInGameMessageInstantly";
	/// <summary> The hide in game message.</summary>
	public const string HideInGameMessage = "HideInGameMessage";
	/// <summary> The fire laser. </summary>
	public const string FireLaser = "FireLaser";
	/// <summary> The display powerup bar. </summary>
	public const string DisplayPowerupBar = "DisplayPowerupBar";
	/// <summary> The hide powerup bar instantly. </summary>
	public const string HidePowerupBarInstantly = "HidePowerupBarInstantly";
	/// <summary> The hide powerup bar. </summary>
	public const string HidePowerupBar = "HidePowerupBar";
	/// <summary> The hide all menus. </summary>
	public const string HideAllMenus = "HideAllMenus";
	/// <summary> The disable flame ball. </summary>
	public const string DisableFlameBall = "DisableFlameBall";
	/// <summary> The activate flame ball. </summary>
	public const string ActivateFlameBall = "ActivateFlameBall";
	/// <summary> The login user. </summary>
	public const string LoginUser = "LoginUser";
	/// <summary> The login result. </summary>
	public const string LoginResult = "LoginResult";
	/// <summary> The disable flame ball. </summary>
	public const string DisableCrazyBall = "DisableCrazyBall";
	/// <summary> The activate flame ball. </summary>
	public const string ActivateCrazyBall = "ActivateCrazyBall";
	/// <summary> A brick was destroyed. </summary>
	public const string BrickWasDestroyed = "BrickWasDestroyed";
	/// <summary>The disable freeze player. </summary>
	public const string DisableFreezePlayer = "DisableFreezePlayer";
	/// <summary>The activate freeze player. </summary>
	public const string ActivateFreezePlayer = "ActivateFreezePlayer";
	/// <summary> The stop level scroller. </summary>
	public const string StopLevelScroller = "StopLevelScroller";

}
public static class MenuEvents {
	/// <summary> The restart game. </summary>
	public const string RestartGame = "RestartGame";
	/// <summary> The display_pause_button.</summary>
	public const string DisplayInGameButtons = "DisplayPauseButton";
	/// <summary> The display pause menu. </summary>
	public const string DisplayPauseMenu = "DisplayPauseMenu";
	/// <summary> The display level complete. </summary>
	public const string DisplayLevelComplete = "DisplayLevelComplete";
	/// <summary> The display game complete. </summary>
	public const string DisplayGameComplete = "DisplayGameComplete";
	/// <summary> The display game over. </summary>
	public const string DisplayGameOver = "DisplayGameOver";
	/// <summary> The donate to the developer. </summary>
	public const string DonateToTheDeveloper = "DonateToTheDeveloper";
	/// <summary> The level complete. </summary>
	public const string LevelComplete = "LevelComplete";
	/// <summary> The next level. </summary>
	public const string NextLevel = "NextLevel";
	/// <summary> The display level loader. </summary>
	public const string DisplayLevelLoader = "DisplayLevelLoader";
	/// <summary> The update points display. </summary>
	public const string UpdatePointsDisplay = "UpdatePointsDisplay";
}
