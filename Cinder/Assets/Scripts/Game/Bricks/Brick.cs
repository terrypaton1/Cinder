using UnityEngine;

public class Brick : BrickBase
{
    private int resetCounter;

    protected override void StartItemFallingFromDestroyedBrick()
    {
        // check if we should drop a falling points object (or a bonus letter)
        if (CoreConnector.GameManager.bonusManager.BrickShouldDropPoints(transform.position))
        {
            //  fallingPointsReference.StartFalling(transform.position);

            int pointsValue = Points.fallingPointValues1;
            int category = 0;

            if (resetHitsToDestroyCount > 1)
            {
                pointsValue = Points.fallingPointValues2;
                category = 1;
            }

            if (resetHitsToDestroyCount > 3)
            {
                pointsValue = Points.fallingPointValues3;
                category = 2;
            }

            if (resetHitsToDestroyCount > 5)
            {
                pointsValue = Points.fallingPointValues4;
                category = 3;
            }

            CoreConnector.GameManager.fallingObjectsManager.AddFallingPoints(transform.position, pointsValue, category);
        }
    }

    public override void Shake(float amount)
    {
        var randomRadius = Random.Range(0.05f, 0.1f) * amount;

        var newLocal = Random.insideUnitSphere * randomRadius;
        newLocal.z = 0;
        visualObjects.transform.localPosition = newLocal;

        newLocal = visualObjects.transform.localEulerAngles;
        newLocal.z = Random.Range(-15f, 15f) * amount;
        visualObjects.transform.localEulerAngles = newLocal;

        resetCounter += 2;
    }

    public override void UpdateLoop()
    {
        if (BrickHasBeenDestroyed)
        {
            return;
        }

        var newPosition =
            Vector3.Lerp(visualObjects.transform.localPosition, Vector3.zero, Time.deltaTime * 10);
        visualObjects.transform.localPosition = newPosition;

        if (resetCounter <= 0)
        {
            return;
        }

        resetCounter--;
        if (resetCounter == 0)
        {
            visualObjects.transform.localEulerAngles = Vector3.zero;
        }
    }
}