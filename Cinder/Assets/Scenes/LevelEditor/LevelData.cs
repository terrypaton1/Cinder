using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    [Serializable]
    public class BrickData
    {
        public Vector3 position;
        public Vector3 eulerRotation;
        public Vector3 scale;
        public BrickType brickType;
    }

    [Serializable]
    public class NonBrickData
    {
        public Vector3 position;
        public Vector3 eulerRotation;
        public Vector3 scale;
        public NonBrickType nonBrickType;
    }

    public LevelData(int number)
    {
        levelNumber = number;
        bricks = new List<BrickData>();
        nonBricks = new List<NonBrickData>();
    }

    public int levelNumber;
    public List<BrickData> bricks;
    public List<NonBrickData> nonBricks;
}