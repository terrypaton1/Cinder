using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "Cinder/LevelSettings", order = 1)]
public class LevelSettings : ScriptableObject
{
    [SerializeField]
    public List<LevelDataStorage> levelDataStorage;

    [SerializeField]
    public BrickBase[] brickPrefabs;

    [SerializeField]
    public List<NonBrick> nonBrickPrefabs;
}

[Serializable]
public class LevelDataStorage
{
    public int levelNumber;
    public string jsonData;
}