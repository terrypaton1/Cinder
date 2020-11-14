using UnityEngine;
using System.Collections;

public class TouchPosition : BaseObject
{
    [SerializeField]
    protected Camera camera_ref;

    [SerializeField]
    protected UICamera _uiCamera;

    private bool gameIsPaused;

    private Vector2 lastTouchPosition = Vector2.zero;
    private static TouchPosition s_Instance;

    protected void OnEnable()
    {
        Messenger.AddListener(GlobalEvents.PauseGame, PauseGame);
        Messenger.AddListener(GlobalEvents.ResumeGame, ResumeGame);
    }

    protected void OnDisable()
    {
        Messenger.RemoveListener(GlobalEvents.PauseGame, PauseGame);
        Messenger.RemoveListener(GlobalEvents.ResumeGame, ResumeGame);
    }

    public void PauseGame()
    {
//		Debug.Log("*** game paused");
        gameIsPaused = true;
    }

    private void ResumeGame()
    {
        StartCoroutine(ResumeGameSequence());
    }

    private IEnumerator ResumeGameSequence()
    {
        yield return new WaitForSeconds(.1f);
        gameIsPaused = false;
    }

    public Vector2 GetPlayersPosition()
    {
        if (gameIsPaused) return lastTouchPosition;
#if UNITY_EDITOR
        Vector3 p = camera_ref.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
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

    protected void OnDestroy()
    {
        s_Instance = null;
    }

    public static TouchPosition instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(TouchPosition)) as TouchPosition;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
                Debug.Log("Could not locate an TouchPosition object!");
//				UnityEngine.Debug.Break();
            }

            return s_Instance;
        }
    }

    protected void OnApplicationQuit()
    {
        s_Instance = null;
    }
}