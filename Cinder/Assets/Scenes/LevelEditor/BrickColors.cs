using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "BrickColors", menuName = "ScriptableObjects/BrickColors", order = 1)]
public class BrickColors : ScriptableObject
{
    [SerializeField]
    private BrickColorSetting[] colorSettings;

    private void OnEnable()
    {
        foreach (var setting in colorSettings)
        {
            setting.MixColors();
        }
    }

    public Color GetColorWithIndex(int index)
    {
        if (index < 0)
        {
            return Color.white;
        }

        var rounded = Mathf.RoundToInt(index / 6.0f);
        if (rounded < colorSettings.Length)
        {
            var setting = colorSettings[rounded];
            return Random.ColorHSV(
                setting.hueMinimum,
                setting.hueMaximum,
                setting.saturationMin,
                setting.saturationMax,
                setting.valueMin,
                setting.valueMax);
        }

        Debug.Log($"NOT FOUND:{rounded}");
        return Random.ColorHSV();
    }

    public Color GetBrickColor(BrickBase brick)
    {
        var index = brickTypesToColor.IndexOf(brick.brickType);
        if (index < 0)
        {
            return Color.white;
        }

        return GetColorWithIndex(index);
    }

    private List<BrickType> brickTypesToColor = new List<BrickType>
    {
        BrickType.Brick1A,
        BrickType.Brick1B,
        BrickType.Brick1C,
        BrickType.Brick2A,
        BrickType.Brick2B,
        BrickType.Brick2C,
        BrickType.Brick3A,
        BrickType.Brick3B,
        BrickType.Brick3C,
        BrickType.Brick4A,
        BrickType.Brick4B,
        BrickType.Brick4C,
        BrickType.Brick5A,
        BrickType.Brick5B,
        BrickType.Brick5C,
        BrickType.Brick6A,
        BrickType.Brick6B,
        BrickType.Brick6C,
        BrickType.Brick7A,
        BrickType.Brick7B,
        BrickType.Brick7C,
        BrickType.Brick8A,
        BrickType.Brick8B,
        BrickType.Brick8C,
        BrickType.Brick9A,
        BrickType.Brick9B,
        BrickType.Brick9C,
        BrickType.Brick10A,
        BrickType.Brick10B,
        BrickType.Brick10C,
        BrickType.Brick11A,
        BrickType.Brick11B,
        BrickType.Brick11C,
        BrickType.Brick12A,
        BrickType.Brick12B,
        BrickType.Brick12C
    };
}