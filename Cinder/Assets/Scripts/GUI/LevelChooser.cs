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
            var random = Random.Range(1.0f, 1.2f);
            var randomScale = new Vector3(random, random, 1.0f);
            //levelButton.transform.localScale = randomScale;

            // var rot = Random.Range(-5.0f, 5.0f);
            // var randomRotation = new Vector3(0, 0, rot);
            //levelButton.transform.localEulerAngles = randomRotation;

            levelButton.ApplyColor(color);
        }
    }
}