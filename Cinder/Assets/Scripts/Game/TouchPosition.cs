using UnityEngine;
using System.Collections;

public class TouchPosition : BaseObject
{
    [SerializeField]
    protected Camera camera_ref;

    private bool gameIsPaused;
    private Vector2 lastTouchPosition = Vector2.zero;
    private IEnumerator coroutine;

    public void PauseGame()
    {
        gameIsPaused = true;
    }

    public void ResumeGame()
    {
        StopRunningCoroutine();
        coroutine = ResumeGameSequence();
        StartCoroutine(coroutine);
    }

    private IEnumerator ResumeGameSequence()
    {
        yield return new WaitForSeconds(0.1f);
        gameIsPaused = false;
    }

    public Vector2 GetPlayersPosition()
    {
        if (gameIsPaused)
        {
            return lastTouchPosition;
        }
#if UNITY_EDITOR
        var p = camera_ref.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            camera_ref.nearClipPlane));
        lastTouchPosition.x = p.x;
        lastTouchPosition.y = p.y;
        return lastTouchPosition;
#else
		if (Input.touchCount < 1)
			return lastTouchPosition;
		Touch _touch = Input.GetTouch(0);
//		Debug.Log("pos:" + _touch.position);
//		Debug.Log("_touch.pos.x:" + _touch.position.x + " / Input.mousePosition.x:" + Input.mousePosition.x);
		Vector3 p =
 camera_ref.ScreenToWorldPoint(new Vector3(_touch.position.x, _touch.position.y, camera_ref.nearClipPlane));
		lastTouchPosition.x = p.x;
		lastTouchPosition.y = p.y;
		return 	lastTouchPosition;
#endif
    }

    public Camera GetCamera()
    {
        return camera_ref;
    }

    private void StopRunningCoroutine()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }
}