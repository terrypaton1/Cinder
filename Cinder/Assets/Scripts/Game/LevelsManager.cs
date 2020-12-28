using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    [SerializeField]
    public Level[] allLevels;

    private void Start()
    {
        CoreConnector.LevelsManager = this;
        HideAllLevels();
    }

    private void OnDisable()
    {
        CoreConnector.LevelsManager = null;
    }

    public void DisplayLevel(int levelNumber)
    {
        HideAllLevels();
        // levels are stored as base zero
        levelNumber -= 1;
        levelNumber = Mathf.Clamp(levelNumber, 0, allLevels.Length);

        var level = allLevels[levelNumber];
        level.Show();
    }

    public void HideAllLevels()
    {
        foreach (var level in allLevels)
        {
            level.Hide();
        }
    }

    public List<BrickBase> GetBricksForCurrentLevel()
    {
        var levelNumber = GetCurrentLevelNumber();
        var level = allLevels[levelNumber];
        var brickList = new List<BrickBase>(level.bricks);
        return brickList;
    }

    public List<NonBrick> GetNonBricksForCurrentLevel()
    {
        var levelNumber = GetCurrentLevelNumber();
        var level = allLevels[levelNumber];

        var brickList = new List<NonBrick>(level.nonBricks);
        return brickList;
    }

    private static int GetCurrentLevelNumber()
    {
        var levelNumber = PlayerPrefs.GetInt(DataVariables.currentLevel);
        // levels are stored as base zero
        levelNumber -= 1;
        return levelNumber;
    }
}