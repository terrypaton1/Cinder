using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    private Vector3 position;
    private float rotation;

    private const float Range = 0.2f;

    protected void Start()
    {
        position = transform.position;
    }

    protected void Update()
    {
        var radian = Mathf.Deg2Rad * rotation;
        var newPosition = position;
        newPosition.x += Mathf.Cos(radian) * Range;
        newPosition.y += Mathf.Sin(radian) * Range;
        transform.position = newPosition;
        rotation += Time.deltaTime * 10.0f;
    }
}