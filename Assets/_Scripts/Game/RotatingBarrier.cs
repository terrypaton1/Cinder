using UnityEngine;
using UnityEngine.Serialization;

public class RotatingBarrier : MonoBehaviour
{
    [SerializeField]
    public bool rotateClockwise = true;

    [SerializeField]
    public float speed = 45f;

    [FormerlySerializedAs("_rigidbody2D")]
    [SerializeField]
    private Rigidbody2D rigidbody2D;

    protected void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected void FixedUpdate()
    {
        if (rotateClockwise)
        {
            rigidbody2D.MoveRotation(rigidbody2D.rotation + speed * Time.fixedDeltaTime);
        }
        else
        {
            rigidbody2D.MoveRotation(rigidbody2D.rotation - speed * Time.fixedDeltaTime);
        }
    }
}