using UnityEngine;
using System.Collections;

public class BrickMutltiHitSprites : MonoBehaviour
{
    /// The two hit sprite, used for displaying how many hits this brick has left
    [SerializeField]
    public GameObject twoHit;

    [SerializeField]
    public GameObject threeHit;

    [SerializeField]
    public GameObject fourHit;

    [SerializeField]
    public GameObject fiveHit;

    [SerializeField]
    public GameObject sixHit;

    public void DisplayHitsLeft(int amountOfHitsToDestroy)
    {
//		Debug.Log("DisplayHitsLeft");
        HideAllHitCounterGameObjects();
        switch (amountOfHitsToDestroy)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                twoHit.SetActive(true);
                break;
            case 3:
                threeHit.SetActive(true);
                break;
            case 4:
                fourHit.SetActive(true);
                break;
            case 5:
                fiveHit.SetActive(true);
                break;
            case 6:
                sixHit.SetActive(true);
                break;
        }

        if (amountOfHitsToDestroy > 6)
        {
            // this is probably a boss.
            sixHit.SetActive(true);
        }
    }

    private void HideAllHitCounterGameObjects()
    {
        twoHit.SetActive(false);
        threeHit.SetActive(false);
        fourHit.SetActive(false);
        fiveHit.SetActive(false);
        sixHit.SetActive(false);
    }
}