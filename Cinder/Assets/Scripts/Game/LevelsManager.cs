using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    [SerializeField]
    public Level[] levelPrefabs;

    [SerializeField]
    public Level[] allLevels;

    private int loadedLevelCounter;

    private void Start()
    {
        CoreConnector.LevelsManager = this;
    }

    public IEnumerator CacheAllLevelsSequence()
    {
        loadedLevelCounter = 0;
        var totalLevels = levelPrefabs.Length;
        allLevels = new Level[totalLevels];
        // while the levels are being loaded, return false
        while (loadedLevelCounter < totalLevels)
        {
            var prefabRef = levelPrefabs[loadedLevelCounter];
            var instance = Instantiate(prefabRef, transform);
            instance.transform.position = Vector3.zero;
            allLevels[loadedLevelCounter] = instance;
            instance.Hide();

            yield return new WaitForSeconds(0.02f);
            loadedLevelCounter++;
        }
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
        levelNumber = Mathf.Clamp(levelNumber, 0, allLevels.Length - 1);

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

    private static int GetCurrentLevelNumber()
    {
        var levelNumber = PlayerPrefs.GetInt(Constants.currentLevel);
        // levels are stored as base zero
        levelNumber -= 1;
        return levelNumber;
    }

    public List<NonBrick> GetNonBricksForCurrentLevel()
    {
        var levelNumber = GetCurrentLevelNumber();
        var level = allLevels[levelNumber];
        var brickList = new List<NonBrick>(level.nonBricks);
        return brickList;
    }
}