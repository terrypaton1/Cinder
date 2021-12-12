using System.Collections.Generic;

public class LevelData
{
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