using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Cinder/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    public Sprite pointsDisplay10;

    [SerializeField]
    public Sprite pointsDisplay50;

    [SerializeField]
    public Sprite pointsDisplay100;

    [Header("Power up sprites")]
    [SerializeField]
    public Sprite powerUpMultiBall;

    [SerializeField]
    public Sprite powerUpSmallBat;

    [SerializeField]
    public Sprite powerUpCrazyBall;

    [SerializeField]
    public Sprite powerUpLaser;

    [SerializeField]
    public Sprite powerUpShield;

    [SerializeField]
    public Sprite powerUpSplitBat;

    [SerializeField]
    public Sprite powerUpRandom;

    [SerializeField]
    public Sprite powerUpFlameBall;

    [SerializeField]
    public Sprite powerUpWide;

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