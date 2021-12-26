using Scenes.Effects.Dash;
using UnityEngine;

public class FallingObjectsManager : MonoBehaviour
{
    [SerializeField]
    public Dash dash;

    public void HideAll()
    {
        dash.HideAll(FallingPoints.DashID);
    }

    public void AddFallingPoints(Vector3 position, int value, int category)
    {
        var falling2 = (FallingPoints) dash.GetDashObject(FallingPoints.DashID);
        falling2.Setup(value, category);
        falling2.StartFalling(position);
    }


    public void AddFallingPowerUp(Vector3 position, PowerupType newPowerUpType)
    {
        var fallingPowerUp = dash.GetDashObject(newPowerUpType.ToString());
        fallingPowerUp.StartFalling(position);
    }

    public void LevelComplete()
    {
        dash.LevelComplete();
    }

    public void LifeLost()
    {
        dash.LifeLost();
    }

    public void Initialize()
    {
        dash.Initialize();
    }
}