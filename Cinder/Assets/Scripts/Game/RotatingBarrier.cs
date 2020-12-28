using UnityEngine;

public class RotatingBarrier : NonBrick
{
    [SerializeField]
    public bool rotateClockwise = true;

    [SerializeField]
    public float speed = 45f;

    [SerializeField]
    protected Rigidbody2D _rigidbody2D;

    protected void FixedUpdate()
    {
        // only run if enabled
        // todo probably wold be better to change this to an object on a pivot
        if (rotateClockwise)
        {
            _rigidbody2D.MoveRotation(_rigidbody2D.rotation + speed * Time.fixedDeltaTime);
        }
        else
        {
            _rigidbody2D.MoveRotation(_rigidbody2D.rotation - speed * Time.fixedDeltaTime);
        }
    }
}