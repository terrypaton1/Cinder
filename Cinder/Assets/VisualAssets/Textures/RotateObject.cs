using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField]
    protected float rotationSpeed = 5.0f;

    protected void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}