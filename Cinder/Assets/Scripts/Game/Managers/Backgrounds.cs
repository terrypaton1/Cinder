using UnityEngine;

public class Backgrounds : MonoBehaviour
{
    [SerializeField]
    protected SpriteRenderer background;

    [SerializeField]
    protected Sprite[] backgrounds;

    public void DisplayForLevel(int level)
    {
        var backgroundId = Mathf.FloorToInt(level / 5.0f);
        backgroundId = Mathf.Clamp(backgroundId, 0, backgrounds.Length - 1);

        var sprite = backgrounds[backgroundId];
        background.sprite = sprite;
    }
}