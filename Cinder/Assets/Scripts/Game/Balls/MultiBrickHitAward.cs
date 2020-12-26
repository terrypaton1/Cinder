using UnityEngine;

public class MultiBrickHitAward : MonoBehaviour
{
    public static void EvaluateBrickHits(int brickQuantity)
    {
        switch (brickQuantity)
        {
            case 7:
                AwardMultiHit(SoundList.MultiHit, Points.MultiHit1, Message.MultiHit);
                break;
            case 12:
                AwardMultiHit(SoundList.MultiHitBrilliant, Points.MultiHit2, Message.Brilliant);
                break;
            case 16:
                AwardMultiHit(SoundList.MultiHitExcellent, Points.MultiHit3, Message.Excellent);
                break;
            case 20:
                AwardMultiHit(SoundList.MultiHitAwesome, Points.MultiHit4, Message.Awesome);
                break;
            case 25:
                AwardMultiHit(SoundList.MultiHitWild, Points.MultiHit5, Message.Wild);
                break;
            case 30:
                AwardMultiHit(SoundList.MultiHitMadness, Points.MultiHit6, Message.Madness);
                break;
            case 40:
                AwardMultiHit(SoundList.MultiHitInsane, Points.MultiHit7, Message.Insane);
                break;
            case 60:
                AwardMultiHit(SoundList.MultiHitUnbelievable, Points.MultiHit8, Message.Unbelievable);
                break;
        }
    }

    private static void AwardMultiHit(SoundList sound, int points, string awardedMessage)
    {
        CoreConnector.SoundManager.PlaySound(sound);
        CoreConnector.GameManager.scoreManager.PointsCollected(points);
        CoreConnector.GameUIManager.gameMessages.DisplayInGameMessage(awardedMessage);
    }
}