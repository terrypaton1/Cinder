using UnityEngine;

[ExecuteInEditMode]
public class Level : MonoBehaviour
{
    [SerializeField]
    public BrickBase[] bricks;

    [SerializeField]
    public NonBrick[] nonBricks;

    protected void OnEnable()
    {
        bricks = GetComponentsInChildren<BrickBase>();
        nonBricks = GetComponentsInChildren<NonBrick>();
    }
}