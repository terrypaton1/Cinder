using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{

    [SerializeField]
    public Level[] allLevels;

    private int loadedLevelCounter;

    [SerializeField]
    private List<BrickBase> brickList;

    private void Start()
    {
        CoreConnector.LevelsManager = this;
    }

    private void OnDisable()
    {
        CoreConnector.LevelsManager = null;
    }

    public void DisplayLevel(int levelNumber)
    {
        CoreConnector.TestLoadLevel.UnLoadAllLevels();
        brickList = CoreConnector.TestLoadLevel.DisplayLevel(levelNumber);
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
        return CoreConnector.TestLoadLevel.currentLevelsBricks;
    }

    public List<NonBrick> GetNonBricksForCurrentLevel()
    {
        return CoreConnector.TestLoadLevel.currentNonLevelsBricks;
    }
}