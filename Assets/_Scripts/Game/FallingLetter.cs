using UnityEngine;

public class FallingLetter : BaseObject
{
    public string letter = "display0";
    public GameObject display0;
    public GameObject display1;
    public GameObject display2;
    public GameObject display3;
    public GameObject display4;

    private int _pointsValue;

    private Collider2D _collider;

    public GameObject visualObjects;

    private Rigidbody2D rigid2D;

    private float currentFallingSpeed = 0;

    private float maximumFallingSpeed;

    private bool isFalling = false;

    protected void OnEnable()
    {
        Messenger.AddListener(MenuEvents.LevelComplete, LevelComplete);
        Messenger.AddListener(GlobalEvents.LifeLost, LifeLost);
    }

    protected void OnDisable()
    {
        Messenger.RemoveListener(MenuEvents.LevelComplete, LevelComplete);
        Messenger.RemoveListener(GlobalEvents.LifeLost, LifeLost);
    }

    private void LifeLost()
    {
        if (!isFalling)
            return;

        BONUSManager.instance.LetterWasNotCollected(this);
        Messenger<ParticleTypes, Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.DestroyFallingItems,
            transform.position, MessengerMode.DONT_REQUIRE_LISTENER);
        Disable();
    }

    private void LevelComplete()
    {
    }

    protected void FixedUpdate()
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

    private void SetupVisualDisplay()
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

    private void Disable()
    {
//		Debug.Log("DisableFallingPoint");
        HideAllVisualObjects();
        _collider.enabled = false;
        isFalling = false;
        currentFallingSpeed = 0;
    }

    private void HideAllVisualObjects()
    {
        display0.SetActive(false);
        display1.SetActive(false);
        display2.SetActive(false);
        display3.SetActive(false);
        display4.SetActive(false);
    }

    protected void OnCollisionEnter2D(Collision2D collision)
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