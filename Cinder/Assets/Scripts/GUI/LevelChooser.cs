using UnityEngine;

public class LevelChooser : UIScreen
{
    [SerializeField]
    protected LevelButton[] levelButtons;

    [SerializeField]
    private BrickColors brickColors;
    
    protected void OnEnable()
    {
        var counter = 1;
        foreach (var levelButton in levelButtons)
        {
            levelButton.levelNumber = counter;
            var color = brickColors.GetColorWithIndex(counter);
            counter++;

            levelButton.ApplyColor(color);
        }
    }
}