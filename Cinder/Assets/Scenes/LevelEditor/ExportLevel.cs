using System;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ExportLevel : MonoBehaviour
{
    [SerializeField]
    private LevelSettings levelSettings;

    private Brick[] bricks;
    private LevelData levelData;

    public void OutputLevelContents()
    {
        Debug.Log("OutputLevelContents");
        var foundLevels = Resources.FindObjectsOfTypeAll<Level>();
        bricks = Resources.FindObjectsOfTypeAll<Brick>();
        foreach (var level in foundLevels)
        {
            int levelNumber = GetLevelNumberFromName(level.name);

            levelData = new LevelData(levelNumber);
            ProcessBricks(level.name, level.bricks);
            ProcessNonBricks(level.name, level.nonBricks, ref levelData);

            string levelJsonData = JsonUtility.ToJson(levelData);
            Debug.Log(levelJsonData);
            StoreLevelData(levelNumber, levelJsonData);
        }
    }

    int GetLevelNumberFromName(String levelName)
    {
        var initalOffset = "LEVEL_".Length;
        var numberString = levelName.Substring(initalOffset, levelName.Length - initalOffset);
        var levelNumber = int.Parse(numberString);
        Debug.Log($"LEVEL:{levelNumber}");
        return levelNumber;
    }

    private void ProcessNonBricks(string levelName, NonBrick[] levelNonBricks, ref LevelData levelData)
    {
        var levelNumber = GetLevelNumberFromName(levelName);
        Debug.Log($"LEVEL:{levelNumber}");
        foreach (var nonBrick in levelNonBricks)
        {
            if (nonBrick == null)
            {
                continue;
            }

            TestNonBrickForTypeDefined(nonBrick);
            StoreNonBrick(nonBrick);
        }

        Debug.Log($"Stored:{levelData.bricks.Count}");
    }

    private void ProcessBricks(string levelName, BrickBase[] levelBricks)
    {
        var initalOffset = "LEVEL_".Length;
        var numberString = levelName.Substring(initalOffset, levelName.Length - initalOffset);
        var levelNumber = int.Parse(numberString);
        Debug.Log($"LEVEL:{levelNumber}");

        foreach (var levelBrick in levelBricks)
        {
            if (levelBrick == null)
            {
                continue;
            }

            TestBrickForTypeDefined(levelBrick);
            StoreBrick(levelBrick);
        }

        Debug.Log($"Stored:{levelData.bricks.Count}");
    }

    private void StoreLevelData(int levelNumber, string levelJsonData)
    {
        while (levelSettings.levelDataStorage.Count < levelNumber + 1)
        {
            levelSettings.levelDataStorage.Add(new LevelDataStorage());
        }

        levelSettings.levelDataStorage[levelNumber].levelNumber = levelNumber;
        levelSettings.levelDataStorage[levelNumber].jsonData = levelJsonData;
        EditorUtility.SetDirty(levelSettings);
    }

    private void StoreNonBrick(NonBrick nonBrick)
    {
        var position = nonBrick.transform.position;
        position = RoundVector(position);
        var newData = new NonBrickData
        {
            position = position,
            eulerRotation = nonBrick.transform.localEulerAngles,
            scale = nonBrick.transform.localScale,
            nonBrickType = nonBrick.nonBrickType
        };

        levelData.nonBricks.Add(newData);
    }

    private void StoreBrick(BrickBase levelBrick)
    {
        var position = levelBrick.transform.position;
        position = RoundVector(position);
        var newData = new BrickData
        {
            position = position,
            eulerRotation = levelBrick.transform.localEulerAngles,
            scale = levelBrick.transform.localScale,
            brickType = levelBrick.brickType
        };

        levelData.bricks.Add(newData);
    }

    private Vector3 RoundVector(Vector3 position)
    {
        position.x = Mathf.RoundToInt(position.x * 1000.0f) / 1000.0f;
        position.y = Mathf.RoundToInt(position.y * 1000.0f) / 1000.0f;
        position.z = Mathf.RoundToInt(position.z * 1000.0f) / 1000.0f;
        return position;
    }

    private void TestNonBrickForTypeDefined(NonBrick nonBrick)
    {
        if (nonBrick.nonBrickType != NonBrickType.Undefined)
        {
            return;
        }

        var name = nonBrick.name;
        var index = name.IndexOf("(");
        if (index > 0)
        {
            name = name.Substring(0, index - 1);
        }

        // Debug.Log(name);
        var foundNonBrick = (NonBrickType) Enum.Parse(typeof(NonBrickType), name, true);

        nonBrick.nonBrickType = foundNonBrick;
        //Debug.Log($"name:{name} foundBrick:{foundBrick}");
    }

    private void TestBrickForTypeDefined(BrickBase levelBrick)
    {
        if (levelBrick.brickType != BrickType.Undefined)
        {
            return;
        }

        var name = levelBrick.name;
        var index = name.IndexOf("(");
        if (index > 0)
        {
            name = name.Substring(0, index - 1);
        }

        // Debug.Log(name);
        var foundBrick = (BrickType) Enum.Parse(typeof(BrickType), name, true);

        levelBrick.brickType = foundBrick;
        //Debug.Log($"name:{name} foundBrick:{foundBrick}");
    }
}