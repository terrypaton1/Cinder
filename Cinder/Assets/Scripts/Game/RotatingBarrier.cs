using UnityEngine;
using UnityEngine.Serialization;

public class RotatingBarrier : NonBrick
{
    [SerializeField]
    public bool rotateClockwise = true;

    [SerializeField]
    public float speed = 45.0f;

    [FormerlySerializedAs("_rigidbody2D")]
    [SerializeField]
    protected Rigidbody2D rigid2D;

    protected void FixedUpdate()
    {
        if (!spriteRenderer.enabled)
        {
            // only run if enabled
            return;
        }

        // todo probably wold be better to change this to an object on a pivot
        if (rotateClockwise)
        {
            rigid2D.MoveRotation(rigid2D.rotation + speed * Time.fixedDeltaTime);
        }
        else
        {
            rigid2D.MoveRotation(rigid2D.rotation - speed * Time.fixedDeltaTime);
        }
    }
}