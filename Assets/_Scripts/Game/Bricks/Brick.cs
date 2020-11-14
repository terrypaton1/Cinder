using UnityEngine;

#if UNITY_EDITOR
[SelectionBase]
[ExecuteInEditMode]
#endif
public class Brick : BrickBase
{
    FallingPoints fallingPointsReference;

    int resetClounter;

    void Awake()
    {
        UpdateAmountOfHitsLeftDisplay();
        _brickAnimation = GetComponent<Animator>();
    }

    void Update()
    {
        visualObjects.transform.localPosition =
            Vector3.Lerp(visualObjects.transform.localPosition, Vector3.zero, Time.deltaTime * 10);

        if (resetClounter > 0)
        {
            resetClounter--;
            if (resetClounter == 0)
            {
                visualObjects.transform.localEulerAngles = Vector3.zero;
                ;
            }
        }
    }

    void OnEnable()
    {
        Messenger.AddListener(MenuEvents.RestartGame, ResetBrick);
        Messenger.AddListener(GlobalEvents.ActivateFlameBall, ActivateFlameBall);
        Messenger.AddListener(GlobalEvents.DisableFlameBall, ApplyNormalLayers);
        Messenger<float>.AddListener(GlobalEvents.ShakeGame, ShakeGame);
    }

    protected void OnDisable()
    {
        Messenger.RemoveListener(MenuEvents.RestartGame, ResetBrick);
        Messenger.RemoveListener(GlobalEvents.ActivateFlameBall, ActivateFlameBall);
        Messenger<float>.RemoveListener(GlobalEvents.ShakeGame, ShakeGame);
    }

    public override void SetupFallingPointObject(FallingPoints _fallingPointObject)
    {
//		Debug.Log("SetupFallingPointObject");
        fallingPointsReference = _fallingPointObject;
        // based on the brick type etc we setup the falling points object
        var pointsValue = GameVariables.fallingPointValues1;
        var category = 0;
        if (amountOfHitsToDestroy > 1)
        {
            pointsValue = GameVariables.fallingPointValues2;
            category = 1;
        }

        if (amountOfHitsToDestroy > 3)
        {
            pointsValue = GameVariables.fallingPointValues3;
            category = 2;
        }

        if (amountOfHitsToDestroy > 5)
        {
            pointsValue = GameVariables.fallingPointValues4;
            category = 3;
        }

        fallingPointsReference.Setup(pointsValue, category);
        fallingPointsReference.Disable();
    }

    public override void ResetBrick()
    {
        fallingPointsReference.Disable();
        base.ResetBrick();
    }

    protected override void StartItemFallingFromDestroyedBrick()
    {
        // check if we should drop a falling points object (or a bonus letter)
        if (BONUSManager.instance.BrickShouldDropPoints(transform.position))
        {
            fallingPointsReference.StartFalling(transform.position);
        }
    }

    protected void ShakeGame(float amount)
    {
        //		Debug.Log ("shake");BrickHitByBall
        var randomRadius = Random.Range(0.05f, 0.1f) * amount;
        var newLocal = Random.insideUnitSphere * randomRadius;
        newLocal.z = 0;
        visualObjects.transform.localPosition = newLocal;
        newLocal = visualObjects.transform.localEulerAngles;
        newLocal.z = Random.Range(-15f, 15f) * amount;
        visualObjects.transform.localEulerAngles = newLocal;
        //	transform.position
        resetClounter += 2;
    }
}