using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Cinder/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    public static bool DrawWallSidesDebug = false;
    public static readonly float HealthReset = 0.5f;

    [SerializeField]
    public Sprite pointsDisplay10;

    [SerializeField]
    public Sprite pointsDisplay50;

    [SerializeField]
    public Sprite pointsDisplay100;

    [SerializeField]
    public Sprite pointsDisplay500;

    [Header("Power up sprites")]
    [SerializeField]
    public Sprite powerupMultiball;

    [SerializeField]
    public Sprite powerupSmallBat;

    [SerializeField]
    public Sprite powerupCrazyBall;

    [SerializeField]
    public Sprite powerupLaser;

    [SerializeField]
    public Sprite powerupShield;

    [SerializeField]
    public Sprite powerupSplitBat;

    [SerializeField]
    public Sprite powerupRandom;

    [SerializeField]
    public Sprite powerupFlameball;

    [SerializeField]
    public Sprite powerupWide;

    [Header("Bonus letters")]
    [SerializeField]
    public Sprite letter_B;

    [SerializeField]
    public Sprite letter_R;

    [SerializeField]
    public Sprite letter_I;

    [SerializeField]
    public Sprite letter_C;

    [SerializeField]
    public Sprite letter_K;

    [SerializeField]
    public AnimationCurve scaleInCurve;

    public Sprite GetLetter(string letter)
    {
        switch (letter)
        {
            case "B":
                return letter_B;
            case "R":
                return letter_R;
            case "I":
                return letter_I;
            case "C":
                return letter_C;
            case "K":
                return letter_K;
        }

        return null;
    }
}