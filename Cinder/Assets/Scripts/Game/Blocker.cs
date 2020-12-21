using UnityEngine;

public class Blocker : MonoBehaviour
{
    protected void Awake()
    {
#if UNITY_EDITOR
        Debug.Log("using blocker to stop ball from going out");
#else
	Destroy(gameObject);
#endif
    }
}