using UnityEngine;

[System.Serializable]
public class BrickColorSetting
{
    [SerializeField]
    public float saturationMin = 0.0f;

    [SerializeField]
    public float saturationMax = 1.0f;

    [SerializeField]
    public float valueMin = 0.0f;

    [SerializeField]
    public float valueMax = 1.0f;

    [SerializeField, Range(0.00f, 1.00f)]
    public float hueMinimum = 0.0f;

    [SerializeField, Range(0.00f, 1.00f)]
    public float hueMaximum = 1.0f;

    [SerializeField]
    public Color colorMin;

    [SerializeField]
    public Color colorMax;

    public void MixColors()
    {
        colorMin = Color.HSVToRGB(hueMinimum, saturationMin, valueMin, true);
        colorMax = Color.HSVToRGB(hueMaximum, saturationMax, valueMax, true);
    }
}