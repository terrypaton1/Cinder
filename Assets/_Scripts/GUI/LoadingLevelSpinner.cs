using UnityEngine;

public class LoadingLevelSpinner : MonoBehaviour
{
    private readonly Vector3 rotateSpeed = new Vector3(0, 0, -540);

    protected void Update()
    {
        transform.Rotate(rotateSpeed * Time.deltaTime);
    }
}