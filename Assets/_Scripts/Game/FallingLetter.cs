using UnityEngine;

public class FallingLetter : BaseObject
{
    public string letter = "display0";
    public GameObject display0;
    public GameObject display1;
    public GameObject display2;
    public GameObject display3;
    public GameObject display4;

    /// <summary>
    /// The points value.
    /// </summary>
    int _pointsValue;

    /// <summary>
    /// The collider.
    /// </summary>
    Collider2D _collider;

    /// <summary>
    /// The visual objects.
    /// </summary>
    public GameObject visualObjects;

    /// <summary>
    /// The Rigidbody2D
    /// </summary>
    Rigidbody2D rigid2D;

    /// <summary>
    /// The current falling speed.
    /// </summary>
    float currentFallingSpeed = 0;

    /// <summary>
    /// The maximum falling speed. Updated from GameVariables on setup
    /// </summary>
    float maximumFallingSpeed;

    /// <summary>
    /// The is falling.
    /// </summary>
    bool isFalling = false;

    void OnEnable()
    {
        Messenger.AddListener(MenuEvents.LevelComplete, LevelComplete);
        Messenger.AddListener(GlobalEvents.LifeLost, LifeLost);
    }

    void OnDisable()
    {
        Messenger.RemoveListener(MenuEvents.LevelComplete, LevelComplete);
        Messenger.RemoveListener(GlobalEvents.LifeLost, LifeLost);
    }

    /// <summary>
    /// The player loses a life
    /// </summary>
    void LifeLost()
    {
        if (!isFalling)
            return;

        BONUSManager.instance.LetterWasNotCollected(this);
        Messenger<ParticleTypes, Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.DestroyFallingItems,
            transform.position, MessengerMode.DONT_REQUIRE_LISTENER);
        Disable();
    }

    /// <summary>
    /// Level complete. Stop the falling powerup
    /// </summary>
    void LevelComplete()
    {
    }

    void FixedUpdate()
    {
        if (isFalling)
        {
            currentFallingSpeed = Mathf.Lerp(currentFallingSpeed, maximumFallingSpeed, Time.deltaTime * 2);
            Vector2 position = rigid2D.transform.position;
            position.y -= currentFallingSpeed;
            rigid2D.MovePosition(position);
        }
    }

    public void Setup(int newPointsValue, string _letter)
    {
        maximumFallingSpeed = GameVariables.maximumFallingLetterItemSpeed;
        _pointsValue = newPointsValue;
        letter = _letter;
        // disable all pointsDisplay
        HideAllVisualObjects();
        SetupVisualDisplay();
//		Debug.Log("setup falling points:" + _pointsValue);
        _collider = GetComponentInChildren<Collider2D>();
        rigid2D = GetComponent<Rigidbody2D>();
        Disable();
//		Debug.Log("colliders:" + colliders.Length);
        if (_collider == null)
        {
            Debug.LogError("Didn't find a colliders for falling points");
        }

        currentFallingSpeed = 0;
        isFalling = false;
    }

    void SetupVisualDisplay()
    {
        switch (letter)
        {
            case "B":
                display0.SetActive(true);
                break;
            case "R":
                display1.SetActive(true);
                break;
            case "I":
                display2.SetActive(true);
                break;
            case "C":
                display3.SetActive(true);
                break;
            case "K":
                display4.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// Starts the falling.
    /// </summary>
    /// <param name="position">Position.</param>
    public void StartFalling(Vector3 position)
    {
        // start falling, slowly at first, then faster
        //		Debug.Log("start falling");
        SetupVisualDisplay();
        transform.position = position;
        _collider.enabled = true;
        currentFallingSpeed = 0;
        isFalling = true;
    }

    /// <summary>
    /// Disables the falling powerup.
    /// </summary>
    public void Disable()
    {
//		Debug.Log("DisableFallingPoint");
        HideAllVisualObjects();
        _collider.enabled = false;
        isFalling = false;
        currentFallingSpeed = 0;
    }

    /// <summary>
    /// Hides all visual objects.
    /// </summary>
    void HideAllVisualObjects()
    {
        display0.SetActive(false);
        display1.SetActive(false);
        display2.SetActive(false);
        display3.SetActive(false);
        display4.SetActive(false);
    }

    /// <summary>
    /// Raises the collision enter2D event.
    /// </summary>
    /// <param name="collision">Collision.</param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("playersbat"))
        {
//			Debug.Log("Points Collected by Player");
            Messenger<int>.Broadcast(GlobalEvents.PointsCollected, _pointsValue, MessengerMode.DONT_REQUIRE_LISTENER);
            BONUSManager.instance.LetterCollected(this);
            if (collision.contacts.Length > 0)
            {
                Messenger<ParticleTypes, Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect,
                    ParticleTypes.FallingPointsCollected, collision.contacts[0].point,
                    MessengerMode.DONT_REQUIRE_LISTENER);
            }

            Disable();
        }

        if (collision.gameObject.CompareTag("deadzone"))
        {
            BONUSManager.instance.LetterWasNotCollected(this);
            Disable();
        }
    }
}