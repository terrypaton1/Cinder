using UnityEngine;

public class Backgrounds : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer background;

    [SerializeField]
    private Sprite[] backgrounds;

    public void DisplayForLevel(int level)
    {
        var backgroundId = Mathf.FloorToInt(level /5.0f);
        backgroundId = Mathf.Clamp(backgroundId, 0, backgrounds.Length);
        
        var sprite = backgrounds[backgroundId];
        background.sprite = sprite;
    }
}